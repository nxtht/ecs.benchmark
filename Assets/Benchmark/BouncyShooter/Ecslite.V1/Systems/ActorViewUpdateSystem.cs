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
    public sealed class ActorViewUpdateSystem : IEcsRunSystem
    {
        public void Run(EcsWorlds worlds)
        {
            var world = worlds.Get();
            var services = worlds.Shared<Services>();
            
            foreach (ref var entity in world.Query()
                .With<Actor>().With<Position>().With<Rotation>().Without<Deleted>())
            {
                ref var actor = ref entity.Get<Actor>();
                ref var position = ref entity.Get<Position>();
                ref var rotation = ref entity.Get<Rotation>();
                
                if (services.ActorViewService.Views.TryGetValue(actor.Id, out var viewTransform))
                {
                    viewTransform.position = new Vector3(position.Value.X, position.Value.Y);
                    viewTransform.rotation =
                        Quaternion.LookRotation(
                            Vector3.forward,
                            new Vector3(rotation.Value.X, rotation.Value.Y));
                }
                else
                {
                    var tr = services.ActorViewService.Pool.Rent();
                    tr.position = new Vector3(position.Value.X, position.Value.Y);
                    tr.rotation =
                        Quaternion.LookRotation(
                            Vector3.forward,
                            new Vector3(rotation.Value.X, rotation.Value.Y));
                    tr.localScale = (services.SharedState.ActorRadius / 0.5f) * Vector3.one;
                    services.ActorViewService.Views.Add(actor.Id, tr);
                    tr.gameObject.SetActive(true);
                }
            }
        }
    }
}