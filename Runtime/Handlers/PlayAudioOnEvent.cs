using UnityEngine;

namespace Peg.Behaviours
{
    /// <summary>
    /// Simple audio source player that is triggered for a given event.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Play Audio on Event (simple)")]
    public class PlayAudioOnEvent : AbstractOperationOnEvent
    {
        [Tooltip("If set, this AudioSource will be used, otherwise the clip will play through the TempAudioSourcePlayer.")]
        public AudioSource Source;
        [Tooltip("An optional clip that can be specified to replace the clip stored in the AudioSource.")]
        public AudioClip Clip;
        
        public override void PerformOp()
        {
            if (Clip != null) Source.clip = Clip;
            if (Source != null)
                Source.Play();
            else if (Clip != null) TempAudioSourcePlayer.Instance.Play(Clip, transform.position);
        }
    }
}
