using Peg.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Peg.Behaviours
{
    /// <summary>
    /// Attach this component to the player avatar to make it broadcast
    /// to the message dispatcher when it has come into existence.
    /// 
    /// TODO: 
    /// 
    /// 1) React to UnetScopeChange event. If we become not the owner, we can revoke
    /// our message as the local avatar
    /// 
    /// 2) Might want to link previous poster of this message and when
    /// this instance is destroyed, repost that previous object if it is still active.
    /// This way we could post-actively remove spawned avatars that were later removed
    /// due to a networking scope change (or some other event as well).
    /// </summary>
    [AddComponentMenu("Toolbox/Common/Trigger Player Spawned Event")]
    [DisallowMultipleComponent]
    public class PlayerAvatar : AbstractAuthoritativeGlobalPost<LocalPlayerAvatarSpawnedEvent>
    {
        public EntityRoot Root { get; private set; }

        /// <summary>
        /// The player id. Starts at 1 and increments for each player.
        /// </summary>
        [Range(1,32)]
        public int PlayerId = 1;
        bool Posted;

        protected virtual void Awake()
        {
            Root = GetComponent<EntityRoot>();
        }

        protected virtual void Start()
        {
            if (ValidateAuthority())
            {
                PostMessage();
                Posted = true;
            }
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(Posted) GlobalMessagePump.Instance.PostMessage(new LocalPlayerAvatarRemovedEvent(gameObject, PlayerId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override LocalPlayerAvatarSpawnedEvent ActivateMsg()
        {
            return new LocalPlayerAvatarSpawnedEvent(gameObject, PlayerId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        void SceneUnloaded(Scene scene)
        {
            CleanupMessage();
        }
    }
    
    
    public class LocalPlayerAvatarSpawnedEvent : TargetMessage<GameObject, LocalPlayerAvatarSpawnedEvent>, IDeferredMessage, IBufferedMessage
    {
        public int PlayerId { get; private set; }

        public LocalPlayerAvatarSpawnedEvent(GameObject avatar, int id) : base(avatar)
        {
            PlayerId = id;
        }
    }
    
    
    public class LocalPlayerAvatarRemovedEvent : TargetMessage<GameObject, LocalPlayerAvatarRemovedEvent>
    {
        public int PlayerId { get; private set; }

        public LocalPlayerAvatarRemovedEvent(GameObject avatar, int id) : base(avatar)
        {
            PlayerId = id;
        }
    }
    
    
    /// <summary>
    /// Base class for rigging events that should be triggered when the player avatar is spawned.
    /// </summary>
    public abstract class AttachToPlayerOnSpawn : Sirenix.OdinInspector.SerializedMonoBehaviour
    {
        protected virtual void Awake()
        {
            GlobalMessagePump.Instance.AddListener<LocalPlayerAvatarSpawnedEvent>(OnLocalPlayerAvatarSpawned);
            GlobalMessagePump.Instance.AddListener<LocalPlayerAvatarRemovedEvent>(OnLocalPlayerAvatarRemoved);
        }

        /// <summary>
        /// For the love of god if you override this class don't forget to invoke the base method!
        /// </summary>
        protected virtual void OnDestroy()
        {
            GlobalMessagePump.Instance.RemoveListener<LocalPlayerAvatarSpawnedEvent>(OnLocalPlayerAvatarSpawned);
            GlobalMessagePump.Instance.RemoveListener<LocalPlayerAvatarRemovedEvent>(OnLocalPlayerAvatarRemoved);
        }

        protected abstract void OnLocalPlayerAvatarSpawned(LocalPlayerAvatarSpawnedEvent msg);
        protected abstract void OnLocalPlayerAvatarRemoved(LocalPlayerAvatarRemovedEvent msg);


    }
}
