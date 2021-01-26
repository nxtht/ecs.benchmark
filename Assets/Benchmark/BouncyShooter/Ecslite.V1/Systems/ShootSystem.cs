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
    public sealed class ShootSystem : IEcsRunSystem
    {
        public void Run(EcsWorlds worlds)
        {
            var world = worlds.Get();
            var services = worlds.Shared<Services>();
            var time = services.TimeService;
            var bulletService = services.BulletService;
            var sharedState = services.SharedState;
            
            foreach (ref var entity in world.Query()
                .With<Weapon>().With<Position>().With<Rotation>())
            {
                ref var weapon = ref entity.Get<Weapon>();
                weapon.FireTimeout -= time.DeltaTime;
                
                if (weapon.FireTimeout > 0)
                    continue;
                
                weapon.FireTimeout = sharedState.FireTimeout;
                ref var actorPosition = ref entity.Get<Position>();
                ref var actorRotation = ref entity.Get<Rotation>();

                var newEntity = world.NewEntity();
                ref var bullet = ref newEntity.Get<Bullet>();

                bullet.Id = bulletService.NextId;
                bullet.LifeTime = sharedState.BulletLifeTime;

                ref var bulletPosition = ref newEntity.Get<Position>();
                bulletPosition.Value = actorPosition.Value;
                    
                ref var bulletDesiredVel = ref newEntity.Get<DesiredVelocity>();
                bulletDesiredVel.Value = actorRotation.Value * sharedState.BulletSpeed;
            }
        }
    }
}