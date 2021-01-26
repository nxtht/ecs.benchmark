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
    public sealed class BulletViewDeleteSystem : IEcsRunSystem
    {
        private readonly BulletViewService _views = default;
        private readonly EcsFilter<Bullet, Deleted> _bullets = default;

        public void Run()
        {
            foreach (var i in _bullets)
            {
                ref var bullet = ref _bullets.Get1(i);
                if (_views.Views.TryGetValue(bullet.Id, out var viewTransform))
                {
                    viewTransform.gameObject.SetActive(false);
                    _views.Pool.Return(viewTransform);
                    _views.Views.Remove(bullet.Id);
                }
            }
        }
    }
}