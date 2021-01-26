using Leopotam.Ecs;
using UnityEngine;

namespace Benchmark.BouncyShooter.LeoEcs.Resize
{
    public sealed class InitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = default;
        private readonly SharedState _sharedState = default;
        private readonly ActorService _actors = default;
        
        public void Init()
        {
            for (int i = 0; i < _sharedState.ActorsCount; i++)
            {
                CreateActorTypesByTypeCount();
            }
        }

        private void CreateActorTypesByTypeCount()
        {
            switch (_sharedState.ActorTypesCount)
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
            var e = _world.NewEntity()
                    .Replace(new Actor() {Id = _actors.NextId})
                    .Replace(new Weapon() {FireTimeout = _sharedState.FireTimeout})
                    .Replace(new Position() {Value = Random.insideUnitCircle * 10})
                    .Replace(new Rotation() {Value = Utils.GetPointOnCircle()})
                    .Replace(new DesiredVelocity())
                ;
            e.Get<TActorType>();
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