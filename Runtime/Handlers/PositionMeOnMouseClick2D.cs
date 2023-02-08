using UnityEngine;
using System;

namespace Toolbox.Behaviours
{
    /// <summary>
    /// Attach to a GameObject that should move to the 2D world-space location clicked by the mouse.
    /// It will, however, not reposition until the mouse button is released and clicked again.
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Position on Mouse Click 2D")]
    public class PositionMeOnMouseClick2D : MonoBehaviour
    {
        [Tooltip("The mouse button to which this will react.")]
        public int Button = 0;

        void Update()
        {
            if (Input.GetMouseButtonDown(Button)) transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}