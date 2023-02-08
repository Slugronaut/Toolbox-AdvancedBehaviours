
namespace Toolbox.Behaviours
{
    /// <summary>
    /// Invokes functions attached to the supplied UnityEvents of this component.
    /// </summary>
    public class InvokeFunctionOnEvent : AbstractOperationOnEvent
    {
        public UnityEngine.Events.UnityEvent OnTriggered;
        public override void PerformOp()
        {
            OnTriggered.Invoke();
        }
    }
}
