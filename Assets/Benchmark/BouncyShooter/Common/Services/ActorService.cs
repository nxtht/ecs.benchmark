namespace Benchmark.BouncyShooter
{
    public class ActorService
    {
        private int _id;
        public int NextId => ++_id;
    }
}