using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GenericNodes.Utility
{
    public static class Tween 
    {
        public static async Task TweenAnchoredPos(this RectTransform rTransform, Vector2 to, float durationSec, 
                                                  Func<float, float> easing = null) {
            easing ??= Easings.EaseLinear;
            Vector2 from = rTransform.anchoredPosition;
            Vector2 position = from;
            DateTime startTime = DateTime.Now;
            float sec;
            do {
                sec = (float) TimeSpan.FromTicks(DateTime.Now.Ticks - startTime.Ticks).TotalSeconds;
                float t = easing(Mathf.Clamp01(sec / durationSec));
                position.x = (to.x - from.x) * t + from.x;
                position.y = (to.y - from.y) * t + from.y;
                rTransform.anchoredPosition = position;
                await Task.Yield();
            } while (sec < durationSec);
            rTransform.anchoredPosition = to;
            await Task.CompletedTask;
        }
    
    }
}
