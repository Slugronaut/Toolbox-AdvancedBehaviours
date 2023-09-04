using UnityEngine;


namespace Peg.Behaviours
{
    /// <summary>
    /// Immediately activates and reactivates a set
    /// of GameObjects or components after Start().
    /// This can be used to resolve some kinds of race conditions
    /// where an object must be enabled *after* all others have run
    /// their Start() methods.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Re-enable on Start")]
    public class ReneableAfterStart : MonoBehaviour
    {
        public float StartDelay = 0;
        public float RestartDelay = 0;
        public GameObject[] GameObjects;
        public Behaviour[] Components;
        
        void Start()
        {
            if (StartDelay <= 0) TurnOff();
            else Invoke("TurnOff", StartDelay);

            
        }

        void TurnOff()
        {
            for (int i = 0; i < GameObjects.Length; i++)
                GameObjects[i].SetActive(false);

            for (int i = 0; i < Components.Length; i++)
                Components[i].enabled = false;

            if (RestartDelay <= 0) TurnOn();
            else Invoke("TurnOn", RestartDelay);
        }

        void TurnOn()
        {
            for (int i = 0; i < GameObjects.Length; i++)
                GameObjects[i].SetActive(true);

            for (int i = 0; i < Components.Length; i++)
                Components[i].enabled = true;
        }
    }
}
