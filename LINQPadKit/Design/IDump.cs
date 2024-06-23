namespace LINQPadKit.Design;

public interface IDump<T>
{
    T Dump();
    T Dump(string? description);
    T Dump(bool toDataGrid, string? description = null);
    T Dump(int depth, int? collapseTo = null);
    T Dump(string? description = null, int? depth = null, int? collapseTo = null, bool toDataGrid = false, string? exclude = null, string? include = null, bool? alpha = null, bool noTotals = false, int? repeatHeadersAt = null, bool includePrivate = false);
    T Dump(string description, int? depth, bool toDataGrid, string? exclude = null, bool? alpha = null);
    T Dump(string description, bool toDataGrid);

    T DumpTrace();
    T DumpTrace(string? description);
    T DumpTrace(bool toDataGrid, string? description = null);
    T DumpTrace(int depth, int? collapseTo = null);
    T DumpTrace(string? description = null, int? depth = null, int? collapseTo = null, bool toDataGrid = false, string? exclude = null, string? include = null, bool? alpha = null, bool noTotals = false, int? repeatHeadersAt = null, bool includePrivate = false);
    T DumpTrace(string? description, bool toDataGrid);
}
