using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Peg.Lib;

namespace Peg.Behaviours
{
    /// <summary>
    /// Sets the position, rotation and / or scale of a Transform when a Unity Event is triggered.
    /// </summary>
    public class SetTransformPropertiesOnEvent : AbstractOperationOnEvent
    {
        [Flags]
        public enum TransformOrientationFlags
        {
            Position    = 1 << 0,
            Rotation    = 1 << 1,
            Scale       = 1 << 2,
        }

        #region Members
        [SerializeField]
        [HideInInspector]
        byte Flags;

        [Space(10)]
        [Title("Transform Properties")]
        public Transform Trans;
        public Space Mode;

        [PropertyOrder(1)]
        [ShowInInspector]
        public bool SetPosition
        {
            get => (Flags & (byte)TransformOrientationFlags.Position) != 0;
            set
            {
                if (value) Flags |= (byte)TransformOrientationFlags.Position;
                else Flags &= (byte)TransformOrientationFlags.Position ^ 0xff;
            }
        }
        [PropertyOrder(2)]
        [ShowIf("SetPosition")]
        [Indent]
        public Vector3 Position;


        [PropertyOrder(3)]
        [ShowInInspector]
        public bool SetRotation
        {
            get => (Flags & (byte)TransformOrientationFlags.Rotation) != 0;
            set
            {
                if (value) Flags |= (byte)TransformOrientationFlags.Rotation;
                else Flags &= (byte)TransformOrientationFlags.Rotation ^ 0xff;
            }
        }

        [PropertyOrder(4)]
        [ShowIf("SetRotation")]
        [Indent]
        public Vector3 Rotation;


        [PropertyOrder(5)]
        [ShowInInspector]
        public bool SetScale
        {
            get => (Flags & (byte)TransformOrientationFlags.Scale) != 0;
            set
            {
                if (value) Flags |= (byte)TransformOrientationFlags.Scale;
                else Flags &= (byte)TransformOrientationFlags.Scale ^ 0xff;
            }
        }
        [PropertyOrder(6)]
        [ShowIf("SetScale")]
        [Indent]
        public Vector3 Scale;
        #endregion


        /// <summary>
        /// 
        /// </summary>
        public override void PerformOp()
        {
            if (Mode == Space.Self)
            {
                if (SetPosition)
                    Trans.localPosition = Position;
                if (SetRotation)
                    Trans.localRotation = Quaternion.Euler(Rotation);
                if (SetScale)
                    Trans.localScale = Scale;
            }
            else
            {
                if (SetPosition)
                    Trans.position= Position;
                if (SetRotation)
                    Trans.rotation = Quaternion.Euler(Rotation);
                if (SetScale)
                    Trans.SetGlobalScale(Scale);
            }
                    
                    
        }
    }
}
