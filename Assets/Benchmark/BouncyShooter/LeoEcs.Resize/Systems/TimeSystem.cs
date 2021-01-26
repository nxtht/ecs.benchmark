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
    public sealed class TimeSystem : IEcsRunSystem
    {
        readonly TimeService _time = default;

        public void Run()
        {
            _time.DeltaTime = Time.deltaTime;
        }
    }
}