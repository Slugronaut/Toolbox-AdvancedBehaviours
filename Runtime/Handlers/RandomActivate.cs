using UnityEngine;
using System;


namespace Toolbox.Behaviours
{
    /// <summary>
    /// Randomly chooses to enable or disable each
    /// behaviour and gameObject in the list upon being enabled.
    /// 
    /// OBSOLETE: Use 'RandomActivateOnEvent' instead.
    /// 
    /// </summary>
    [System.Obsolete("Use 'RandomActivateOnEvent' instead. NOTE: Component needs to be implemented.")]
    public class RandomActivate : MonoBehaviour
    {
        [Tooltip("If set, will give all GameObjects and Behaviours the same setting.")]
        public bool Uniform = false;
        public GameObject[] GameObjects;
        public Behaviour[] Behaviours;

        void OnEnable()
        {
            bool flag = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));

            if(GameObjects != null)
            {
                for (int i = 0; i < GameObjects.Length; i++)
                {
                    GameObjects[i].SetActive(flag);
                    if(!Uniform) flag = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
                }

                for (int i = 0; i < Behaviours.Length; i++)
                {
                    Behaviours[i].enabled = flag;
                    if (!Uniform) flag = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
                }
            }
        }
    }
}
