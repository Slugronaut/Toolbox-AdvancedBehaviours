using UnityEngine;
using System;

namespace Peg.Behaviours
{
    /// <summary>
    /// Attach to a GameObject that should move to the 2D world-space location clicked by the mouse.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Position on Mouse Hold 2D")]
    public class PositionMeOnMouseHold2D : MonoBehaviour
    {
        [Tooltip("The mouse button to which this will react.")]
        public int Button = 0;
        
        void Update()
        {
            if (Input.GetMouseButton(Button))
            {
                transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }
}