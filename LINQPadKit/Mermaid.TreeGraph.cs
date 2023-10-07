using System.Text;

namespace LINQPadKit;

public partial class Mermaid
{
    public class TreeGraph : Mermaid
    {
        public interface ITreeNode
        {
            string Text { get; }
            ITreeNode[] Children { get; }
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
                        sb.AppendLine($"style {empty} fill:#f100,stroke-width:0px");
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
