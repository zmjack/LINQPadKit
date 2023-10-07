using NStandard;

namespace LINQPadKit.Internal;

internal class StateHandler
{
    public State.ValueReceivedHandler<object> Changed;
    public State.ValueReceivedHandler<object> Updating;
}
