using System.Collections;
using System.Text;

namespace LINQPadKit.Remotes;

public partial class Mermaid
{
    public class TreeGraph : Mermaid
    {
        public class TreeNode : ITreeNode, IEnumerable<ITreeNode>
        {
            public string Text { get; }
            private readonly List<ITreeNode> _children = [];
            public IEnumerable<ITreeNode> Children => _children;

            public TreeNode()
            {
            }

            public TreeNode(string text)
            {
                Text = text;
            }

            public void Add(ITreeNode node)
            {
                _children.Add(node);
            }

            public void Clear()
            {
                _children.Clear();
            }

            public IEnumerator<ITreeNode> GetEnumerator()
            {
                return _children.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _children.GetEnumerator();
            }
        }

        public interface ITreeNode
        {
            string Text { get; }
            IEnumerable<ITreeNode> Children { get; }
        }

        private readonly Direction _direction;

        public TreeGraph() : this(Direction.TB)
        {
        }

        public TreeGraph(Direction direction)
        {
            _direction = direction;
        }

        public override void Add(string content)
        {
            throw new NotSupportedException();
        }

        public void Add(ITreeNode node)
        {
            void Build(StringBuilder sb, ITreeNode node)
            {
                if (node.Children is not null)
                {
                    foreach (var child in node.Children)
                    {
                        if (child is not null)
                        {
                            sb.AppendLine($"{node.GetHashCode()}(({node.Text})) --- {child.GetHashCode()}(({child.Text}))");
                            Build(sb, child);
                        }
                        else
                        {
                            var empty = Guid.NewGuid();
                            sb.AppendLine($"{node.GetHashCode()}(({node.Text})) ~~~ {empty}(( ))");
                            sb.AppendLine($"style {empty} fill:transparent,stroke-width:0px");
                        }
                    }
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine($"graph {_direction}");
            Build(sb, node);

            Content = sb.ToString();
        }
    }

}
