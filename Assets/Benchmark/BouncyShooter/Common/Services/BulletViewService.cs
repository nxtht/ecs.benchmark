using System.Collections.Generic;
using UnityEngine;

namespace Benchmark.BouncyShooter
{
    public class BulletViewService
    {
        public Dictionary<int, Transform> Views;
        public Pool<Transform> Pool;
    }
}