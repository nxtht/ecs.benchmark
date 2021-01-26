using Leopotam.Ecs.Types;

namespace Benchmark.OneComponent
{
    public sealed class LocalSharedState
    {
        public int Count;
        public Float3 MoveVector = Float3.Forward;
    }
}