using UnityEngine;

namespace Peg.Behaviours
{
    /// <summary>
    /// Returns the gameobject to the pool from which it came upon colliding with a physics collider.
    /// </summary>
    public class RelenquishToPoolOnCollide : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Lazarus.Lazarus.Instance.RelenquishToPool(gameObject);
        }
    }
}
