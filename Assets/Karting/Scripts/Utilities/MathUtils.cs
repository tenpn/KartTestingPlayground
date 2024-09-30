using UnityEngine;

namespace KartGame
{
    public static class MathUtils
    {
        public static bool IsApproximately(float a, float b, float epsilon)
        {
            return Mathf.Abs(a - b) <= epsilon;
        }
    }
}