using UnityEngine;


namespace Toolbox.Behaviours
{
    /// <summary>
    /// Deactivates a gameObject if a behaviour is disabled.
    /// This occurs during the Update cycle of the frame.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Deactivate if Disabled")]
    public sealed class DeactivateIfDisabled : MonoBehaviour
    {
        public Behaviour TargetBehaviour;
        public GameObject TargetGameObject;
        public bool BehaviourStateTrigger = false;
        public bool GameObjectState = false;
        
        void Update()
        {
            if (TargetBehaviour.enabled == BehaviourStateTrigger &&
               TargetGameObject.activeSelf != GameObjectState)
                TargetGameObject.SetActive(GameObjectState);
        }
    }
}
