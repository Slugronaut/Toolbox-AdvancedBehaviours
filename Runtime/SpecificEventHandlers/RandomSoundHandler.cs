using UnityEngine;


namespace Toolbox.Behaviours
{
    /// <summary>
    /// Plays a randomly selected sound from a list when its public method is called.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Handlers/Random Sound Handler")]
    public class RandomSoundHandler : MonoBehaviour, IUnityEventHandler
    {
        [Tooltip("Used to specify a source to play through. If none is supplied, the TempAudioSourcePlayer will be used.")]
        public AudioSource ManualSource;
        [Tooltip("The volume to play at.")]
        public float Volume = 1;
        [Tooltip("If a previous sound was playing, should we interrupt it or overlap it with the new sound?")]
        public bool InterruptPrevious = false;
        [Tooltip("The index of the TempAudioSourcePlayer's audio source to use. Only needed if SourceOverride is null. Leave at 0 if not sure as that is always guaranteed to exist.")]
        public int TempAudioId = 0;
        [Tooltip("Cooldown between played sounds.")]
        public float Cooldown;
        [Tooltip("A list of audio clips to randomly choosen to play upon receiving the given message.")]
        public AudioClip[] Sounds;
        

        float LastTime;


        public void Handle()
        {
            if (Sounds == null || Sounds.Length < 1) return;

            float t = Time.time;
            if (Cooldown > 0 && t - LastTime < Cooldown) return;
            LastTime = t;

            var clip = Sounds[Random.Range(0, Sounds.Length)];

            if (ManualSource != null)
            {
                if (InterruptPrevious) ManualSource.Stop();
                ManualSource.PlayOneShot(clip, Volume);
            }
            else
            {
                if (InterruptPrevious) TempAudioSourcePlayer.Instance.StopLastAndPlayNew(TempAudioId, GetInstanceID(), clip, transform.position, Volume);
                else TempAudioSourcePlayer.Instance.Play(TempAudioId, clip, transform.position, Volume);
            }
        }
    }
    
    
}
