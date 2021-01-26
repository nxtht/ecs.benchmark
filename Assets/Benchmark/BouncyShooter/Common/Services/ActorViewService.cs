using System.Collections.Generic;
using UnityEngine;

namespace Benchmark.BouncyShooter
{
    public class ActorViewService
    {
        public Dictionary<int, Transform> Views;
        public Pool<Transform> Pool;
    }
}