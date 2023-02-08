using Toolbox.Behaviours;
using UnityEngine;


namespace Toolbox.Behaviours
{
    /// <summary>
    /// Sets the parent of a set of GameObjects when the specified Unity event occurs.
    /// </summary>
    public class SetParentOnEvent : AbstractOperationOnEvent
    {
        [Tooltip("The transform to re-parent the recently evicted children of this object. Can be null to specify the scene root.")]
        public Transform NewParent = null;

        [Tooltip("When reparenting, do the children remain in their current world position or are they moved to match their current local position?")]
        public bool WorldPositionStays = true;

        [Tooltip("The list of objects who are to be moved.")]
        public Transform[] Trans;



        public override void PerformOp()
        {
            for(int i = 0; i < Trans.Length; i++)
            {
                var trans = Trans[i];
                if (trans != null)
                    trans.SetParent(NewParent, WorldPositionStays);
            }
        }

    }
}
