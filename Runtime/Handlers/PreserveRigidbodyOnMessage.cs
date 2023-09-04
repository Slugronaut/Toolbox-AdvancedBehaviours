using System;
using Peg.Messaging;
using UnityEngine;


namespace Peg.Behaviours
{
    /// <summary>
    /// Upon receiving a message, stores all values of a rigibody and then removes it.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Preserve Rigidbody on Message")]
    public sealed class PreserveRigidbodyOnMessage : AbstractMessageReciever
    {
        public Rigidbody Body;
        public bool Stored { get; private set;}

        GameObject BodyGO;

        float Drag;
        float AngularDrag;
        bool UseGravity;
        bool IsKinematic;
        RigidbodyInterpolation Interpolation;
        CollisionDetectionMode CollisionDetectionMode;
        bool DetectCollisions;
        RigidbodyConstraints Constraints;


        protected override void HandleMessage(Type msgType, object msg)
        {
            Remove();
        }

        public void Remove()
        {
            if (Body == null) return;
            BodyGO = Body.gameObject;

            Drag = Body.drag;
            AngularDrag = Body.angularDrag;
            UseGravity = Body.useGravity;
            IsKinematic = Body.isKinematic;
            Interpolation = Body.interpolation;
            CollisionDetectionMode = Body.collisionDetectionMode;
            DetectCollisions = Body.detectCollisions;
            Constraints = Body.constraints;

            Destroy(Body);
            Stored = true;
        }

        public void Restore()
        {
            if (!Stored) return;
            Body = BodyGO.AddComponent<Rigidbody>();

            Body.drag = Drag;
            Body.angularDrag = AngularDrag;
            Body.useGravity = UseGravity;
            Body.isKinematic = IsKinematic;
            Body.interpolation = Interpolation;
            Body.collisionDetectionMode = CollisionDetectionMode;
            Body.detectCollisions = DetectCollisions;
            Body.constraints = Constraints;

            Stored = false;
        }
    }
}
