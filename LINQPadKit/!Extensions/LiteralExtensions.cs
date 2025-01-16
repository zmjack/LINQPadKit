using LINQPad.Controls;
using LINQPadKit.Internal;
using System.Text.Encodings.Web;

namespace LINQPadKit;

public static class LiteralExtensions
{
    private static readonly Dictionary<StructTuple<Literal, IState>, StateHandler> _handlerMap = [];

    public static Literal Bind(this Literal @this, IState state)
    {
        var key = StructTuple.Create(@this, state);

        if (!_handlerMap.ContainsKey(key))
        {
            var received = new State.ValueReceivedHandler<object>(value =>
            {
                @this.HtmlElement.InnerHtml = HtmlEncoder.Default.Encode(state.Value?.ToString() ?? "");
            });

            state.Changed += received;
            state.Updating += received;

            _handlerMap.Add(key, new StateHandler
            {
                Changed = received,
                Updating = received,
            });
        }

        @this.HtmlElement.InnerHtml = HtmlEncoder.Default.Encode(state.Value?.ToString() ?? "");
        return @this;
    }

    public static Literal Unbind(this Literal @this, IState state)
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
