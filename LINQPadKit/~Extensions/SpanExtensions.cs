using LINQPad.Controls;
using LINQPadKit.Internal;
using NStandard;
using System.ComponentModel;

namespace LINQPadKit
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SpanExtensions
    {
        private static readonly Dictionary<StructTuple<Span, IState>, StateHandler> _handlerMap = new();

        public static Span Bind(this Span @this, IState state)
        {
            var key = StructTuple.Create(@this, state);

            if (!_handlerMap.ContainsKey(key))
            {
                var received = new State.ValueReceivedHandler<object>(value =>
                {
                    @this.Text = state.Value?.ToString();
                });

                state.Changed += received;
                state.Updating += received;

                _handlerMap.Add(key, new StateHandler
                {
                    Changed = received,
                    Updating = received,
                });
            }

            @this.Text = state.Value?.ToString();
            return @this;
        }

        public static Span Unbind(this Span @this, IState state)
        {
            var key = StructTuple.Create(@this, state);

            if (_handlerMap.TryGetValue(key, out var handler))
            {
                state.Changed -= handler.Changed;
                state.Updating += handler.Updating;
            }

            return @this;
        }
    }
}
