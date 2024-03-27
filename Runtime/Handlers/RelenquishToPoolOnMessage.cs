using Peg.Messaging;
using System;

namespace Peg.Behaviours
{
    /// <summary>
    /// Returns this gameobject to the pool from which it came upon receiving a message.
    /// </summary>
    public class RelenquishToPoolOnMessage : AbstractMessageReciever
    {
        protected override void HandleMessage(Type msgType, object msg)
        {
            Lazarus.Lazarus.Instance.RelenquishToPool(gameObject);
        }
    }
}
