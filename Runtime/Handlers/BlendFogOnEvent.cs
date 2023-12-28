using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;


namespace Peg.Behaviours
{
    /// <summary>
    /// Blends the settings of a light source over time upong being triggered by a Unity event.
    /// </summary>
    public class BlendFogOnEvent : AbstractOperationOnEvent
    {
        public Color Color;
        [HideIf("IsExponentialFog")]
        public float Start;
        [HideIf("IsExponentialFog")]
        public float End;
        [ShowIf("IsExponentialFog")]
        public float Density;
        public float BlendTime;

        static int CurrentBlendId;
        static Coroutine Queue;
        static AmbientRequest From;
        static AmbientRequest To;



        bool IsExponentialFog => RenderSettings.fogMode == FogMode.Exponential || RenderSettings.fogMode == FogMode.ExponentialSquared;
        

        [Serializable]
        public class AmbientRequest
        {
            public Color Color;
            public float StartDist;
            public float EndDist;
            public double BlendTime;


            public AmbientRequest(Color color, float startDist, float endDist, double time)
            {
                Color = color;
                StartDist = startDist;
                EndDist = endDist;
                BlendTime = time;
            }
            public AmbientRequest(Color color, float density, double time)
            {
                Color = color;
                EndDist = density;
                BlendTime = time;
            }

            /// <summary>
            /// Blends this LightRequest settings from itself toward another request's settings based on a percentage.
            /// </summary>
            /// <param name="from"></param>
            /// <param name="percent"></param>
            public void LinearBlendTo(AmbientRequest to, float percent, out Color color, out float startDist, out float endDist)
            {
                if (percent < 0) percent = 0;
                else if (percent > 1) percent = 1;

                color = Color.Lerp(Color, to.Color, percent);
                startDist = Mathf.Lerp(StartDist, to.StartDist, percent);
                endDist = Mathf.Lerp(EndDist, to.EndDist, percent);
            }

            /// <summary>
            /// Blends this LightRequest settings from itself toward another request's settings based on a percentage.
            /// </summary>
            /// <param name="from"></param>
            /// <param name="percent"></param>
            public void ExponentialBlendTo(AmbientRequest to, float percent, out Color color, out float density)
            {
                if (percent < 0) percent = 0;
                else if (percent > 1) percent = 1;

                color = Color.Lerp(Color, to.Color, percent);
                density = Mathf.Lerp(EndDist, to.EndDist, percent);
            }

        }


        public override void PerformOp()
        {
            if (IsExponentialFog)
            {
                From = new AmbientRequest(RenderSettings.fogColor, RenderSettings.fogDensity, Time.timeAsDouble);
                To = new AmbientRequest(Color, Density, BlendTime);
            }
            else
            {
                From = new AmbientRequest(RenderSettings.fogColor, RenderSettings.fogStartDistance, RenderSettings.fogEndDistance, Time.timeAsDouble);
                To = new AmbientRequest(Color, Start, End, BlendTime);
            }

            //unlike Lights, we can only have one AmbientLight blend at a time,
            //however, we don't want to override our own instance of a blend so we
            //need to track a unique id for each instance.
            if (Queue == null || CurrentBlendId != this.GetInstanceID())
            {
                CurrentBlendId = this.GetInstanceID();
                if (Queue != null) StopCoroutine(Queue);
                Queue = StartCoroutine(BlendLight());
            }
        }


        IEnumerator BlendLight()
        {
            bool blending = true;

            while (blending)
            {
                yield return null;

                Color c;
                var from = From;
                var to = To;
                double executionTime = Time.timeAsDouble - from.BlendTime;
                double percent = to.BlendTime <= 0 ? 1 : executionTime / to.BlendTime;

                if (IsExponentialFog)
                {
                    from.ExponentialBlendTo(to, (float)percent, out c, out float density);
                    RenderSettings.fogDensity = density;
                }
                else
                {
                    from.LinearBlendTo(to, (float)percent, out c, out float start, out float end);
                    RenderSettings.fogStartDistance = start;
                    RenderSettings.fogEndDistance = end;
                }
                RenderSettings.fogColor = c;

                if (percent >= 1)
                    blending = false;
            }

            From = null;
            To = null;
            Queue = null;
            yield break;
        }

    }
}
