namespace Benchmark.BouncyShooter
{
    public class BulletService
    {
        private int _id;
        public int NextId => ++_id;
    }
}