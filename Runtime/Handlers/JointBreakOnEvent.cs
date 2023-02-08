using UnityEngine;


namespace Toolbox.Behaviours
{
    /// <summary>
    /// Breaks a set of joints when the Unity event is triggered.
    /// </summary>
    public class JointBreakOnEvent : AbstractOperationOnEvent
    {
        [Tooltip("Should we set the force break limit?")]
        public bool SetForce;
        public float Force;
        [Tooltip("Should we set the torque break limit?")]
        public bool SetTorque;
        public float Torque;

        public Joint[] Joints;
        

        public override void PerformOp()
        {
            for (int i = 0; i < Joints.Length; i++)
            {
                if(SetForce) Joints[i].breakForce = Force;
                if(SetTorque) Joints[i].breakTorque = Torque;
            }
        }
    }
}
