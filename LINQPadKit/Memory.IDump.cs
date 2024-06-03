using LINQPad;
using LINQPadKit.Design;

namespace LINQPadKit;

public partial class Memory : IDump<Memory>
{
    public Memory Dump()
    {
        GetGraphObject().Dump();
        return this;
    }

    public Memory Dump(string description)
    {
        GetGraphObject().Dump(description);
        return this;
    }

    public Memory Dump(bool toDataGrid, string description = null)
    {
        GetGraphObject().Dump(toDataGrid, description);
        return this;
    }

    public Memory Dump(int depth, int? collapseTo = null)
    {
        GetGraphObject().Dump(depth, collapseTo);
        return this;
    }

    public Memory Dump(string description = null, int? depth = null, int? collapseTo = null, bool toDataGrid = false, string exclude = null, string include = null, bool? alpha = null, bool noTotals = false, int? repeatHeadersAt = null, bool includePrivate = false)
    {
        GetGraphObject().Dump(description, depth, collapseTo, toDataGrid, exclude, include, alpha, noTotals, repeatHeadersAt, includePrivate);
        return this;
    }

    public Memory Dump(string description, int? depth, bool toDataGrid, string exclude = null, bool? alpha = null)
    {
        GetGraphObject().Dump(description, depth, toDataGrid, exclude, alpha);
        return this;
    }

    public Memory Dump(string description, bool toDataGrid)
    {
        GetGraphObject().Dump(description, toDataGrid);
        return this;
    }

    public Memory DumpTrace()
    {
        GetGraphObject().DumpTrace();
        return this;
    }

    public Memory DumpTrace(string description)
    {
        GetGraphObject().DumpTrace(description);
        return this;
    }

    public Memory DumpTrace(bool toDataGrid, string description = null)
    {
        GetGraphObject().DumpTrace(toDataGrid, description);
        return this;
    }

    public Memory DumpTrace(int depth, int? collapseTo = null)
    {
        GetGraphObject().DumpTrace(depth, collapseTo);
        return this;
    }

    public Memory DumpTrace(string description = null, int? depth = null, int? collapseTo = null, bool toDataGrid = false, string exclude = null, string include = null, bool? alpha = null, bool noTotals = false, int? repeatHeadersAt = null, bool includePrivate = false)
    {
        GetGraphObject().DumpTrace(description, depth, collapseTo, toDataGrid, exclude, include, alpha, noTotals, repeatHeadersAt, includePrivate);
        return this;
    }

    public Memory DumpTrace(string description, bool toDataGrid)
    {
        GetGraphObject().DumpTrace(description, toDataGrid);
        return this;
    }
}
