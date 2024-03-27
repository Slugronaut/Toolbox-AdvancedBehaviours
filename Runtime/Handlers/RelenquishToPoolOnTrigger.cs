using UnityEngine;

namespace Peg.Behaviours
{
    /// <summary>
    /// Returns this gameobject to the pool from which it came upon colliding with a physics trigger.
    /// </summary>
    public class RelenquishToPoolOnTrigger : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            Lazarus.Lazarus.Instance.RelenquishToPool(gameObject);
        }
    }
}
