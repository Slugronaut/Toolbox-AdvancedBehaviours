using Peg.Messaging;

namespace Peg.Behaviours
{
    /// <summary>
    /// Posts a simple message when this GameObject has a built-in Unity event triggered on it.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Post Message on Event")]
    public class PostMessageOnEvent : AbstractMessagePoster
    {
        public EventAndCollisionTiming Occurs;
        public float Delay = 0;

        
        protected override void Awake()
        {
            base.Awake();
            if (Occurs == EventAndCollisionTiming.Awake)
                Post(Delay);
        }

        protected override void Start()
        {
            base.Start();
            if (Occurs == EventAndCollisionTiming.Start)
                Post(Delay);
        }

        protected virtual void OnEnable()
        {
            if (Occurs == EventAndCollisionTiming.Enable)
                Post(Delay);
        }

        protected virtual void OnDisable()
        {
            if (Occurs == EventAndCollisionTiming.Disable)
                Post(Delay);
        }

        protected virtual void OnDestroy()
        {
            if (Occurs == EventAndCollisionTiming.Destroy)
                Post(Delay);
        }

        #if TOOLBOX_2DCOLLIDER
        protected void OnTriggerEnter2D(UnityEngine.Collider2D other)
        {
            if (Occurs == EventAndCollisionTiming.Triggered)
                Post(Delay);
        }
        #else
        protected void OnTriggerEnter(UnityEngine.Collider other)
        {
            if (Occurs == EventAndCollisionTiming.Triggered)
                Post(Delay);
        }
        #endif

        protected void OnTriggerExit(UnityEngine.Collider other)
        {
            if (Occurs == EventAndCollisionTiming.TriggerExit)
                Post(Delay);
        }

        void OnRelenquish()
        {
            if (Occurs == EventAndCollisionTiming.Relenquished)
                Post(Delay);
        }

        public void Post(float delay)
        {
            if (delay > 0)
                Invoke(nameof(PostMessage), delay);
            else PostMessage();
        }
    }


    


}
