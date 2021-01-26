using Leopotam.EcsLite;
using UnityEngine;

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
    public sealed class TimeSystem : IEcsRunSystem
    {
        public void Run(EcsWorlds worlds)
        {
            var services = worlds.Shared<Services>();
            services.TimeService.DeltaTime = Time.deltaTime;
        }
    }
}