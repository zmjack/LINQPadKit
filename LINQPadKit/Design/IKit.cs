using LINQPad.Controls.Core;
using System.Diagnostics;

namespace LINQPadKit.Design
{
    public interface IKit
    {
        abstract static void Import();
        string Export { get; }
        bool IsRendered { get; }
        bool Ready { get; set; }

        Task StartRenderTask(HtmlElement element)
        {
            return Task.Run(() =>
            {
                var stop = new Stopwatch();
                stop.Start();
                while (!IsRendered)
                {
                    if (stop.Elapsed > TimeSpan.FromSeconds(10)) throw new TimeoutException();
                }
                stop.Stop();

                element.InvokeScript(false, "eval", $"{Export}.render(document.getElementById('{element.ID}'));");
                Ready = true;
            });
        }

        void WaitReady()
        {
            if (Ready) return;

            var stop = new Stopwatch();
            stop.Start();
            while (!Ready)
            {
                if (stop.Elapsed > TimeSpan.FromSeconds(10)) throw new TimeoutException();
            }
            stop.Stop();
        }
    }

}
