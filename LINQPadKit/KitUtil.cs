using LINQPad;
using System.Reflection;

namespace LINQPadKit;

internal static class KitUtil
{
    public static void Load(params string[] srcPath)
    {
        var file = srcPath.Last();
        if (file.EndsWith(".js"))
        {
            var path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                Path.Combine("..", "..", "content", "src"),
                Path.Combine(srcPath)
            );
            Util.HtmlHead.AddScriptFromUri(path);
        }
        else if (file.EndsWith(".css"))
        {
            var path = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                Path.Combine("..", "..", "content", "src"),
                Path.Combine(srcPath)
            );
            Util.HtmlHead.AddStyles(File.ReadAllText(path));
        }
        else throw new ArgumentException($"Unsupported file ({file}).");
    }
}
