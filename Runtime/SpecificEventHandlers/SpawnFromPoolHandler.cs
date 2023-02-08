using Sirenix.OdinInspector;
using Toolbox.AutoCreate;
using Toolbox.Lazarus;
using Toolbox.Math;
using UnityEngine;
using UnityEngine.Assertions;
using static Toolbox.Behaviours.SpawnFromPoolOnEvent;

namespace Toolbox.Behaviours
{
    /// <summary>
    /// Simple event handlers that can be linked to UnityEvents in the inspector in
    /// order to spawn pooled objects.
    /// </summary>
    public class SpawnFromPoolHandler : MonoBehaviour
    {

        #region Members
        [SerializeField]
        [HideInInspector]
        byte SerializedFlags = (byte)SpawnFromPoolFlags.Pooled | (byte)SpawnFromPoolFlags.OrientDecal;

        [Space(15)]
        [Header("Spawn Settings")]
        public SelectionModes SelectionMode;
        [ShowIf("IsIndexedMode")]
        [Indent]
        public ushort SelectionIndex;
        bool IsIndexedMode => SelectionMode == SelectionModes.Specific;

        [PropertyOrder(0)]
        [Tooltip("An offset applied to the spawn point.")]
        public Vector3 Offset;
        [PropertyOrder(1)]
        [Tooltip("The amount of jitter in world space.")]
        public Vector3 Jitter;


        [PropertyOrder(2)]
        [ShowInInspector]
        [PropertyTooltip("Should spwned objects be placed and oriented against a surface?")]
        public bool Decal
        {
            get { return (SerializedFlags & (byte)SpawnFromPoolFlags.Decal) != 0; }
            set
            {
                if (value) SerializedFlags |= (byte)SpawnFromPoolFlags.Decal;
                else SerializedFlags &= ((byte)SpawnFromPoolFlags.Decal ^ 0xff);
            }
        }

        [HideInInspector]
        [SerializeField]
        LayerMask _Layers;

        [PropertyOrder(3)]
        [ShowInInspector]
        [ShowIf("Decal")]
        [Indent(1)]
        public LayerMask Layers
        {
            get => _Layers;
            set => _Layers = value;
        }

        [PropertyOrder(3)]
        [ShowInInspector]
        [ShowIf("Decal")]
        [Indent(1)]
        [PropertyTooltip("If it's decaled, will it have it's facing orientation set? Turning this off will ensure an exact spawning point based on a raycast aginst the colliding surface but will leave the spawned object in it's default orientation.")]
        public bool OrientDecal
        {
            get => (SerializedFlags & (byte)SpawnFromPoolFlags.OrientDecal) != 0;
            set
            {
                if (value) SerializedFlags |= (byte)SpawnFromPoolFlags.OrientDecal;
                else SerializedFlags &= ((byte)SpawnFromPoolFlags.OrientDecal ^ 0xff);
            }
        }


        [PropertyOrder(4)]
        [ShowIf("Decal")]
        [Indent(1)]
        [Tooltip("If a decal, how far down to raycast when checking for placement on the ground.")]
        public float RaycastDist = 2;


        [PropertyOrder(5)]
        [ShowIf("Decal")]
        [Indent]
        [ShowInInspector]
        [PropertyTooltip("If a decal raycast fails, should we simply spawn where we are now or not spawn at all?")]
        public bool SpawnIfRaycastFails
        {
            get { return (SerializedFlags & (byte)SpawnFromPoolFlags.SpawnIfRayFail) != 0; }
            set
            {
                if (value) SerializedFlags |= (byte)SpawnFromPoolFlags.SpawnIfRayFail;
                else SerializedFlags &= ((byte)SpawnFromPoolFlags.SpawnIfRayFail ^ 0xff);
            }
        }

        [PropertyOrder(6)]
        [ShowInInspector]
        [PropertyTooltip("Should a pool be used or should this be spawned raw?")]
        public bool Pooled
        {
            get { return (SerializedFlags & (byte)SpawnFromPoolFlags.Pooled) != 0; }
            set
            {
                if (value) SerializedFlags |= (byte)SpawnFromPoolFlags.Pooled;
                else SerializedFlags &= ((byte)SpawnFromPoolFlags.Pooled ^ 0xff);
            }
        }


        [PropertyOrder(7)]
        [ShowIf("Pooled")]
        [Indent]
        [ShowInInspector]
        [PropertyTooltip("Should pooled objects that are currently active be re-used when the pool runs dry? If false, when new objects are needed the pool will be increased.")]
        public bool Recycle
        {
            get { return (SerializedFlags & (byte)SpawnFromPoolFlags.Recycle) != 0; }
            set
            {
                if (value) SerializedFlags |= (byte)SpawnFromPoolFlags.Recycle;
                else SerializedFlags &= ((byte)SpawnFromPoolFlags.Recycle ^ 0xff);
            }
        }

        [PropertyOrder(8)]
        [Tooltip("An optional cooldown timer that can limit the frequency that spawning occurs. A value of zero allows spawning to occur at the current framerate.")]
        public float Cooldown;


        [PropertyOrder(9)]
        [Tooltip("How many instances to spawn.")]
        [MinValue(1)]
        public ushort Count = 1;


        [PropertyOrder(10)]
        [Tooltip("One of these will randomly be spawned.")]
        public WeightedGameObject[] Prefabs;

        float LastTime;
        byte LastIndex;
        IPoolSystem Lazarus;
        #endregion

        protected virtual void Awake()
        {
            Lazarus = AutoCreator.AsSingleton<IPoolSystem>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Spawn()
        {
            if (Prefabs == null || Prefabs.Length < 1) return;
            if (Cooldown > 0 && Time.time - LastTime < Cooldown)
                return;
            LastTime = Time.time;
            Vector3 jitter = Jitter; //todo: randomize this
            var myPos = transform.position + Offset + jitter;
            GameObject prefab = null;


            switch (SelectionMode)
            {
                case SelectionModes.RandomOneType:
                    {
                        prefab = Prefabs.SelectWeightedRandom(); //Prefabs[Random.Range(0, Prefabs.Length)];
                        break;
                    }
                case SelectionModes.Sequential:
                    {
                        if (LastIndex >= Prefabs.Length)
                            LastIndex = 0;
                        prefab = Prefabs[LastIndex].Value;
                        LastIndex++;
                        break;
                    }
                case SelectionModes.Specific:
                    {
                        prefab = Prefabs[SelectionIndex].Value;
                        break;
                    }
                //handle randomize each time here
                case SelectionModes.All:
                    {
                        for (int i = 0; i < Prefabs.Length; i++)
                        {
                            for (int x = 0; x < Count; x++)
                            {
                                if (Decal)
                                {
                                    if (OrientDecal) SpawnAsDecal(Lazarus, myPos, Pooled, Recycle, transform.forward, RaycastDist, Layers, SpawnIfRaycastFails, Prefabs[i].Value);
                                    else SpawnAsNonOrientedDecal(Lazarus, myPos, Pooled, Recycle, transform.forward, RaycastDist, Layers, SpawnIfRaycastFails, Prefabs[i].Value);
                                }
                                else SpawnInPlace(Lazarus, myPos, Pooled, Recycle, Prefabs[i].Value);
                            }
                        }
                        return;
                    }
            }


            //handle single-instance of random here
            for (int i = 0; i < Count; i++)
            {
                if (SelectionMode == SelectionModes.Random)
                    prefab = Prefabs.SelectWeightedRandom();// Prefabs[Random.Range(0, Prefabs.Length)];

                if (Decal)
                {
                    if (OrientDecal) SpawnAsDecal(Lazarus, myPos, Pooled, Recycle, transform.forward, RaycastDist, Layers, SpawnIfRaycastFails, prefab);
                    else SpawnAsNonOrientedDecal(Lazarus, myPos, Pooled, Recycle, transform.forward, RaycastDist, Layers, SpawnIfRaycastFails, prefab);
                }
                else SpawnInPlace(Lazarus, myPos, Pooled, Recycle, prefab);
            }
        }


    }
}
