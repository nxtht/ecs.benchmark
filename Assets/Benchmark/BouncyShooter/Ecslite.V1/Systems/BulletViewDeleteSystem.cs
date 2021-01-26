using Leopotam.EcsLite;

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
    public sealed class BulletViewDeleteSystem : IEcsRunSystem
    {
        public void Run(EcsWorlds worlds)
        {
            var world = worlds.Get();
            var services = worlds.Shared<Services>();

            foreach (ref var entity in world.Query().With<Bullet>().With<Deleted>())
            {
                ref var bullet = ref entity.Get<Bullet>();
                if (services.BulletViewService.Views.TryGetValue(bullet.Id, out var viewTransform))
                {
                    viewTransform.gameObject.SetActive(false);
                    services.BulletViewService.Pool.Return(viewTransform);
                    services.BulletViewService.Views.Remove(bullet.Id);
                }
            }
        }
    }
}