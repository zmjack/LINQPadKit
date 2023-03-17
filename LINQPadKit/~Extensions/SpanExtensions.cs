using LINQPad.Controls;
using NStandard;
using System.ComponentModel;

namespace LINQPadKit
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SpanExtensions
    {
        private static readonly Dictionary<StructTuple<Span, IState>, State.UpdatingHandler> _handlerMap = new();

        public static Span Bind(this Span @this, IState state)
        {
            var key = StructTuple.Create(@this, state);

            if (!_handlerMap.ContainsKey(key))
            {
                var updating = new State.UpdatingHandler(value =>
                {
                    @this.Text = state.Value?.ToString();
                });

                state.Updating += updating;

                _handlerMap.Add(key, updating);
            }

            @this.Text = state.Value?.ToString();
            return @this;
        }

        public static Span Unbind(this Span @this, IState state)
        {
            var key = StructTuple.Create(@this, state);

            if (_handlerMap.ContainsKey(key))
            {
                var updating = _handlerMap[key];
                state.Updating -= updating;
            }

            return @this;
        }
    }
}
