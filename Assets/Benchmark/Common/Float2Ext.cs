using Leopotam.Ecs.Types;

namespace Benchmark.Common
{
    public static class Float2Ext
    {
        public static Float2 RotateD(this Float2 f, float degrees)
        {
            return f.Rotate(degrees * MathFast.Deg2Rad);
        }

        public static Float2 Rotate(this Float2 v, float radians)
        {
            var ca = MathFast.Cos(radians);
            var sa = MathFast.Sin(radians);
            return new Float2(ca*v.X - sa*v.Y, sa*v.X + ca*v.Y);
        }
    }
}