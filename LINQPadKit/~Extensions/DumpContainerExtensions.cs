using LINQPad;
using NStandard;
using System.ComponentModel;

namespace LINQPadKit
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DumpContainerExtensions
    {
        private static readonly Dictionary<StructTuple<DumpContainer, IState>, State.UpdatingHandler> _handlerMap = new();

        public static DumpContainer Bind(this DumpContainer @this, IState state)
        {
            var key = StructTuple.Create(@this, state);

            if (!_handlerMap.ContainsKey(key))
            {
                var updating = new State.UpdatingHandler(value =>
                {
                    @this.Refresh();
                });

                state.Updating += updating;

                _handlerMap.Add(key, updating);
            }

            @this.Content = state.Value;
            return @this;
        }

        public static DumpContainer Unbind(this DumpContainer @this, IState state)
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
