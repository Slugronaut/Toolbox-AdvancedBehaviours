using UnityEngine;

namespace Peg.Behaviours
{
    /// <summary>
    /// Sets a transform's orientation upon a Unity event.
    /// </summary>
    public class SetOrientationOnEvent : AbstractOperationOnEvent
    {
        public Transform Trans;
        public Vector3 Rotation;
        public Space RotationSpace;

        public override void PerformOp()
        {
            if (RotationSpace == Space.Self)
                Trans.localRotation = Quaternion.Euler(Rotation);
            else Trans.rotation = Quaternion.Euler(Rotation);
        }
    }
}
