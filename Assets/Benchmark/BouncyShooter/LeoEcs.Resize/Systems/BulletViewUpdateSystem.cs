using Leopotam.Ecs;
using UnityEngine;

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
    public sealed class BulletViewUpdateSystem : IEcsRunSystem
    {
        private readonly SharedState _ss = default;
        private readonly BulletViewService _views = default;
        private readonly EcsFilter<Bullet, Position>.Exclude<Deleted> _bullets = default;

        public void Run()
        {
            foreach (var i in _bullets)
            {
                ref var bullet = ref _bullets.Get1(i);
                ref var position = ref _bullets.Get2(i);
                
                if (_views.Views.TryGetValue(bullet.Id, out var viewTransform))
                {
                    viewTransform.position = new Vector3(position.Value.X, position.Value.Y);
                }
                else
                {
                    var tr = _views.Pool.Rent();
                    tr.position = new Vector3(position.Value.X, position.Value.Y);
                    tr.localScale = (_ss.BulletRadius / 0.5f) * Vector3.one;
                    _views.Views.Add(bullet.Id, tr);
                    tr.gameObject.SetActive(true);
                }
            }
        }
    }
}