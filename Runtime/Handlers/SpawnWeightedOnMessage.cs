using Peg.AutoCreate;
using Peg.Lazarus;
using Peg.Lib;
using Peg.Messaging;
using UnityEngine;


namespace Peg.Behaviours
{
    /// <summary>
    /// Spawns a recycle-pooled object upon receiving a local event.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Spawn from Pool on Message (weighted)")]
    public class SpawnWeightedOnMessage : AbstractMessageReciever
    {
        [Tooltip("How many instances to spawn.")]
        public int Count = 1;
        [Tooltip("The amount of jitter in world space.")]
        public Vector3 Jitter;
        public Vector3 Offset;
        [Tooltip("Should spwned objects be placed and oriented against a surface?")]
        public bool Decal;
        [Tooltip("If a decal, how far down to raycast when checking for placement on the ground.")]
        public float RaycastDist = 2;
        [Tooltip("Should a pool be used or should this be spawned raw?")]
        public bool Pooled = true;
        [Tooltip("Should objects be recyled from a pool rather than spawned anew?")]
        public bool Recycle;
        [Tooltip("One of these will randomly be spawned.")]
        public WeightedGameObject[] Prefabs;

        IPoolSystem Lazarus;
        static LayerMask Layers;
        const float DecalOffset = 0.05f;

        protected override void Awake()
        {
            Layers = LayerMask.GetMask("Default");
            //UPDATE: Not sure why the hell I forced CanDisable here. Turned it off due to corpse-spawning bug it caused
            //CanDisable = true; //force this to happen
            base.Awake();
            AutoCreator.AsSingleton<IPoolSystem>();
        }

        protected override void HandleMessage(System.Type msgType, object message)
        {
            if (message == null || Prefabs == null || Prefabs.Length < 1) return;
            Spawn(WeightedValueCollections.SelectWeightedRandom(Prefabs));
        }

        protected void Spawn(GameObject prefab)
        {
            if (prefab == null) return;
            var myPos = transform.position;

            if (Decal)
            {
                if (Pooled)
                {
                    if (Recycle)
                    {
                        for (int i = 0; i < Count; i++)
                            GameUtils.SpawnRecycledDecal(prefab, myPos + Jitter + Offset + Vector3.up, Vector3.down, DecalOffset, RaycastDist, Layers);
                    }
                    else
                    {
                        for (int i = 0; i < Count; i++)
                            GameUtils.SpawnPooledDecal(prefab, myPos + Jitter + Offset + Vector3.up, Vector3.down, DecalOffset, RaycastDist, Layers);
                    }
                }
                else
                {
                    for (int i = 0; i < Count; i++)
                        GameUtils.SpawnDecal(prefab, myPos + Jitter + Offset + Vector3.up, Vector3.down, DecalOffset, RaycastDist, Layers);
                }
            }
            else
            {
                if (Pooled)
                {
                    if (Recycle)
                    {
                        for (int i = 0; i < Count; i++)
                            Lazarus.RecycleSummon(prefab, myPos + Jitter + Offset);
                    }
                    else
                    {
                        for (int i = 0; i < Count; i++)
                            Lazarus.Summon(prefab, myPos + Jitter + Offset);
                    }
                }
                else
                {
                    for (int i = 0; i < Count; i++)
                        Instantiate(prefab, myPos + Jitter + Offset, Quaternion.identity);
                }
            }
        }
    }



}
