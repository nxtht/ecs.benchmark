using UnityEngine;
using UnityEngine.UI;

namespace Benchmark
{
    // TODO: Add a benchmarking tool more precise than Graphy
    public class BenchmarkRun : MonoBehaviour
    {
        [SerializeField] private Text summaryText;
        private IBenchmark _benchmark;

        private void Start()
        {
            _benchmark = BenchmarkOptions.GetBenchmark();
            summaryText.text = _benchmark.GetSummary();
            _benchmark.Start();
        }

        private void Update()
        {
            _benchmark.Update();
        }

        private void OnDestroy()
        {
            _benchmark.Destroy();
        }
    }
}