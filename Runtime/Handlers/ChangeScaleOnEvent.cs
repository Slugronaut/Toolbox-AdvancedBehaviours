using UnityEngine;


namespace Toolbox.Behaviours
{
    public class ChangeScaleOnEvent : AbstractOperationOnEvent
    {
        public Transform Trans;
        public Vector3 Scale;


        public override void PerformOp()
        {
            Trans.localScale = Scale;
        }

        public void ManuallySetScale(Vector3 scale)
        {
            Scale = scale;
            PerformOp();
        }
    }
}
