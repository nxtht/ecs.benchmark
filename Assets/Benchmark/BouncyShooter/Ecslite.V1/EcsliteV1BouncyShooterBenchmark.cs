using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Benchmark.BouncyShooter.Ecslite.V1
{
    public class EcsliteV1BouncyShooterBenchmark : IBenchmark
    {
        private readonly BouncyShooterOptions _options;
        private EcsWorlds _worlds;
        private EcsSystems _systems;
        private EcsWorld _world;

        public EcsliteV1BouncyShooterBenchmark(BenchmarkOptions options)
        {
            _options = (BouncyShooterOptions) options;
        }

        public void Start()
        {
            var services = CreateAllSharedState(_options);
            var worldCfg = new EcsWorldConfig()
            {
                ComponentsCount = _options.EntitiesMax,
                EntitiesCount = _options.EntitiesMax,
            };

            _worlds = new EcsWorlds(services);
            _world = _worlds.Create(cfg: worldCfg);
            _world
                .Register<Actor>()
                .Register<Bullet>()
                .Register<Deleted>()
                .Register<DesiredVelocity>()
                .Register<Position>()
                .Register<Rotation>()
                .Register<Weapon>()
                ;

            _systems = new EcsSystems();
            _systems
                .Add(new InitSystem())
                .Add(new TimeSystem())
                .Add(new ShootSystem())
                .Add(new BulletSystem())
                .Add(new MoveSystem())
                ;

            AddActorTypeSystemsByTypeCount();

            if (_options.IsRendering)
            {
                _systems
                    .Add(new ActorViewUpdateSystem())
                    .Add(new BulletViewDeleteSystem())
                    .Add(new BulletViewUpdateSystem())
                    ;
            }

            _systems.Add(new BulletDeleteSystem());

            _worlds.Init();
            _systems.Init(_worlds);
            _world = null;
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

        private Services CreateAllSharedState(BouncyShooterOptions options)
        {
            var ss = new SharedState
            {
                ActorTypesCount = options.ActorTypesCount,
                BulletSpeed = options.BulletSpeed,
                FireTimeout = 1f / options.RateOfFire,
                ActorsCount = options.ActorsCount,
                ActorSpeed = options.ActorSpeed,
                BulletLifeTime = options.BulletLifeTime,
                ActorRadius = options.ActorRadius,
                BulletRadius = options.BulletRadius
            };

            var allShared = new Services()
            {
                TimeService = new TimeService(),
                ActorService = new ActorService(),
                BulletService = new BulletService(),
                SharedState = ss
            };

            if (options.IsRendering)
            {
                var bullet = options.BulletPrefab;
                var bulletViewService = new BulletViewService()
                {
                    Pool = new Pool<Transform>(
                        () =>
                        {
                            var tr = Object.Instantiate(bullet).transform;
                            tr.gameObject.SetActive(false);
                            return tr;
                        }, 2),
                    Views = new Dictionary<int, Transform>()
                };
                var actor = options.ShooterPrefab;

                var actorViewService = new ActorViewService()
                {
                    Pool = new Pool<Transform>(
                        () =>
                        {
                            var tr = Object.Instantiate(actor).transform;
                            tr.gameObject.SetActive(false);
                            return tr;
                        }, 2),
                    Views = new Dictionary<int, Transform>()
                };

                allShared.ActorViewService = actorViewService;
                allShared.BulletViewService = bulletViewService;
            }

            return allShared;
        }

        private void AddActorTypeSystemsByTypeCount()
        {
            switch (_options.ActorTypesCount)
            {
                case 2:
                    AddActorTypeSystems02();
                    break;
                case 3:
                    AddActorTypeSystems03();
                    break;
                case 4:
                    AddActorTypeSystems04();
                    break;
                case 5:
                    AddActorTypeSystems05();
                    break;
                case 6:
                    AddActorTypeSystems06();
                    break;
                case 7:
                    AddActorTypeSystems07();
                    break;
                case 8:
                    AddActorTypeSystems08();
                    break;
                case 9:
                    AddActorTypeSystems09();
                    break;
                case 10:
                    AddActorTypeSystems10();
                    break;
                case 11:
                    AddActorTypeSystems11();
                    break;
                case 12:
                    AddActorTypeSystems12();
                    break;
                case 13:
                    AddActorTypeSystems13();
                    break;
                case 14:
                    AddActorTypeSystems14();
                    break;
                case 15:
                    AddActorTypeSystems15();
                    break;
                case 16:
                    AddActorTypeSystems16();
                    break;
                case 17:
                    AddActorTypeSystems17();
                    break;
                case 18:
                    AddActorTypeSystems18();
                    break;
                case 19:
                    AddActorTypeSystems19();
                    break;
                case 20:
                    AddActorTypeSystems20();
                    break;
                case 21:
                    AddActorTypeSystems21();
                    break;
                case 22:
                    AddActorTypeSystems22();
                    break;
                case 23:
                    AddActorTypeSystems23();
                    break;
                case 24:
                    AddActorTypeSystems24();
                    break;
                case 25:
                    AddActorTypeSystems25();
                    break;
                case 26:
                    AddActorTypeSystems26();
                    break;
                case 27:
                    AddActorTypeSystems27();
                    break;
                case 28:
                    AddActorTypeSystems28();
                    break;
                case 29:
                    AddActorTypeSystems29();
                    break;
                case 30:
                    AddActorTypeSystems30();
                    break;

                default:
                    AddActorTypeSystems01();
                    break;
            }
        }

        private void AddActorTypeSystems<TActorType>() where TActorType : struct
        {
            _world.Register<TActorType>();
            _systems.Add(new ActorByTypeUpdateSystem<TActorType>());
        }

        private void AddActorTypeSystems01()
        {
            AddActorTypeSystems<ActorType01>();
        }

        private void AddActorTypeSystems02()
        {
            AddActorTypeSystems<ActorType02>();
            AddActorTypeSystems01();
        }

        private void AddActorTypeSystems03()
        {
            AddActorTypeSystems<ActorType03>();
            AddActorTypeSystems02();
        }

        private void AddActorTypeSystems04()
        {
            AddActorTypeSystems<ActorType04>();
            AddActorTypeSystems03();
        }

        private void AddActorTypeSystems05()
        {
            AddActorTypeSystems<ActorType05>();
            AddActorTypeSystems04();
        }

        private void AddActorTypeSystems06()
        {
            AddActorTypeSystems<ActorType06>();
            AddActorTypeSystems05();
        }

        private void AddActorTypeSystems07()
        {
            AddActorTypeSystems<ActorType07>();
            AddActorTypeSystems06();
        }

        private void AddActorTypeSystems08()
        {
            AddActorTypeSystems<ActorType08>();
            AddActorTypeSystems07();
        }

        private void AddActorTypeSystems09()
        {
            AddActorTypeSystems<ActorType09>();
            AddActorTypeSystems08();
        }

        private void AddActorTypeSystems10()
        {
            AddActorTypeSystems<ActorType10>();
            AddActorTypeSystems09();
        }

        private void AddActorTypeSystems11()
        {
            AddActorTypeSystems<ActorType11>();
            AddActorTypeSystems10();
        }

        private void AddActorTypeSystems12()
        {
            AddActorTypeSystems<ActorType12>();
            AddActorTypeSystems11();
        }

        private void AddActorTypeSystems13()
        {
            AddActorTypeSystems<ActorType13>();
            AddActorTypeSystems12();
        }

        private void AddActorTypeSystems14()
        {
            AddActorTypeSystems<ActorType14>();
            AddActorTypeSystems13();
        }

        private void AddActorTypeSystems15()
        {
            AddActorTypeSystems<ActorType15>();
            AddActorTypeSystems14();
        }

        private void AddActorTypeSystems16()
        {
            AddActorTypeSystems<ActorType16>();
            AddActorTypeSystems15();
        }

        private void AddActorTypeSystems17()
        {
            AddActorTypeSystems<ActorType17>();
            AddActorTypeSystems16();
        }

        private void AddActorTypeSystems18()
        {
            AddActorTypeSystems<ActorType18>();
            AddActorTypeSystems17();
        }

        private void AddActorTypeSystems19()
        {
            AddActorTypeSystems<ActorType19>();
            AddActorTypeSystems18();
        }

        private void AddActorTypeSystems20()
        {
            AddActorTypeSystems<ActorType20>();
            AddActorTypeSystems19();
        }

        private void AddActorTypeSystems21()
        {
            AddActorTypeSystems<ActorType21>();
            AddActorTypeSystems20();
        }

        private void AddActorTypeSystems22()
        {
            AddActorTypeSystems<ActorType22>();
            AddActorTypeSystems21();
        }

        private void AddActorTypeSystems23()
        {
            AddActorTypeSystems<ActorType23>();
            AddActorTypeSystems22();
        }

        private void AddActorTypeSystems24()
        {
            AddActorTypeSystems<ActorType24>();
            AddActorTypeSystems23();
        }

        private void AddActorTypeSystems25()
        {
            AddActorTypeSystems<ActorType25>();
            AddActorTypeSystems24();
        }

        private void AddActorTypeSystems26()
        {
            AddActorTypeSystems<ActorType26>();
            AddActorTypeSystems25();
        }

        private void AddActorTypeSystems27()
        {
            AddActorTypeSystems<ActorType27>();
            AddActorTypeSystems26();
        }

        private void AddActorTypeSystems28()
        {
            AddActorTypeSystems<ActorType28>();
            AddActorTypeSystems27();
        }

        private void AddActorTypeSystems29()
        {
            AddActorTypeSystems<ActorType29>();
            AddActorTypeSystems28();
        }

        private void AddActorTypeSystems30()
        {
            AddActorTypeSystems<ActorType30>();
            AddActorTypeSystems29();
        }
    }
}