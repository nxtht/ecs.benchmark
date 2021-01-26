using Leopotam.Ecs;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Benchmark.OneComponent.LeoEcs.Resize
{
#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption (Option.DivideByZeroChecks, false)]
#endif
    public sealed class MoveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Movable> _movables = null;
        private readonly LocalSharedState _lss = null;

        public void Run()
        {
            foreach (var i in _movables)
            {
                ref var movable = ref _movables.Get1(i);
                movable.Position += _lss.MoveVector;
            }
        }
    }
}