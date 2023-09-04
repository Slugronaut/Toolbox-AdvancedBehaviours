using UnityEngine;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Playables;
#endif

namespace Peg.Behaviours
{
    /// <summary>
    /// Plays a Timeline playable upon receiving a message.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Play Timeline on Event")]
#if !UNITY_2017_1_OR_NEWER
    [System.Obsolete("Missing functionality in this version of Unity. Please upgrade to Unity 2017.1 or newer.")]
#endif
    public class PlayTimelineOnEvent : AbstractOperationOnEvent
    {
#if UNITY_2017_1_OR_NEWER
        [Tooltip("The playable director that is used.")]
        public PlayableDirector Director;

        [Tooltip("The playable asset to play. If null, the Director will play it's current playable.")]
        public PlayableAsset Asset;
#endif

        public override void PerformOp()
        {
#if UNITY_2017_1_OR_NEWER
            if (Asset == null)
                Director.Play();
            else Director.Play(Asset);
#else
            Debug.LogWarning("PlayTimelineOnEvent ("+name+") is missing functionality in this version of Unity. Please upgrade to Unity 2017.1 or newer.");
#endif
        }
    }
}
