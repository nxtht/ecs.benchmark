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
    public sealed class BulletViewUpdateSystem : IEcsRunSystem
    {
        public void Run(EcsWorlds worlds)
        {
            var world = worlds.Get();
            var services = worlds.Shared<Services>();

            foreach (ref var entity in world.Query().With<Bullet>().With<Position>().Without<Deleted>())
            {
                ref var bullet = ref entity.Get<Bullet>();
                ref var position = ref entity.Get<Position>();

                if (services.BulletViewService.Views.TryGetValue(bullet.Id, out var viewTransform))
                {
                    viewTransform.position = new Vector3(position.Value.X, position.Value.Y);
                }
                else
                {
                    var tr = services.BulletViewService.Pool.Rent();
                    tr.position = new Vector3(position.Value.X, position.Value.Y);
                    tr.localScale = (services.SharedState.BulletRadius / 0.5f) * Vector3.one;
                    services.BulletViewService.Views.Add(bullet.Id, tr);
                    tr.gameObject.SetActive(true);
                }
            }
        }
    }
}