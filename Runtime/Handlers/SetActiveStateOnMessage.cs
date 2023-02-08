using UnityEngine;
using System;
using Toolbox.Messaging;
using Toolbox.Lazarus;
using Toolbox.AutoCreate;

namespace Toolbox.Behaviours
{
    /// <summary>
    /// Used to activate or deactivate various GameObjects or components when a a message is received.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Set Active State on Message")]
    public sealed class SetActiveStateOnMessage : AbstractMessageReciever
    {
        [Tooltip("How long after the trigger count is reached before performing the set operation.")]
        public float Delay = 0;
        [Tooltip("How many times the event must occur before the operation is performed.")]
        public int TriggerCount = 1;
        [Tooltip("Does the counter reset after the operation is triggered? Once this object has been instantiated, the initial TriggerCount will always be used as the default reset value.")]
        public bool Resets = true;
        [Tooltip("The operation to perform on the listed objects. Note that 'Relenquish' only operates on GameObjects.")]
        public StateOperation Op;

        
        public GameObject[] GameObjects;
        public Behaviour[] Behaviours;
        public Renderer[] Renderers;
        public Collider[] Colliders3d;
        public Collider2D[] Colliders2d;

        int CachedCount;
        IPoolSystem Lazarus;


        protected override void Awake()
        {
            base.Awake();
            Lazarus = AutoCreator.AsSingleton<IPoolSystem>();
            CachedCount = TriggerCount;
        }

        protected override void HandleMessage(Type msgType, object message)
        {
            if (message == null) return;
            TryPerformOp();
        }
        
        void TryPerformOp()
        {
            //if (!isActiveAndEnabled) return;
            TriggerCount--;
            if (TriggerCount == 0)
            {
                if (Resets) TriggerCount = CachedCount;
                switch (Op)
                {
                    case StateOperation.Activate:
                        {
                            if (Delay > 0) Invoke(DelayedActivateProxyMethod, Delay);
                            else SetActive(true);
                            break;
                        }
                    case StateOperation.Destroy:
                        {
                            if (Delay > 0) Invoke(DelayedDestroyMethod, Delay);
                            else DelayedDestroy();
                            break;
                        }
                    case StateOperation.Deactivate:
                        {
                            if (Delay > 0) Invoke(DelayedDeactivateProxyMethod, Delay);
                            else SetActive(false);
                            break;
                        }
                    case StateOperation.Relenquish:
                        {
                            if (Delay > 0) Invoke(DelayedRelenquishMethod, Delay);
                            else DelayedRelenquish();
                            break;
                        }
                }
            }

        }

        const string DelayedActivateProxyMethod = "DelayedActivateProxy";
        const string DelayedDeactivateProxyMethod = "DelayedDeactivateProxy";
        const string DelayedDestroyMethod = "DelayedDestroy";
        const string DelayedRelenquishMethod = "DelayedRelenquish";

        void DelayedActivateProxy()
        {
            SetActive(true);
        }

        void DelayedDeactivateProxy()
        {
            SetActive(false);
        }

        void SetActive(bool state)
        {
            for (int i = 0; i < GameObjects.Length; i++)
                GameObjects[i].SetActive(state);

            for (int i = 0; i < Behaviours.Length; i++)
                Behaviours[i].enabled = state;

            for (int i = 0; i < Colliders3d.Length; i++)
                Colliders3d[i].enabled = state;

            for (int i = 0; i < Colliders2d.Length; i++)
                Colliders2d[i].enabled = state;

            for (int i = 0; i < Renderers.Length; i++)
                Renderers[i].enabled = state;
        }

        void DelayedDestroy()
        {
            for (int i = 0; i < Behaviours.Length; i++)
                Destroy(Behaviours[i]);

            for (int i = 0; i < Colliders3d.Length; i++)
                Destroy(Colliders3d[i]);

            for (int i = 0; i < Colliders2d.Length; i++)
                Destroy(Colliders2d[i]);

            for (int i = 0; i < Renderers.Length; i++)
                Destroy(Renderers[i]);

            for (int i = 0; i < GameObjects.Length; i++)
                Destroy(GameObjects[i]);
        }

        void DelayedRelenquish()
        {
            SetActive(false);

            for (int i = 0; i < GameObjects.Length; i++)
                Lazarus.RelenquishToPool(GameObjects[i]);
        }
    }

}
