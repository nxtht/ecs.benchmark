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
    public sealed class ShootSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        private readonly SharedState _sharedState = default;
        private readonly TimeService _time = default;
        private readonly BulletService _bulletService = default;
        private readonly EcsFilter<Weapon, Position, Rotation> _shooters = null;

        public void Run()
        {
            foreach (var i in _shooters)
            {
                ref var weapon = ref _shooters.Get1(i);
                weapon.FireTimeout -= _time.DeltaTime;
                if (weapon.FireTimeout <= 0)
                {
                    weapon.FireTimeout = _sharedState.FireTimeout;
                    ref var actorPosition = ref _shooters.Get2(i);
                    ref var actorRotation = ref _shooters.Get3(i);

                    var bullet = new Bullet()
                    {
                        Id = _bulletService.NextId,
                        LifeTime = _sharedState.BulletLifeTime
                    };
                    
                    var bulletPosition = new Position() {Value = actorPosition.Value};
                    var bulletDesiredVel = new DesiredVelocity()
                        {Value = actorRotation.Value * _sharedState.BulletSpeed};

                    _world.NewEntity()
                        .Replace(bullet)
                        .Replace(bulletPosition)
                        .Replace(bulletDesiredVel)
                        ;
                }
            }
        }
    }
}