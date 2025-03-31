using LINQPad.Controls;
using LINQPadKit.Internal;
using NStandard.Static;
using System.ComponentModel;

namespace LINQPadKit;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class TextAreaExtensions
{
    private static readonly Dictionary<StructTuple<TextArea, IState>, StructTuple<EventHandler, StateHandler>> _handlerMap = [];

    public static TextArea Bind(this TextArea @this, IState state)
    {
        var key = StructTuple.Create(@this, state);

        if (!_handlerMap.ContainsKey(key))
        {
            var received = new State.ValueReceivedHandler<object>(value =>
            {
                @this.Text = state.Value?.ToString();
            });

            var textInput = new EventHandler((sender, e) =>
            {
                try
                {
                    state.Value = ConvertEx.ChangeType(@this.Text, state.ValueType);
                }
                catch { }
            });

            state.Changed += received;
            state.Updating += received;
            @this.TextInput += textInput;

            _handlerMap.Add(key, StructTuple.Create(textInput, new StateHandler
            {
                Changed = received,
                Updating = received,
            }));
        }

        @this.Text = state.Value?.ToString();
        return @this;
    }

    public static TextArea Unbind(this TextArea @this, IState state)
    {
        var key = StructTuple.Create(@this, state);

        if (_handlerMap.ContainsKey(key))
        {
            var (textInput, handler) = _handlerMap[key];
            @this.TextInput -= textInput;
            state.Changed -= handler.Changed;
            state.Updating -= handler.Updating;
        }

        return @this;
    }

}
