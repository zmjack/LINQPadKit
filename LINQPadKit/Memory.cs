using LINQPad;
using LINQPad.Controls;
using LINQPadKit.Design;
using LINQPadKit.Internal;
using NStandard;
using System.Collections;

namespace LINQPadKit;

public partial class Memory : IEnumerable<object>, IDumpObject
{
    public bool Decoded { get; }
    public int ChunkSize { get; }

    private readonly List<object> _children = [];
    public object GetGraphObject() => Util.VerticalRun(_children);

#if NET6_0_OR_GREATER
    public Memory(bool decoded) : this(decoded, 32) { }
    public Memory(bool decoded, int chunk)
    {
        if (chunk < 1) throw new ArgumentOutOfRangeException(nameof(chunk));

        Decoded = decoded;
        ChunkSize = chunk;
    }
#else
    public Memory(bool decoded)
    {
        Decoded = decoded;
        ChunkSize = 0;
    }
#endif

    private void InnerAdd<T>(T obj) where T : unmanaged
    {
        var value = new Span($"{obj}");
        var type = new Span($"({typeof(T)})");
        type.Styles["font-weight"] = "bold";

        _children.Add(Util.HorizontalRun(true, [type, value]));
        _children.Add(new Arithmetic<T>(obj, Decoded, ChunkSize));
    }

    public void Add(short value) => InnerAdd(value);
    public void Add(int value) => InnerAdd(value);
    public void Add(long value) => InnerAdd(value);
#if NET7_0_OR_GREATER
    public void Add(Int128 value) => InnerAdd(value);
#endif

    public void Add(ushort value) => InnerAdd(value);
    public void Add(uint value) => InnerAdd(value);
    public void Add(ulong value) => InnerAdd(value);
#if NET7_0_OR_GREATER
    public void Add(UInt128 value) => InnerAdd(value);
#endif

    public void Add(float value) => InnerAdd(value);
    public void Add(double value) => InnerAdd(value);
    public void Add(decimal value) => InnerAdd(value);

    public void Add<T>(T value) where T : unmanaged => InnerAdd(value);

    public IEnumerator GetEnumerator()
    {
        return _children.GetEnumerator();
    }

    IEnumerator<object> IEnumerable<object>.GetEnumerator()
    {
        return _children.AsEnumerable().GetEnumerator();
    }
}
