namespace Benchmark
{
    // TODO: Replace with abstract class?
    public interface IBenchmark
    {
        void Start();
        void Update();
        void Destroy();
        string GetSummary();
    }
}