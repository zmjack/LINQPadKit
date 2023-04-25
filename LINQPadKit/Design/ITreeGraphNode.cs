namespace LINQPadKit.Design
{
    public interface ITreeGraphNode
    {
        string Text { get; }
        ITreeGraphNode[] Children { get; }
    }
}
