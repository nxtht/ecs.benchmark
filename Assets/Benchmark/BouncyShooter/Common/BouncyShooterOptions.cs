using System;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Benchmark.BouncyShooter
{
    [CreateAssetMenu(fileName = "BouncyShooterOptionsObject", menuName = "Benchmark/Create BouncyShooter Options")]
    public class BouncyShooterOptions : BenchmarkOptions
    {
        public int ActorTypesCountMax => 30;

        public int ActorTypesCount = 1;
        public int ActorsCount = 100;
        public float BulletLifeTime;
        public float RateOfFire;
        public float BulletSpeed;
        public float ActorSpeed;
        public bool IsRendering;
        public GameObject ShooterPrefab;
        public GameObject BulletPrefab;
        public float ActorRadius = 0.5f;
        public float BulletRadius = 0.1f;

        public int EntitiesMax =>
            ActorTypesCount * ActorsCount +
            (int) Math.Ceiling(ActorTypesCount * ActorsCount * BulletLifeTime * RateOfFire);

        public int EntityCreationsPerSecond => (int) Math.Ceiling(ActorTypesCount * ActorsCount * RateOfFire);
        
        public override string GetSummary()
        {
            var sb = new StringBuilder();
            sb.Append("Benchmark: ").AppendLine(benchmarkName)
                .Append("Actor Types Count: ").AppendLine(ActorTypesCount.ToString())
                .Append("Actors Per Type Count: ").AppendLine(ActorsCount.ToString())
                .Append("BulletLifeTime: ").AppendLine(BulletLifeTime.ToString(CultureInfo.CurrentCulture))
                .Append("RateOfFire: ").AppendLine(RateOfFire.ToString(CultureInfo.CurrentCulture))
                .Append("EntitiesMax: ").AppendLine(EntitiesMax.ToString())
                .Append("Entity Creations per sec: ").AppendLine(EntityCreationsPerSecond.ToString())
                ;
            return sb.ToString();
        }
    }
}