using System.Runtime.CompilerServices;
using UnityEngine;

namespace Benchmark
{
    public static class Utils
    {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static int GetNextPoT (int v) {
            if (v < 2) {
                return 2;
            }
            var n = v - 1;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            return n + 1;
        }
        
        public static Vector2 GetPointOnCircle() {
            float randomAngle = UnityEngine.Random.Range(0f, 2 * Mathf.PI - float.Epsilon);
            return new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        }
    }
}