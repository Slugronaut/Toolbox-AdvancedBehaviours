using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Peg.Messaging;
using Peg.AutonomousEntities;

namespace Peg.Behaviours
{
    /// <summary>
    /// Sets various states of a rigidbody when a message is handled.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Set Rigidbody on Message")]
    public sealed class SetRigidbodyStateOnMessage : AbstractMessageReciever
    {
        [Tooltip("If not set, will use GetComponent() and FindComponentInEntity() in that order to locate a rigidbody.")]
        public Rigidbody BodyOverride;

        [Space(10)]
        [Tooltip("Should this handler change the kinematic state?")]
        public bool ChangeKinematicState;
        [Indent]
        [ShowIf("ChangeKinematicState")]
        public bool Kinematic;

        [Space(10)]
        [Tooltip("Should this handler change the gravity state?")]
        public bool ChangeGravityState;
        [Indent]
        [ShowIf("ChangeGravityState")]
        public bool Gravity;

        [Space(15)]
        public bool ChangeFreezeStates;
        [Header("Freeze Positions")]
        [Indent]
        [ShowIf("ChangeFreezeStates")]
        public bool xPos;
        [Indent]
        [ShowIf("ChangeFreezeStates")]
        public bool yPos;
        [Indent]
        [ShowIf("ChangeFreezeStates")]
        public bool zPos;
        [Header("Freeze Rotations")]
        [Indent]
        [ShowIf("ChangeFreezeStates")]
        public bool xRot;
        [Indent]
        [ShowIf("ChangeFreezeStates")]
        public bool yRot;
        [Indent]
        [ShowIf("ChangeFreezeStates")]
        public bool zRot;


        protected override void Awake()
        {
            if (BodyOverride == null)
            {
                BodyOverride = GetComponent<Rigidbody>();
                if (BodyOverride == null)
                    BodyOverride = gameObject.FindComponentInEntity<Rigidbody>();
            }

            base.Awake();
        }

        protected override void HandleMessage(Type msgType, object msg)
        {
            if(ChangeKinematicState) BodyOverride.isKinematic = Kinematic;
            if(ChangeGravityState) BodyOverride.useGravity = Gravity;

            if (ChangeFreezeStates)
            {
                RigidbodyConstraints flags = RigidbodyConstraints.None;
                if (xPos) flags |= RigidbodyConstraints.FreezePositionX;
                if (yPos) flags |= RigidbodyConstraints.FreezePositionY;
                if (zPos) flags |= RigidbodyConstraints.FreezePositionZ;
                if (xRot) flags |= RigidbodyConstraints.FreezeRotationX;
                if (yRot) flags |= RigidbodyConstraints.FreezeRotationY;
                if (zRot) flags |= RigidbodyConstraints.FreezeRotationZ;
                BodyOverride.constraints = flags;
            }

        }
    }
}
