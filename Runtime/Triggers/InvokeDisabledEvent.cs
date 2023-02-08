using System;
using UnityEngine;
using UnityEngine.Events;



namespace Toolbox.Behaviours
{
    /// <summary>
    /// Invokes an event when disabled.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class InvokeDisabledEvent : MonoBehaviour
    {
        [Serializable]
        public class DisabledUnityEvent : UnityEvent<EntityRoot> { }

        public DisabledUnityEvent OnDisabled;


        void OnDisable()
        {
            OnDisabled.Invoke(gameObject.GetEntityRoot());
        }
    }
}
