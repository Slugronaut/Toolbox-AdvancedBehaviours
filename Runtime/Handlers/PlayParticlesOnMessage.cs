using System;
using Peg.Messaging;
using UnityEngine;

namespace Peg.Behaviours
{
    /// <summary>
    /// Plays a list of ParticleSystems upon receving a message.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Play Particles on Message")]
    public class PlayParticlesOnMessage : AbstractMessageReciever
    {
        public ParticleSystem[] Particles;
        
        protected override void HandleMessage(Type msgType, object msg)
        {
            for (int i = 0; i < Particles.Length; i++)
                Particles[i].Play();
        }
    }
}
