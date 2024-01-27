using System.Collections;
using UnityEngine;


namespace Peg.Behaviours
{
    /// <summary>
    /// Blends the settings of a light source over time upong being triggered by a Unity event.
    /// </summary>
    public class BlendAmbientLightOnEvent : AbstractOperationOnEvent
    {
        public Color Color;
        public float Intensity;
        public float BlendTime;
        
        static int CurrentBlendId;
        static Coroutine Queue;
        static AmbientRequest From;
        static AmbientRequest To;


        public class AmbientRequest
        {
            public Color Color;
            public float Intensity;
            public double BlendTime;


            public AmbientRequest(Color color, float intensity, double time)
            {
                Color = color;
                Intensity = intensity;
                BlendTime = time;
            }

            /// <summary>
            /// Blends this LightRequest settings from itself toward another request's settings based on a percentage.
            /// </summary>
            /// <param name="from"></param>
            /// <param name="percent"></param>
            public void BlendTo(AmbientRequest to, float percent, out Color color, out float intensity)
            {
                if (percent < 0) percent = 0;
                else if (percent > 1) percent = 1;

                color = Color.Lerp(Color, to.Color, percent);
                intensity = Mathf.Lerp(Intensity, to.Intensity, percent);
            }
            
            /// <summary>
            /// Applies this request's settings to a light
            /// </summary>
            /// <param name="light"></param>
            public static void ApplyToScene(Color color, float intensity)
            {
                RenderSettings.ambientLight = color;
                RenderSettings.ambientIntensity = intensity;
            }
        }

        
        public override void PerformOp()
        {
            From = new AmbientRequest(RenderSettings.ambientLight, RenderSettings.ambientIntensity, Time.timeAsDouble);
            To = new AmbientRequest(Color, Intensity, BlendTime);

            //unlike Lights, we can only have one AmbientLight blend at a time,
            //however, we don't want to override our own instance of a blend so we
            //need to track a unique id for each instance.
            if (Queue == null || CurrentBlendId != this.GetInstanceID())
            {
                //skip the coroutine if we are using instant-time. this also allows us to use this when trigged by disabled events
                if(BlendTime <= 0)
                {
                    AmbientRequest.ApplyToScene(Color, Intensity);
                    return;
                }
                CurrentBlendId = this.GetInstanceID();
                if(Queue != null) StopCoroutine(Queue);
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
                float i;
                var from = From;
                var to = To;
                double executionTime = Time.timeAsDouble - from.BlendTime;
                double percent = to.BlendTime <= 0 ? 1 : executionTime / to.BlendTime;

                from.BlendTo(to, (float)percent, out c, out i);
                AmbientRequest.ApplyToScene(c, i);

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
