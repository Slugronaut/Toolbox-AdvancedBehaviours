using Peg.MessageDispatcher;
using UnityEngine;

namespace Peg.Behaviours
{
    /// <summary>
    /// 
    /// </summary>
    public class CameraSpawnedEvent : TargetMessage<Camera, CameraSpawnedEvent>, IDeferredMessage, IBufferedMessage
    {
        public CameraSpawnedEvent(Camera cam) : base(cam) { }
    }


    /// <summary>
    /// 
    /// </summary>
    public class CameraRemovedEvent : TargetMessage<Camera, CameraRemovedEvent>
    {
        public CameraRemovedEvent(Camera cam) : base(cam) { }
    }
}
