using Sirenix.OdinInspector;
using UnityEngine;
using System;
using Peg.Lib;

namespace Peg.Behaviours
{
    /// <summary>
    /// Ultimate sound playing utility.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Play Sound Handler")]
    public sealed class PlaySoundHandler : MonoBehaviour
    {
        [Space(10)]
        [Title("Play Sound")]
        [Tooltip("Used to specify a source to play through. If none is supplied, the TempAudioSourcePlayer will be used.")]
        public AudioSource SourceOverride;
        [Tooltip("The index of the TempAudioSourcePlayer's audio source to use. Only needed if SourceOverride is null. Leave at 0 if not sure as that is always guaranteed to exist.")]
        [ShowIf("IsSourceOverrideNull")]
        [Indent]
        public byte TempAudioId = 0;
        [Tooltip("If a previous sound was playing, should we interrupt it or overlap it with the new sound?")]
        public InterruptModes InterruptMode = InterruptModes.Interrupt;
        [Tooltip("The policy for choosing which sound to play.")]
        public WeightedSelectionModes SelectionMode;
        [Tooltip("The index of the audip clip from the list that will be played.")]
        [ShowIf("IsUsingSelectionIndex")]
        [Indent]
        public byte SelectionIndex;
        [Tooltip("How long to delay playing the sound.")]
        public float Delay = 0;
        [Tooltip("The volume to play the clip at.")]
        public float Volume = 1;
        [Tooltip("A cooldown between playing sounds.")]
        public float Cooldown = 0;
        [Tooltip("A list of audio clips that will be selected to play based on the selection mode specified.")]
        public WeightedAudioClip[] Sounds;


        double LastTime = 0;
        bool IsSourceOverrideNull => SourceOverride == null;
        bool IsUsingSelectionIndex => SelectionMode == WeightedSelectionModes.Specific;

        public void PlaySound()
        {
            if (Delay <= 0) PlaySoundInternal();
            else Invoke(nameof(PlaySoundInternal), Delay);
        }

        void PlaySoundInternal()
        {
            if (Sounds == null || Sounds.Length < 1) return;
            if (Time.timeAsDouble - LastTime < Cooldown) return;
            LastTime = Time.timeAsDouble;

            if(SelectionMode == WeightedSelectionModes.All)
            {
                for (int i = 0; i < Sounds.Length; i++)
                    Play(Sounds[i].Value);
            }
            else if(SelectionMode == WeightedSelectionModes.Random)
                Play(Sounds.SelectWeightedRandom());
            else if(SelectionMode == WeightedSelectionModes.Specific)
                Play(Sounds[SelectionIndex].Value);
            
        }

        /// <summary>
        /// Helper method for playing the clip based on the interrupt policy.
        /// </summary>
        /// <param name="clip"></param>
        private void Play(AudioClip clip)
        {
            if (SourceOverride != null)
                SourceOverride.PlayOneShot(clip);
            else
             TempAudioSourcePlayer.Instance.PlayWithInterruptMode(InterruptMode, TempAudioId, GetInstanceID(), clip, transform.position, Volume);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class WeightedAudioClip : WeightedValue<AudioClip>
    {
    }

    public enum WeightedSelectionModes : byte
    {
        Random,
        All,
        Specific,
    }

}
