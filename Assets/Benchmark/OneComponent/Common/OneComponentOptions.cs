using System.Globalization;
using System.Text;
using UnityEngine;

namespace Benchmark.OneComponent
{
    [CreateAssetMenu(fileName = "OneComponentOptionsObject", menuName = "Benchmark/Create OneComponent Options")]
    public class OneComponentOptions : BenchmarkOptions
    {
        public int ComponentsCount;

        public override string GetSummary()
        {
            var sb = new StringBuilder();
            sb.Append("Benchmark: ").AppendLine(benchmarkName)
                .Append("ComponentsCount: ").AppendLine(ComponentsCount.ToString());
                
            return sb.ToString();
        }
    }
}