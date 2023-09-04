using UnityEngine;
using static Peg.Behaviours.SetAnimParamOnMessage;

namespace Peg.Behaviours
{
    /// <summary>
    /// Sets a named animator parameter when a trigger on this GameObject is struck.
    /// 
    /// TODO: Implement more param types.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Set AnimParam on Event")]
    public class SetAnimParamOnEvent: AbstractOperationOnEvent
    {
        public Animator Animator;
        public BoolParam[] Bools;
        public HashedString[] Triggers;


        public override void PerformOp()
        {
            for (int i = 0; i < Bools.Length; i++)
                Animator.SetBool(Bools[i].Name.Hash, Bools[i].State);

            for (int i = 0; i < Triggers.Length; i++)
                Animator.SetTrigger(Triggers[i].Hash);
        }
    }
}
