using System;
using Leopotam.Ecs;

namespace Benchmark.OneComponent.LeoEcs.PoolRef
{
    public class LeoEcsPoolRefOneComponentBenchmark : IBenchmark
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private readonly OneComponentOptions _options;
        
        public LeoEcsPoolRefOneComponentBenchmark(OneComponentOptions options)
        {
            _options = options;
        }
        
        public void Start()
        {
            var lss = new LocalSharedState();
            lss.Count = _options.ComponentsCount;

            var worldCfg = new EcsWorldConfig()
            {
                FilterEntitiesCacheSize = lss.Count,
                WorldEntitiesCacheSize = lss.Count,
            };

            _world = new EcsWorld(worldCfg);
            _systems = new EcsSystems(_world);
            _systems
                .Add(new InitSystem())
                .Add(new MoveSystem())
                .Inject(lss)
                ;
            _systems.ProcessInjects();
            _systems.Init();
        }

        public void Update()
        {
            _systems.Run();
        }

        public void Destroy()
        {
            _systems?.Destroy();
            _systems = null;
            _world?.Destroy();
            _world = null;
        }

        public string GetSummary()
        {
            return $"Framework: LeoEcs.PoolRef{Environment.NewLine}{_options.GetSummary()}";

        }
    }
}