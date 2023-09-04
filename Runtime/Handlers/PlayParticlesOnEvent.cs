using UnityEngine;

namespace Peg.Behaviours
{

    /// <summary>
    /// Plays a list of ParticleSystems upon triggering by a standar Unity GameObject event.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Play Particles on Event")]
    public class PlayParticlesOnEvent : AbstractOperationOnEvent
    {
        public ParticleSystem[] Particles;
        
        public override void PerformOp()
        {
            for (int i = 0; i < Particles.Length; i++)
                Particles[i].Play();
        }
        
    }

}
