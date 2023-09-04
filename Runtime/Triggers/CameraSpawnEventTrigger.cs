using UnityEngine;
using Peg;

namespace Peg.Behaviours
{
    /// <summary>
    /// Attach this component to any camera to make it broadcast
    /// to the message dispatcher when it has come into existence.
    /// </summary>
    [AddComponentMenu("Toolbox/Common/Camera Spawn Event")]
    [RequireComponent(typeof(Camera))]
    [DisallowMultipleComponent]
    public sealed class CameraSpawnEventTrigger : MonoBehaviour
    {
        CameraSpawnedEvent Buffered;

        void Start()
        {
            Buffered = new CameraSpawnedEvent(GetComponent<Camera>());
            GlobalMessagePump.Instance.PostMessage(Buffered);
        }

        void OnDestroy()
        {
            GlobalMessagePump.Instance.RemoveBufferedMessage(Buffered);
            GlobalMessagePump.Instance.PostMessage(new CameraRemovedEvent(GetComponent<Camera>()));
        }
    }
}

namespace Peg
{
    public class CameraSpawnedEvent : TargetMessage<Camera, CameraSpawnedEvent>, IDeferredMessage, IBufferedMessage
    {
        public CameraSpawnedEvent(Camera cam) : base(cam) { }
    }

    public class CameraRemovedEvent : TargetMessage<Camera, CameraRemovedEvent>
    {
        public CameraRemovedEvent(Camera cam) : base(cam) { }
    }
}
