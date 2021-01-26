using System;
using Leopotam.EcsLite;

namespace Benchmark.OneComponent.Ecslite.V1
{
    public class EcsliteV1OneComponentBenchmark : IBenchmark
    {
        private EcsWorlds _worlds;
        private EcsSystems _systems;
        private readonly OneComponentOptions _options;

        public EcsliteV1OneComponentBenchmark(OneComponentOptions options)
        {
            _options = options;
        }

        public void Start()
        {
            var lss = new LocalSharedState();
            lss.Count = _options.ComponentsCount;

            var worldCfg = new EcsWorldConfig()
            {
                ComponentsCount = lss.Count,
                EntitiesCount = lss.Count,
            };

            _worlds = new EcsWorlds(lss);
            _worlds
                .Create(cfg: worldCfg)
                .Register<Movable>();

            _worlds.Init();
            _systems = new EcsSystems();
            _systems
                .Add(new MoveSystem())
                .Init(_worlds)
                ;
        }

        public void Update()
        {
            _systems.Run(_worlds);
        }

        public void Destroy()
        {
            _systems?.Destroy(_worlds);
            _systems = null;
            _worlds?.Destroy();
            _worlds = null;
        }

        public string GetSummary()
        {
            return $"Framework: Ecslite.V1{Environment.NewLine}{_options.GetSummary()}";

        }
    }
}