using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Toolbox.Behaviours
{
    /// <summary>
    /// Used to fade the tint of a UI image over time.
    /// 
    /// TODO: Needs to support unscaled time too.
    /// </summary>
    public class FadeImageTintOnEvent : AbstractOperationOnEvent
    {
        [Space(5)]
        [Title("Fade Settings")]
        public Image Image;
        public Gradient Fade;
        public float FadeTime = 1f;
        [Tooltip("Does pressing any button (but not axis) skip the fade effect?")]
        public bool AnyButtonSkips;
        public UnityEvent OnFadeStart = new UnityEvent();
        public UnityEvent OnFadeComplete = new UnityEvent();

        //we use this to ensure we don't pick up the button release from a previous menu
        bool ButtonDown;

#if REWIRED
        /// <summary>
        /// Helper for 
        /// </summary>
        /// <returns></returns>
        public Rewired.Player GetMenuInput()
        {
            return Rewired.ReInput.players.GetPlayer(0);
        }
#endif

        public override void PerformOp()
        {
            StartFade();
        }

        /// <summary>
        /// For linking to UIs.
        /// </summary>
        public void StartFade()
        {
            Debug.LogWarning("There is still code in FadeImageTintOnEvent related directly to rewired input. This needs to be abstracted away and made into an interrupt that comes from outside sources.");
#if REWIRED
            ButtonDown = GetMenuInput().GetAnyButtonDown();
#endif
            StopAllCoroutines();
            StartCoroutine(FadeRoutine());
        }

        IEnumerator FadeRoutine()
        {
            OnFadeStart.Invoke();
            float start = Time.time;
            while (Time.time - start < FadeTime)
            {
#if REWIRED
                if(ButtonDown)
                {
                    if (GetMenuInput().GetAnyButtonUp())
                        ButtonDown = false;
                }
                else if(AnyButtonSkips && GetMenuInput().GetAnyButtonUp())
                    break;
#endif
                Image.color = Fade.Evaluate((Time.time - start) / FadeTime);
                yield return null;
            }
            Image.color = Fade.Evaluate(1);
            OnFadeComplete.Invoke();
        }


    }
}
