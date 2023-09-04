using Peg.AutoCreate;
using Peg.Lazarus;
using UnityEngine;

namespace Peg.Behaviours
{

    /// <summary>
    /// Allows setting enabled/active/destruction of
    /// components and GameObjects when a specified Unity
    /// event occurs.
    /// 
    /// TODO:   -Add support for Rigidbody preservation.
    ///         -CachedCount is being serialized by Fullinspector? Is it greedy for private vars?
    /// 
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Set Active State on Event")]
    public class SetActiveStateOnEvent : AbstractOperationOnEvent
    {
        [Tooltip("The operation to perform on the listed objects. Note that 'Relenquish' only operates on GameObjects.")]
        public StateOperation Op;
        
        #if UNITY_EDITOR
        [Tooltip("Optionally used to leaves notes for the purpose this component serves. Stripped from final build.")]
        [TextArea(3,5)]
        public string Comments;
        #endif

        public GameObject[] GameObjects;
        public Behaviour[] Behaviours;
        public Collider[] Colliders;
        public Collider2D[] Colliders2D;
        public Renderer[] Renderers;
        public Rigidbody[] Bodies;

        IPoolSystem Lazarus;

        protected override void Awake()
        {
            base.Awake();
            Lazarus = AutoCreator.AsSingleton<IPoolSystem>();
        }

        private void Reset()
        {
            GameObjects = new GameObject[0];
            Behaviours = new Behaviour[0];
            Colliders = new Collider[0];
            Colliders2D = new Collider2D[0];
            Renderers = new Renderer[0];
            Bodies = new Rigidbody[0];
        }

        public void ForceOp(int o)
        {
            StateOperation op = (StateOperation)o;
            switch (op)
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

        public override void PerformOp()
        { 
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
            //UPDATE: added a null check due to the need to add this component
            //at runtime (which won't ensure a default empty array)
            if (GameObjects != null)
            {
                for (int i = 0; i < GameObjects.Length; i++)
                    GameObjects[i].SetActive(state);
            }

            if (Behaviours != null)
            {
                for (int i = 0; i < Behaviours.Length; i++)
                    Behaviours[i].enabled = state;
            }

            if (Colliders != null)
            {
                for (int i = 0; i < Colliders.Length; i++)
                    Colliders[i].enabled = state;
            }

            if (Colliders2D != null)
            {
                for (int i = 0; i < Colliders2D.Length; i++)
                    Colliders2D[i].enabled = state;
            }

            if (Renderers != null)
            {
                for (int i = 0; i < Renderers.Length; i++)
                    Renderers[i].enabled = state;
            }

            //can only disable bodies, no enable supported
            if (state == false && Bodies != null)
            {
                for (int i = 0; i < Bodies.Length; i++)
                {
                    Bodies[i].isKinematic = true;
                    Bodies[i].useGravity = false;
                    Bodies[i].interpolation = RigidbodyInterpolation.None;
                }
            }
        }

        void DelayedDestroy()
        {
            for (int i = 0; i < Behaviours.Length; i++)
                Destroy(Behaviours[i]);

            for (int i = 0; i < Colliders.Length; i++)
                Destroy(Colliders[i]);

            for (int i = 0; i < Colliders2D.Length; i++)
                Destroy(Colliders2D[i]);

            for (int i = 0; i < Renderers.Length; i++)
                Destroy(Renderers[i]);

            for (int i = 0; i < Bodies.Length; i++)
                Destroy(Bodies[i]);

            for (int i = 0; i < GameObjects.Length; i++)
                Destroy(GameObjects[i]);

        }

        void DelayedRelenquish()
        {
            SetActive(false);

            for(int i = 0; i < GameObjects.Length; i++)
                 Lazarus.RelenquishToPool(GameObjects[i]);
        }

    }
    
}
