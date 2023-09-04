using UnityEngine;

namespace Peg.Behaviours
{
    /// <summary>
    /// Sets an object's parent to a local player avatar when one spawns.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Parent to Avatar Spawn")]
    public class ParentOnPlayerSpawn : MonoBehaviour
    {
        public Vector3 Offset;

        [Tooltip("These components will be transfered to the parenting object upon spawn and then tranfered back to thier source when despawned.")]
        public Transform[] Transfers;
        Transform[] Sources;

        void Start()
        {
            GlobalMessagePump.Instance.AddListener<LocalPlayerAvatarSpawnedEvent>(HandleSpawn);
            GlobalMessagePump.Instance.AddListener<LocalPlayerAvatarRemovedEvent>(HandleRemoved);
        }

        void OnDestroy()
        {
            GlobalMessagePump.Instance.RemoveListener<LocalPlayerAvatarSpawnedEvent>(HandleSpawn);
            GlobalMessagePump.Instance.RemoveListener<LocalPlayerAvatarRemovedEvent>(HandleRemoved);
        }

        void HandleSpawn(LocalPlayerAvatarSpawnedEvent msg)
        {
            if (msg.Target == null) return;
            transform.SetParent(msg.Target.transform, false);
            transform.localPosition = Offset;

            if (Transfers != null && Transfers.Length > 0)
            {
                Sources = new Transform[Transfers.Length];
                for (int i = 0; i < Transfers.Length; i++)
                {
                    Sources[i] = Transfers[i].parent;
                    Transfers[i].SetParent(msg.Target.transform, false);
                }
            }
        }

        void HandleRemoved(LocalPlayerAvatarRemovedEvent msg)
        {
            if (!TypeHelper.IsReferenceNull(gameObject))
            {
                transform.SetParent(null, false);

                if (Transfers != null && Transfers.Length > 0)
                {
                    for (int i = 0; i < Transfers.Length; i++)
                        Transfers[i].SetParent(Sources[i], false);
                }

                Sources = null;
            }


        }
    }
}
