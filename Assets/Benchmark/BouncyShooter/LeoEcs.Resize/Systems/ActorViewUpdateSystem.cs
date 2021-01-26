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
    public sealed class ActorViewUpdateSystem : IEcsRunSystem
    {
        private readonly SharedState _ss = default;
        private readonly ActorViewService _views = default;
        private readonly EcsFilter<Actor, Position, Rotation>.Exclude<Deleted> _actors = default;

        public void Run()
        {
            foreach (var i in _actors)
            {
                ref var actor = ref _actors.Get1(i);
                ref var position = ref _actors.Get2(i);
                ref var rotation = ref _actors.Get3(i);

                if (_views.Views.TryGetValue(actor.Id, out var viewTransform))
                {
                    viewTransform.position = new Vector3(position.Value.X, position.Value.Y);
                    viewTransform.rotation =
                        Quaternion.LookRotation(
                            Vector3.forward,
                            new Vector3(rotation.Value.X, rotation.Value.Y));
                }
                else
                {
                    var tr = _views.Pool.Rent();
                    tr.position = new Vector3(position.Value.X, position.Value.Y);
                    tr.rotation =
                        Quaternion.LookRotation(
                            Vector3.forward,
                            new Vector3(rotation.Value.X, rotation.Value.Y));
                    tr.localScale = (_ss.ActorRadius / 0.5f) * Vector3.one;
                    _views.Views.Add(actor.Id, tr);
                    tr.gameObject.SetActive(true);
                }
            }
        }
    }
}