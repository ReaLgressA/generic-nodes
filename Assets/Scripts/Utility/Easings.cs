using UnityEngine;

namespace Utility {
    public static class Easings {

        public static float EaseLinear(float x) {
            return x;
        }
        
        public static float EaseInOutQuad(float x) {
            return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
        }
    }
}