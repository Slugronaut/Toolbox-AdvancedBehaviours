using System;
using UnityEngine;


namespace Toolbox.Behaviours
{
    /// <summary>
    /// Translates, rotates, and scales this entity on the given unity event.
    /// </summary>
    public class TransformOnEvent : AbstractOperationOnEvent
    {
        public Vector3 Translate;
        public Vector3 Rotate;
        public Vector3 Scale = Vector3.one;
        public Space Space;

        [SerializeField]
        [HideInInspector]
        TransformFlags _TransformFlags;

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


        public override void PerformOp()
        {
            var trans = transform;

            if (Space == Space.World)
            {
                trans.position += Translate;
                trans.Rotate(Rotate, Space.World);
                trans.localScale = Vector3.Scale(trans.lossyScale, Scale);
            }
            else
            {
                trans.localPosition += Translate;
                trans.Rotate(Rotate, Space.Self);
                trans.localScale = Vector3.Scale(trans.lossyScale, Scale);
            }

        }
    }
}
