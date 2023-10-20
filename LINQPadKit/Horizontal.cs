using LINQPad;
using LINQPadKit.Design;
using System.Collections;

namespace LINQPadKit;

public partial class Horizontal : IEnumerable<object>, IDumpObject
{
    private readonly List<object> _children = new();
    private readonly bool _withGaps;

    public Horizontal(bool withGaps = true)
    {
        _withGaps = withGaps;
    }

    public object GetGraphObject() => Util.HorizontalRun(_withGaps, _children);

    public void Add(object obj)
    {
        _children.Add(obj switch
        {
            Horizontal horizontal => horizontal.GetGraphObject(),
            Vertical vertical => vertical.GetGraphObject(),
            _ => obj,
        });
    }
    public IEnumerator GetEnumerator()
    {
        return _children.GetEnumerator();
    }

    IEnumerator<object> IEnumerable<object>.GetEnumerator()
    {
        return _children.AsEnumerable().GetEnumerator();
    }
}
