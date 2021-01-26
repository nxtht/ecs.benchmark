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
    public sealed class BulletDeleteSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Bullet, Deleted> _bullets = null;

        public void Run()
        {
            foreach (var i in _bullets)
            {
                ref var entity = ref _bullets.GetEntity(i);
                entity.Destroy();
            }
        }
    }
}