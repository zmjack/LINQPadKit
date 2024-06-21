using LINQPad;
using LINQPadKit.Internal;
using NStandard;
using System.ComponentModel;

namespace LINQPadKit;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class DumpContainerExtensions
{
    private static readonly Dictionary<StructTuple<DumpContainer, IState>, StateHandler> _handlerMap = [];

    public static DumpContainer Bind(this DumpContainer @this, IState state)
    {
        var key = StructTuple.Create(@this, state);

        if (!_handlerMap.ContainsKey(key))
        {
            var received = new State.ValueReceivedHandler<object>(value =>
            {
                @this.Content = state.Value;
            });

            state.Changed += received;
            state.Updating += received;
            _handlerMap.Add(key, new StateHandler
            {
                Changed = received,
                Updating = received,
            });
        }

        @this.Content = state.Value;
        return @this;
    }

    public static DumpContainer Unbind(this DumpContainer @this, IState state)
    {
        var key = StructTuple.Create(@this, state);

        if (_handlerMap.TryGetValue(key, out var handler))
        {
            state.Changed -= handler.Changed;
            state.Updating -= handler.Updating;
        }

        return @this;
    }
}
