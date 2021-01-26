using Benchmark.Common;
using Leopotam.Ecs;
using Leopotam.Ecs.Types;

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
    public sealed class ActorByTypeUpdateSystem<TActorType> : IEcsRunSystem where TActorType : struct
    {
        private const float RotationSpeed = MathFast.Pi / 3;  
        private readonly TimeService _time = default;
        private readonly EcsFilter<Actor, Position, Rotation, TActorType>.Exclude<Deleted> _actors = default;

        public void Run()
        {
            foreach (var i in _actors)
            {
                ref var rotation = ref _actors.Get3(i);
                rotation.Value = rotation.Value.Rotate(_time.DeltaTime * RotationSpeed); 
            }
        }
    }
}