using UnityEngine;
using UnityEngine.UI;

namespace Benchmark
{
    public class Il2CppChecker : MonoBehaviour
    {
        public Text il2cppText;

        void Start()
        {
#if ENABLE_IL2CPP
        il2cppText.text = "IL2CPP Enabled";
#else
            il2cppText.text = "IL2CPP Disabled";
#endif
        }
    }
}