using Leopotam.Ecs;
using Leopotam.Ecs.Types;
using UnityEngine;

namespace Benchmark.OneComponent.LeoEcs.PoolRef
{
    public sealed class InitSystem : IEcsInitSystem {
        private readonly EcsWorld _world = null;
        private readonly LocalSharedState _lss = null;

        public void Init () {
            _world.GetPool<Movable> ().SetCapacity (_lss.Count);
            for (int i = 0, iMax = _lss.Count; i < iMax; i++) {
                ref var movable = ref _world.NewEntity ().Get<Movable> ();
                movable.Position = new Float3(
                    Random.Range (-100f, 100f), 
                    Random.Range (-100f, 100f), 
                    Random.Range (-100f, 100f));
            }
        }
    }
}