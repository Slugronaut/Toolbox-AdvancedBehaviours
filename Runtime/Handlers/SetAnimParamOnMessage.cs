using System;
using System.Collections;
using Toolbox.Messaging;
using UnityEngine;


namespace Toolbox.Behaviours
{
    /// <summary>
    /// Sets a named animator parameter when a Unity event occurs on this GameObject.
    /// 
    /// TODO: Implement more param types.
    /// 
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Set AnimParam on Message")]
    public class SetAnimParamOnMessage : AbstractMessageReciever
    {
        [System.Serializable]
        public class BoolParam
        {
            public HashedString Name;
            public bool State;
        }

        [System.Serializable]
        public class FloatParam
        {
            public HashedString Name;
            public float State;
        }

        [System.Serializable]
        public class IntParam
        {
            public HashedString Name;
            public int State;
        }

        /// <summary>
        /// A special case of the float param. It can be used to tell
        /// an AnimatorEx that the param needs to scale the value by a
        /// given animation state clip length.
        /// </summary>
        [Serializable]
        public class AnimationTimeParam
        {
            public HashedString Name;
            public float Value;
        }


        public Animator Animator;

        public BoolParam[] Bools;
        public HashedString[] Triggers;
        public HashedString[] QueuedTriggers;

        protected override void HandleMessage(Type msgType, object msg)
        {
            for (int i = 0; i < Bools.Length; i++)
                Animator.SetBool(Bools[i].Name.Hash, Bools[i].State);

            for (int i = 0; i < Triggers.Length; i++)
                Animator.SetTriggerOneFrame(this, Triggers[i].Hash);

            for (int i = 0; i < QueuedTriggers.Length; i++)
                Animator.SetTrigger(QueuedTriggers[i].Hash);
        }
    }

    
    /// <summary>
    /// Extension methods for UnityEngine.Animator.
    /// </summary>
    public static class AnimatorExtension
    {

        public static void SetTriggerOneFrame(this Animator anim, MonoBehaviour coroutineRunner, string trigger)
        {
            coroutineRunner.StartCoroutine(TriggerOneFrame(anim, trigger));
        }

        public static void SetTriggerOneFrame(this Animator anim, MonoBehaviour coroutineRunner, int trigger)
        {
            coroutineRunner.StartCoroutine(TriggerOneFrame(anim, trigger));
        }

        private static IEnumerator TriggerOneFrame(Animator anim, string trigger)
        {
            anim.SetTrigger(trigger);
            yield return null;
            anim.ResetTrigger(trigger);
        }

        private static IEnumerator TriggerOneFrame(Animator anim, int trigger)
        {
            anim.SetTrigger(trigger);
            yield return null;
            anim.ResetTrigger(trigger);
        }
    }
}

