using Leopotam.Ecs.Types;
using Leopotam.EcsLite;
using UnityEngine;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Benchmark.OneComponent.Ecslite.V1
{
#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption (Option.DivideByZeroChecks, false)]
#endif
    public sealed class MoveSystem : IEcsRunSystem, IEcsInitSystem
    {
        public void Run(EcsWorlds worlds)
        {
            var world = worlds.Get();

            foreach (ref var entity in world.Query().With<Movable>())
            {
                ref var movable = ref entity.Get<Movable>();
                movable.Position += Float3.Forward;
            }
        }

        public void Init(EcsWorlds worlds)
        {
            var sharedData = worlds.Shared<LocalSharedState>();
            var world = worlds.Get();

            for (var i = 0; i < sharedData.Count; i++)
            {
                ref var movable = ref world.NewEntity().Get<Movable>();
                movable.Position = new Float3(
                    Random.Range(-100f, 100f),
                    Random.Range(-100f, 100f),
                    Random.Range(-100f, 100f)
                );
            }
        }
    }
}