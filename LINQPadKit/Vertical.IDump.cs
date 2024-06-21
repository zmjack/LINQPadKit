﻿using LINQPad;
using LINQPadKit.Design;

namespace LINQPadKit;

public partial class Vertical : IDump<Vertical>
{
    public Vertical Dump()
    {
        GetGraphObject().Dump();
        return this;
    }

    public Vertical Dump(string description)
    {
        GetGraphObject().Dump(description);
        return this;
    }

    public Vertical Dump(bool toDataGrid, string description = null)
    {
        GetGraphObject().Dump(toDataGrid, description);
        return this;
    }

    public Vertical Dump(int depth, int? collapseTo = null)
    {
        GetGraphObject().Dump(depth, collapseTo);
        return this;
    }

    public Vertical Dump(string description = null, int? depth = null, int? collapseTo = null, bool toDataGrid = false, string exclude = null, string include = null, bool? alpha = null, bool noTotals = false, int? repeatHeadersAt = null, bool includePrivate = false)
    {
        GetGraphObject().Dump(description, depth, collapseTo, toDataGrid, exclude, include, alpha, noTotals, repeatHeadersAt, includePrivate);
        return this;
    }

    public Vertical Dump(string description, int? depth, bool toDataGrid, string exclude = null, bool? alpha = null)
    {
        GetGraphObject().Dump(description, depth, toDataGrid, exclude, alpha);
        return this;
    }

    public Vertical Dump(string description, bool toDataGrid)
    {
        GetGraphObject().Dump(description, toDataGrid);
        return this;
    }

    public Vertical DumpTrace()
    {
        GetGraphObject().DumpTrace();
        return this;
    }

    public Vertical DumpTrace(string description)
    {
        GetGraphObject().DumpTrace(description);
        return this;
    }

    public Vertical DumpTrace(bool toDataGrid, string description = null)
    {
        GetGraphObject().DumpTrace(toDataGrid, description);
        return this;
    }

    public Vertical DumpTrace(int depth, int? collapseTo = null)
    {
        GetGraphObject().DumpTrace(depth, collapseTo);
        return this;
    }

    public Vertical DumpTrace(string description = null, int? depth = null, int? collapseTo = null, bool toDataGrid = false, string exclude = null, string include = null, bool? alpha = null, bool noTotals = false, int? repeatHeadersAt = null, bool includePrivate = false)
    {
        GetGraphObject().DumpTrace(description, depth, collapseTo, toDataGrid, exclude, include, alpha, noTotals, repeatHeadersAt, includePrivate);
        return this;
    }

    public Vertical DumpTrace(string description, bool toDataGrid)
    {
        GetGraphObject().DumpTrace(description, toDataGrid);
        return this;
    }
}
