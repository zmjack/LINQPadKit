using LINQPad.Controls;
using NStandard;
using System.ComponentModel;

namespace LINQPadKit
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TextBoxExtensions
    {
        private static readonly Dictionary<StructTuple<TextBox, IState>, StructTuple<EventHandler, StructTuple<State.ChangedHandler, State.UpdatingHandler>>> _handlerMap = new();

        public static TextBox Bind(this TextBox @this, IState state)
        {
            var key = StructTuple.Create(@this, state);

            if (!_handlerMap.ContainsKey(key))
            {
                var textInput = new EventHandler((sender, e) =>
                {
                    try
                    {
                        state.Value = ConvertEx.ChangeType(@this.Text, state.ValueType);
                    }
                    catch { }
                });
                var changed = new State.ChangedHandler(value =>
                {
                    @this.Text = state.Value?.ToString();
                });
                var updating = new State.UpdatingHandler(value =>
                {
                    @this.Text = state.Value?.ToString();
                });

                @this.TextInput += textInput;
                state.Changed += changed;
                state.Updating += updating;

                _handlerMap.Add(key, StructTuple.Create(textInput, StructTuple.Create(changed, updating)));
            }

            @this.Text = state.Value?.ToString();
            return @this;
        }

        public static TextBox Unbind(this TextBox @this, IState state)
        {
            var key = StructTuple.Create(@this, state);

            if (_handlerMap.ContainsKey(key))
            {
                var (textInput, (changed, updating)) = _handlerMap[key];
                @this.TextInput -= textInput;
                state.Changed -= changed;
                state.Updating += updating;
            }

            return @this;
        }

    }
}
