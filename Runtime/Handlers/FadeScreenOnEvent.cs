﻿using Peg.Systems;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Peg.Behaviours
{
    /// <summary>
    /// Used to transition to new scenes with smooth effects like screen fade and BGM corssfade.
    /// </summary>
    public class FadeScreenOnEvent : AbstractOperationOnEvent
    {
        public enum FadeDirections
        {
            FadeTo,
            FadeFrom,
        }

        [Space(5)]
        [Title("Fade Settings")]
        public FadeDirections Direction;
        public Color FadeColor = Color.black;
        public float FadeTime = 0.75f;
        [Tooltip("Does pressing any button (but not axis) skip the fade effect?")]
        public bool AnyButtonSkips;
        public UnityEvent OnFadeComplete = new UnityEvent();
        
        System.Action CachedCallback;


        public override void PerformOp()
        {
            if (Direction == FadeDirections.FadeTo)
                Fadeout();
            else Fadein();
        }

        /// <summary>
        /// For linking to UIs.
        /// </summary>
        public void Fadeout()
        {
            ScreenUIFadeUtility.Instance.FadeTo(FadeColor, FadeTime, true, FadeComplete, FadeUpdate);
        }

        /// <summary>
        /// For linking to UIs.
        /// </summary>
        public void Fadeout(System.Action callback)
        {
            CachedCallback = callback;
            ScreenUIFadeUtility.Instance.FadeTo(FadeColor, FadeTime, true, FadeComplete, FadeUpdate);
        }

        /// <summary>
        /// For linking to UIs.
        /// </summary>
        public void Fadein()
        {
            ScreenUIFadeUtility.Instance.FadeFrom(FadeColor, FadeTime, FadeComplete, FadeUpdate);
        }

        /// <summary>
        /// For linking to UIs.
        /// </summary>
        public void Fadein(System.Action callback)
        {
            CachedCallback = callback;
            ScreenUIFadeUtility.Instance.FadeFrom(FadeColor, FadeTime, FadeComplete, FadeUpdate);
        }

        void FadeUpdate()
        {
            if (AnyButtonSkips && Input.anyKeyDown)
                ScreenUIFadeUtility.Instance.EndFadeout();
        }

        void FadeComplete()
        {
            var callback = CachedCallback;
            callback?.Invoke();
            OnFadeComplete.Invoke();
        }

    }
}
