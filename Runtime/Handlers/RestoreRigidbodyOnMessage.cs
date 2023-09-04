using System;
using Peg.Messaging;
using UnityEngine;


namespace Peg.Behaviours
{
    /// <summary>
    /// Upon receiving a message, adds a rigidbody and restores all of its previously stored values.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Restore Rigidbody on Message")]
    [RequireComponent(typeof(PreserveRigidbodyOnMessage))]
    public sealed class RestoreRigidbodyOnMessage : AbstractMessageReciever
    {
        PreserveRigidbodyOnMessage Preserve;

        protected override void Awake()
        {
            base.Awake();
            Preserve = GetComponent<PreserveRigidbodyOnMessage>();
        }

        protected override void HandleMessage(Type msgType, object msg)
        {
            if (Preserve != null) Preserve.Restore();
        }
    }
}
