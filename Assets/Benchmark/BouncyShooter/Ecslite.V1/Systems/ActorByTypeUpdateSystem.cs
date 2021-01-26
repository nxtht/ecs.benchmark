using Benchmark.Common;
using Leopotam.Ecs.Types;
using Leopotam.EcsLite;
using UnityEngine;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Benchmark.BouncyShooter.Ecslite.V1
{
#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption (Option.DivideByZeroChecks, false)]
#endif
    public sealed class ActorByTypeUpdateSystem<TActorType> : IEcsRunSystem where TActorType : struct
    {
        private const float RotationSpeed = MathFast.Pi / 3;

        public void Run(EcsWorlds worlds)
        {
            var world = worlds.Get();
            var services = worlds.Shared<Services>();
            var time = services.TimeService;

            foreach (ref var entity in world.Query()
                .With<Actor>().With<Position>().With<Rotation>().With<TActorType>().Without<Deleted>())
            {
                ref var rotation = ref entity.Get<Rotation>();
                rotation.Value = rotation.Value.Rotate(time.DeltaTime * RotationSpeed);
            }
        }
    }
}