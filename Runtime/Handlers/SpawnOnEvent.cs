using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Assertions;
using Peg.Lazarus;
using Peg.AutoCreate;
using Peg.Lib;

namespace Peg.Behaviours
{
    /// <summary>
    /// Spawns a recycle-pooled object upon receiving a local event.
    /// </summary>
    [AddComponentMenu("Toolbox/Action Triggers/Spawn on Event")]
    public class SpawnOnEvent : AbstractOperationOnEvent
    {
        public enum SpawnMode
        {
            Random,
            All,
            //Sequential,
        }
        [Tooltip("How many instances to spawn.")]
        public int Count = 1;
        [Tooltip("The amount of jitter in world space.")]
        public Vector3 Jitter;
        [Tooltip("Should spwned objects be placed and oriented against a surface?")]
        public bool Decal;
        [Sirenix.OdinInspector.ShowIf("Decal")]
        [Tooltip("If a decal, how far down to raycast when checking for placement on the ground.")]
        public float RaycastDist = 2;
        [Tooltip("Should a pool be used or should this be spawned raw?")]
        public bool Pooled = true;
        [Sirenix.OdinInspector.ShowIf("Pooled")]
        [Tooltip("Should objects be recyled from a pool rather than spawned anew?")]
        public bool Recycle;
        [Tooltip("How will the collection of prefabs be spawned? Randomly chosen? All? Sequentially chosen?")]
        public SpawnMode Mode;
        [Tooltip("One of these will randomly be spawned.")]
        public GameObject[] Prefabs;

        IPoolSystem Lazarus;

        static LayerMask Layers;
        const float Offset = 0.05f;


        protected override void Awake()
        {
            Lazarus = AutoCreator.AsSingleton<IPoolSystem>();
            Layers = LayerMask.GetMask("Default");
            base.Awake();
        }


        public override void PerformOp()
        {
            if (Prefabs == null || Prefabs.Length < 1) return;
            switch(Mode)
            {
                case SpawnMode.Random:
                    {
                        Spawn(Prefabs[Random.Range(0, Prefabs.Length)]);
                        break;
                    }
                case SpawnMode.All:
                    {
                        for (int i = 0; i < Prefabs.Length; i++)
                            Spawn(Prefabs[i]);
                        break;
                    }
            }
            
        }

        protected void Spawn(GameObject prefab)
        {
            Assert.IsNotNull(prefab);
            var myPos = transform.position;

            if (Decal)
            {
                if (Pooled)
                {
                    if (Recycle)
                    {
                        for (int i = 0; i < Count; i++)
                            GameUtils.SpawnRecycledDecal(prefab, myPos + Jitter + Vector3.up, Vector3.down, Offset, RaycastDist, Layers);
                    }
                    else
                    {
                        for (int i = 0; i < Count; i++)
                            GameUtils.SpawnPooledDecal(prefab, myPos + Jitter + Vector3.up, Vector3.down, Offset, RaycastDist, Layers);
                    }
                }
                else
                {
                    for (int i = 0; i < Count; i++)
                        GameUtils.SpawnDecal(prefab, myPos + Jitter + Vector3.up, Vector3.down, Offset, RaycastDist, Layers);
                }
            }
            else
            {
                if (Pooled)
                {
                    if (Recycle)
                    {
                        for (int i = 0; i < Count; i++)
                            Lazarus.RecycleSummon(prefab, myPos + Jitter);
                    }
                    else
                    {
                        for (int i = 0; i < Count; i++)
                            Lazarus.Summon(prefab, myPos + Jitter);
                    }
                }
                else
                {
                    for (int i = 0; i < Count; i++)
                        Instantiate(prefab, myPos + Jitter, Quaternion.identity);
                }
            }
        }

    }
}
