using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Peg.Behaviours
{
    /// <summary>
    /// Blends the settings of a light source over time upong being triggered by a Unity event.
    /// </summary>
    public class BlendLightOnEvent : AbstractOperationOnEvent
    {
        public Light Light;
        public Color Color;
        public float Radius;
        public float Intensity;
        public float BlendTime;

        static Coroutine Queue;
        static HashSet<Tuple<LightRequest, LightRequest>> Pending = new();
        static List<Tuple<LightRequest, LightRequest>> RemoveList = new(5);


        public class LightRequest
        {
            public Light Light;
            public Color Color;
            public float Radius;
            public float Intensity;
            public double BlendTime;


            public LightRequest(Light light, Color color, float radius, float intensity, double time)
            {
                Light = light;
                Color = color;
                Radius = radius;
                Intensity = intensity;
                BlendTime = time;
            }


            /// <summary>
            /// Blends this LightRequest settings from itself toward another request's settings based on a percentage.
            /// </summary>
            /// <param name="from"></param>
            /// <param name="percent"></param>
            public void BlendTo(LightRequest to, float percent)
            {
                if (percent < 0) percent = 0;
                else if (percent > 1) percent = 1;

                Color = Color.Lerp(Color, to.Color, percent);
                Radius = Mathf.Lerp(Radius, to.Radius, percent);
                Intensity = Mathf.Lerp(Intensity, to.Intensity, percent);
            }

            /// <summary>
            /// Blends this LightRequest settings from itself toward another request's settings based on a percentage.
            /// </summary>
            /// <param name="from"></param>
            /// <param name="percent"></param>
            public void BlendTo(LightRequest to, float percent, out Color color, out float radius, out float intensity)
            {
                if (percent < 0) percent = 0;
                else if (percent > 1) percent = 1;

                color = Color.Lerp(Color, to.Color, percent);
                radius = Mathf.Lerp(Radius, to.Radius, percent);
                intensity = Mathf.Lerp(Intensity, to.Intensity, percent);
            }

            /// <summary>
            /// Applies this request's settings to a light
            /// </summary>
            /// <param name="light"></param>
            public void ApplyToLight(Light light)
            {
                light.color = Color;
                light.range = Radius;
                light.intensity = Intensity;
            }

            /// <summary>
            /// Applies this request's settings to a light
            /// </summary>
            /// <param name="light"></param>
            public static void ApplyToLight(Light light, Color color, float radius, float intensity)
            {
                light.color = color;
                light.range = radius;
                light.intensity = intensity;
            }
        }




        public override void PerformOp()
        {
            Pending.Add(new Tuple<LightRequest, LightRequest>(
                new LightRequest(Light, Light.color, Light.range, Light.intensity, Time.timeAsDouble), 
                new LightRequest(Light, Color, Radius, Intensity, BlendTime)));


            if (Queue == null)
                Queue = StartCoroutine(BlendLight());
        }


        IEnumerator BlendLight()
        {

            while(Pending.Count > 0)
            {
                yield return null;

                foreach(var pen in Pending)
                {
                    var from = pen.Item1;
                    var to = pen.Item2;
                    double executionTime = Time.timeAsDouble - from.BlendTime;
                    double percent = BlendTime / executionTime;

                    from.BlendTo(to, (float)percent, out Color c, out float r, out float i);
                    LightRequest.ApplyToLight(to.Light, c, r, i);

                    if(percent >= 1)
                        RemoveList.Add(pen);

                }

                for (int i = 0; i < RemoveList.Count; i++)
                    Pending.Remove(RemoveList[i]);

                RemoveList.Clear();
            }

            Queue = null;
            yield break;
        }
        
    }
}
