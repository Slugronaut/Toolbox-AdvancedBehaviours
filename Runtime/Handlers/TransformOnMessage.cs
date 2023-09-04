using System;
using Peg.Messaging;
using UnityEngine;


namespace Peg.Behaviours
{
    /// <summary>
    /// Translates, rotates, and scales this entity upon receiving a message.
    /// </summary>
    public class TransformOnMessage : AbstractMessageReciever
    {
        public Vector3 Translate;
        public Vector3 Rotate;
        public Vector3 Scale = Vector3.one;
        public Space Space;
        public RotationModes RotMode;

        [SerializeField]
        [HideInInspector]
        TransformFlags _TransformFlags;

        public enum RotationModes
        {
            AboutAxis,
            Absolute,
        }

        [Flags]
        public enum TransformFlags
        {
            Position = 1,
            Rotation = 2,
            Scale = 4,
        }


        [Sirenix.OdinInspector.ShowInInspector]
        public bool TranslatePosition
        {
            get { return (_TransformFlags & TransformFlags.Position) != 0; }
            set
            {
                if (value) _TransformFlags |= TransformFlags.Position;
                else _TransformFlags &= ~TransformFlags.Position;
            }
        }

        [Sirenix.OdinInspector.ShowInInspector]
        public bool TranslateRotation
        {
            get { return (_TransformFlags & TransformFlags.Rotation) != 0; }
            set
            {
                if (value) _TransformFlags |= TransformFlags.Rotation;
                else _TransformFlags &= ~TransformFlags.Rotation;
            }
        }

        [Sirenix.OdinInspector.ShowInInspector]
        public bool TranslateScale
        {
            get { return (_TransformFlags & TransformFlags.Scale) != 0; }
            set
            {
                if (value) _TransformFlags |= TransformFlags.Scale;
                else _TransformFlags &= ~TransformFlags.Scale;
            }
        }



        protected override void HandleMessage(Type msgType, object msg)
        {
            var trans = transform;

            if (Space == Space.World)
            {
                trans.position += Translate;
                if (RotMode == RotationModes.AboutAxis) trans.Rotate(Rotate, Space.World);
                else trans.eulerAngles = Rotate;
                trans.localScale = Vector3.Scale(trans.lossyScale, Scale);
            }
            else
            {
                trans.localPosition += Translate;
                if (RotMode == RotationModes.AboutAxis) trans.Rotate(Rotate, Space.Self);
                else trans.localEulerAngles = Rotate;
                trans.localScale = Vector3.Scale(trans.lossyScale, Scale);
            }

        }
    }
}
