using UnityEngine;


namespace Peg.Behaviours
{
    /// <summary>
    /// Plays a randomly selected sound from a list when a Unity event is triggered.
    /// The sound will be played at the position of this GameObject in worldspace.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Random Sound on Event")]
    public class RandomSoundOnEvent : AbstractOperationOnEvent
    {
        [Tooltip("Used to specify a source to play through. If none is supplied, the TempAudioSourcePlayer will be used.")]
        public AudioSource ManualSource;
        [Tooltip("If a previous sound was playing, should we interrupt it or overlap it with the new sound?")]
        public bool InterruptPrevious = false;
        [Tooltip("The volume at which to play the sound.")]
        public float Volume = 1.0f;
        [Tooltip("The index of the TempAudioSourcePlayer's audio source to use. Only needed if SourceOverride is null. Leave at 0 if not sure as that is always guaranteed to exist.")]
        public int TempAudioId = 0;
        [Tooltip("A list of audio clips to randomly choosen to play upon receiving the given message.")]
        public AudioClip[] Sounds;


        public override void PerformOp()
        {
            if (Sounds == null || Sounds.Length < 1) return;
            var clip = Sounds[Random.Range(0, Sounds.Length)];

            if (ManualSource != null)
            {
                if (InterruptPrevious) ManualSource.Stop();
                ManualSource.PlayOneShot(clip);
            }
            else
            {
                if (InterruptPrevious) TempAudioSourcePlayer.Instance.StopLastAndPlayNew(TempAudioId, GetInstanceID(), clip, transform.position);
                else TempAudioSourcePlayer.Instance.Play(TempAudioId, clip, transform.position, Volume);
            }
        }
    }
}
