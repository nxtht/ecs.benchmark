using Leopotam.Ecs;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Benchmark.BouncyShooter.LeoEcs.Resize
{
#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption (Option.DivideByZeroChecks, false)]
#endif
    public sealed class MoveSystem : IEcsRunSystem
    {
        private readonly TimeService _time = default;
        private readonly EcsFilter<Position, DesiredVelocity> _movers = null;

        public void Run()
        {
            foreach (var i in _movers)
            {
                ref var position = ref _movers.Get1(i);
                ref var desVelocity = ref _movers.Get2(i);
                position.Value += desVelocity.Value * _time.DeltaTime;
            }
        }
    }
}