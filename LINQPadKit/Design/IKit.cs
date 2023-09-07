using LINQPad.Controls.Core;
using System.Diagnostics;
using System.Reflection;

namespace LINQPadKit.Design
{
    public interface IKit
    {
        abstract static void Import();
        string Instance { get; }
        bool IsRendered { get; }

        void InitailizeInstance();
        void Render();

        Task InitailizeAsync()
        {
            return Task.Run(() =>
            {
                Wait(() => IsRendered);
                InitailizeInstance();
            });
        }

        public void WaitForReady() => Wait(() => Instance is not null);

        void Wait(Func<bool> predicate)
        {
            if (predicate()) return;

            var stop = new Stopwatch();
            stop.Start();
            while (!predicate())
            {
                if (stop.Elapsed > TimeSpan.FromSeconds(5)) throw new TimeoutException();
            }
            stop.Stop();
        }
    }

}
