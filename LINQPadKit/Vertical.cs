using LINQPad;
using LINQPadKit.Design;
using System.Collections;

namespace LINQPadKit;

public partial class Vertical : IEnumerable<object>, IDumpObject
{
    private readonly List<object> _children = [];
    public object GetGraphObject() => Util.VerticalRun(_children);

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
