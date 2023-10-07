using System.Diagnostics;

namespace LINQPadKit.Design;

public interface IKit
{
    string Instance { get; }
    bool IsRendered { get; }

    void InitailizeInstance();
    void Render();
}

public static class IKitExtensions
{
    public static Task InitailizeAsync(this IKit @this)
    {
        return Task.Run(() =>
        {
            Wait(() => @this.IsRendered);
            @this.InitailizeInstance();
        });
    }

    public static void WaitForReady(this IKit @this) => Wait(() => @this.Instance is not null);

    private static void Wait(Func<bool> predicate)
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