using System;
using Toolbox.Messaging;
using UnityEngine;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Playables;
#endif

namespace Toolbox.Behaviours
{
    /// <summary>
    /// Plays a Timeline playable upon receiving a message.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Play Timeline on Message")]
    public class PlayTimelineOnMessage : AbstractMessageReciever
    {
        [Tooltip("The playable director that is used.")]
        public PlayableDirector Director;

        [Tooltip("The playable asset to play. If null, the Director will play it's current playable.")]
        public PlayableAsset Asset;
        
        protected override void HandleMessage(Type msgType, object msg)
        {
            if (Asset == null)
                Director.Play();
            else Director.Play(Asset);
        }
    }
}

