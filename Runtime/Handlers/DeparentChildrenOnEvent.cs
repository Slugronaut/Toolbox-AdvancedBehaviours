using Peg.Behaviours;
using UnityEngine;


namespace Peg.Behaviours
{
    /// <summary>
    /// Removes all children from this object and optionally destroys it.
    /// </summary>
    public class DeparentChildrenOnEvent : AbstractOperationOnEvent
    {
        [Tooltip("Is this object destroyed after deparenting?")]
        public bool DestroySelf = true;

        [Tooltip("The transform to re-parent the recently evicted children of this object. Can be null to specify the scene root.")]
        public Transform NewParent = null;

        [Tooltip("When reparenting, do the children remain in their current world position or are they moved to match their current local position?")]
        public bool WorldPositionStays = true;



        public override void PerformOp()
        {
            Transform trans = transform;

            while (trans.childCount > 0)
            {
                var child = trans.GetChild(0);
                child.SetParent(NewParent, WorldPositionStays);

            }

            if (DestroySelf)
                Destroy(gameObject);
        }
        
    }
}
