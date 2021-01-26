using Leopotam.EcsLite;
using UnityEngine;

namespace Benchmark.BouncyShooter.Ecslite.V1
{
    public sealed class InitSystem : IEcsInitSystem
    {
        private EcsWorld world = default;
        private Services services = default;
        public void Init(EcsWorlds worlds)
        {
            world = worlds.Get();
            services = worlds.Shared<Services>();

            // Instantiate actors
            for (var i = 0; i < services.SharedState.ActorsCount; i++)
            {
                CreateActorTypesByTypeCount(services.SharedState.ActorTypesCount);
            }

            world = null;
            services = null;
        }
        
        private void CreateActorTypesByTypeCount(int actorTypesCount)
        { 
            switch (actorTypesCount)
                {
                    case 2:
                        CreateActorType02();
                        break;
                    case 3:
                        CreateActorType03();
                        break;
                    case 4:
                        CreateActorType04();
                        break;
                    case 5:
                        CreateActorType05();
                        break;
                    case 6:
                        CreateActorType06();
                        break;
                    case 7:
                        CreateActorType07();
                        break;
                    case 8:
                        CreateActorType08();
                        break;
                    case 9:
                        CreateActorType09();
                        break;
                    case 10:
                        CreateActorType10();
                        break;
                    case 11:
                        CreateActorType11();
                        break;
                    case 12:
                        CreateActorType12();
                        break;
                    case 13:
                        CreateActorType13();
                        break;
                    case 14:
                        CreateActorType14();
                        break;
                    case 15:
                        CreateActorType15();
                        break;
                    case 16:
                        CreateActorType16();
                        break;
                    case 17:
                        CreateActorType17();
                        break;
                    case 18:
                        CreateActorType18();
                        break;
                    case 19:
                        CreateActorType19();
                        break;
                    case 20:
                        CreateActorType20();
                        break;
                    case 21:
                        CreateActorType21();
                        break;
                    case 22:
                        CreateActorType22();
                        break;
                    case 23:
                        CreateActorType23();
                        break;
                    case 24:
                        CreateActorType24();
                        break;
                    case 25:
                        CreateActorType25();
                        break;
                    case 26:
                        CreateActorType26();
                        break;
                    case 27:
                        CreateActorType27();
                        break;
                    case 28:
                        CreateActorType28();
                        break;
                    case 29:
                        CreateActorType29();
                        break;
                    case 30:
                        CreateActorType30();
                        break;

                    default:
                        CreateActorType01();
                        break;
                }
        }

        private void CreateActorType<TActorType>() where TActorType : struct
        {
            var entity = world.NewEntity();
            ref var actor = ref entity.Get<Actor>();
            actor.Id = services.ActorService.NextId;

            ref var weapon = ref entity.Get<Weapon>();
            weapon.FireTimeout = services.SharedState.FireTimeout;

            ref var pos = ref entity.Get<Position>();
            pos.Value = Random.insideUnitCircle * 10;

            ref var rotation = ref entity.Get<Rotation>();
            rotation.Value = Utils.GetPointOnCircle();

            ref var desVel = ref entity.Get<DesiredVelocity>();
            ref var actorType = ref entity.Get<TActorType>();
        }

        private void CreateActorType01()
        {
            CreateActorType<ActorType01>();
        }

        private void CreateActorType02()
        {
            CreateActorType<ActorType02>();
            CreateActorType01();
        }

        private void CreateActorType03()
        {
            CreateActorType<ActorType03>();
            CreateActorType02();
        }

        private void CreateActorType04()
        {
            CreateActorType<ActorType04>();
            CreateActorType03();
        }

        private void CreateActorType05()
        {
            CreateActorType<ActorType05>();
            CreateActorType04();
        }

        private void CreateActorType06()
        {
            CreateActorType<ActorType06>();
            CreateActorType05();
        }

        private void CreateActorType07()
        {
            CreateActorType<ActorType07>();
            CreateActorType06();
        }

        private void CreateActorType08()
        {
            CreateActorType<ActorType08>();
            CreateActorType07();
        }

        private void CreateActorType09()
        {
            CreateActorType<ActorType09>();
            CreateActorType08();
        }

        private void CreateActorType10()
        {
            CreateActorType<ActorType10>();
            CreateActorType09();
        }

        private void CreateActorType11()
        {
            CreateActorType<ActorType11>();
            CreateActorType10();
        }

        private void CreateActorType12()
        {
            CreateActorType<ActorType12>();
            CreateActorType11();
        }

        private void CreateActorType13()
        {
            CreateActorType<ActorType13>();
            CreateActorType12();
        }

        private void CreateActorType14()
        {
            CreateActorType<ActorType14>();
            CreateActorType13();
        }

        private void CreateActorType15()
        {
            CreateActorType<ActorType15>();
            CreateActorType14();
        }

        private void CreateActorType16()
        {
            CreateActorType<ActorType16>();
            CreateActorType15();
        }

        private void CreateActorType17()
        {
            CreateActorType<ActorType17>();
            CreateActorType16();
        }

        private void CreateActorType18()
        {
            CreateActorType<ActorType18>();
            CreateActorType17();
        }

        private void CreateActorType19()
        {
            CreateActorType<ActorType19>();
            CreateActorType18();
        }

        private void CreateActorType20()
        {
            CreateActorType<ActorType20>();
            CreateActorType19();
        }

        private void CreateActorType21()
        {
            CreateActorType<ActorType21>();
            CreateActorType20();
        }

        private void CreateActorType22()
        {
            CreateActorType<ActorType22>();
            CreateActorType21();
        }

        private void CreateActorType23()
        {
            CreateActorType<ActorType23>();
            CreateActorType22();
        }

        private void CreateActorType24()
        {
            CreateActorType<ActorType24>();
            CreateActorType23();
        }

        private void CreateActorType25()
        {
            CreateActorType<ActorType25>();
            CreateActorType24();
        }

        private void CreateActorType26()
        {
            CreateActorType<ActorType26>();
            CreateActorType25();
        }

        private void CreateActorType27()
        {
            CreateActorType<ActorType27>();
            CreateActorType26();
        }

        private void CreateActorType28()
        {
            CreateActorType<ActorType28>();
            CreateActorType27();
        }

        private void CreateActorType29()
        {
            CreateActorType<ActorType29>();
            CreateActorType28();
        }

        private void CreateActorType30()
        {
            CreateActorType<ActorType30>();
            CreateActorType29();
        }
    }
}