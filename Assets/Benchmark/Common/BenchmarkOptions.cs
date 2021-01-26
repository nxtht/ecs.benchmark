using System;
using UnityEngine;

namespace Benchmark
{
    // TODO: Add auto-save feature
    public abstract class BenchmarkOptions : ScriptableObject
    {
        [SerializeField] protected string benchmarkName;
        private static Func<IBenchmark> _benchmarkFactory;
        
        public static IBenchmark GetBenchmark()
        {
            return _benchmarkFactory();
        }

        public void SetBenchmark(Func<IBenchmark> func)
        {
            _benchmarkFactory = func;
        }

        // TODO: Change to autogeneration with reflection
        public virtual string GetSummary()
        {
            return $"Benchmark: {benchmarkName}";
        }
    }
}