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
    public sealed class BulletSystem : IEcsRunSystem
    {
        public void Run(EcsWorlds worlds)
        {
            var world = worlds.Get();
            var services = worlds.Shared<Services>();
            var time = services.TimeService;
            
            foreach (ref var entity in world.Query().With<Bullet>())
            {
                ref var bullet = ref entity.Get<Bullet>();
                
                bullet.LifeTime -= time.DeltaTime;
                if (bullet.LifeTime <= 0)
                {
                    entity.Get<Deleted>();
                }
            }
        }
    }
}