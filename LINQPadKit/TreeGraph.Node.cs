using LINQPad;
using LINQPad.Controls;
using LINQPadKit.Design;

namespace LINQPadKit
{
    public partial class TreeGraph : Div, IKit
    {
        public class Node : ITreeGraphNode
        {
            public string Text { get; set; }
            public ITreeGraphNode[] Children { get; set; }

            public static Node Parse<T>(T root, Func<T, string> textSelector, Func<T, T[]> childrenSelector)
            {
                var children = childrenSelector(root);
                var node = new Node
                {
                    Text = textSelector(root),
                    Children = children?.Where(x => x is not null).Select(x => Parse(x, textSelector, childrenSelector)).ToArray()
                };
                return node;
            }
        }

    }
}
