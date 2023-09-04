using System;
using Peg.Messaging;
using UnityEngine.Events;

namespace Peg.Behaviours
{
    /// <summary>
    /// Invokes a function when a message is received.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Invoke Function on Message")]
    public class InvokeFunctionOnMessage : AbstractMessageReciever
    {
        public UnityEvent OnTriggered;
        public MessageHandlerEvent OnHandleMessage;

        [Serializable]
        public class MessageHandlerEvent : UnityEvent<Type, object> {}


        protected override void HandleMessage(Type msgType, object msg)
        {
            OnTriggered.Invoke();
            OnHandleMessage.Invoke(msgType, msg);
        }
    }
}
