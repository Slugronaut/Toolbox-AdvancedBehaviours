using Peg.Messaging;
using UnityEngine;


namespace Peg.Behaviours
{
    /// <summary>
    /// Plays a sound randomly from a list upn receiving a message.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Random Sound on Message (Ex)")]
    public sealed class RandomSoundOnMessageEx : AbstractMessageReciever
    {
        [Tooltip("Used to specify a source to play through. If none is supplied, the TempAudioSourcePlayer will be used.")]
        public AudioSource SourceOverride;
        [Tooltip("If a previous sound was playing, should we interrupt it or overlap it with the new sound?")]
        public bool InterruptPrevious = false;
        [Tooltip("The index of the TempAudioSourcePlayer's audio source to use. Only needed if SourceOverride is null. Leave at 0 if not sure as that is always guaranteed to exist.")]
        public int TempAudioId = 0;
        [Tooltip("How long to delay playing the sound.")]
        public float Delay = 0;
        [Tooltip("The volume to play the clip at.")]
        public float Volume = 1;
        [Tooltip("A cooldown time between playing sounds.")]
        public float Cooldown = 0;
        [Tooltip("A list of audio clips to randomly choosen to play upon receiving the given message.")]
        public AudioClip[] Sounds;

        float LastTime = float.MinValue;
        public static string PlaySoundFunc = "PlaySound";


        protected override void HandleMessage(System.Type msgType, object msg)
        {
            if (Delay <= 0) PlaySound();
            else Invoke(PlaySoundFunc, Delay);
        }

        void PlaySound()
        {
            float t = Time.time;
            if (t - LastTime < Cooldown)
                return;

            LastTime = t;
            if (Sounds == null || Sounds.Length < 1) return;
            var clip = Sounds[Random.Range(0, Sounds.Length)];

            if (SourceOverride != null)
                SourceOverride.PlayOneShot(clip);
            else
            {
                if (InterruptPrevious) TempAudioSourcePlayer.Instance.StopLastAndPlayNew(TempAudioId, GetInstanceID(), clip, transform.position, Volume);
                else TempAudioSourcePlayer.Instance.Play(TempAudioId, clip, transform.position, Volume);
            }
        }
    }
}
