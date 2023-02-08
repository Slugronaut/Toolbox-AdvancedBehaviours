using UnityEngine;

namespace Toolbox.Behaviours
{
    /// <summary>
    /// Attach to a GameObject that you want to teleport to
    /// the target of a <see cref="LocalPlayerAvatarSpawnedEvent"/>> event.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Move to Avatar on Spawn")]
    public class MoveToPlayerOnSpawn : AttachToPlayerOnSpawn
    {
        [Tooltip("Offset from the player object once moved.")]
        [Compact]
        public Vector3 Offset;

        protected override void OnLocalPlayerAvatarSpawned(LocalPlayerAvatarSpawnedEvent msg)
        {
            transform.position = msg.Target.transform.position + Offset;
        }

        protected override void OnLocalPlayerAvatarRemoved(LocalPlayerAvatarRemovedEvent msg)
        {
            //does nothing but must be overriden due to being abstract
        }

    }
}
