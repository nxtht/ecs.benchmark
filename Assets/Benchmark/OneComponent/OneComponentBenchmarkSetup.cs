using Benchmark.OneComponent.Ecslite.V1;
using Benchmark.OneComponent.LeoEcs.PoolRef;
using Benchmark.OneComponent.LeoEcs.Resize;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Benchmark.OneComponent
{
    public class OneComponentBenchmarkSetup : MonoBehaviour
    {
        [SerializeField] private InputField componentsCountInputField;

        [SerializeField] private string sceneName;
        [SerializeField] private OneComponentOptions options;


        private void Start()
        {
            componentsCountInputField.text = options.ComponentsCount.ToString();

            componentsCountInputField.onValueChanged.AddListener(ComponentsCount_onValueChanged);
        }

        private void OnDestroy()
        {
            componentsCountInputField.onValueChanged.RemoveListener(ComponentsCount_onValueChanged);
        }

        public void LeoEcsResizeButton_onClick()
        {
            options.SetBenchmark(() => new LeoEcsResizeOneComponentBenchmark(options));
            StartBenchmark();
        }

        public void LeoEcsPoolRefButton_onClick()
        {
            options.SetBenchmark(() => new LeoEcsPoolRefOneComponentBenchmark(options));
            StartBenchmark();
        }

        public void EcsliteButton_onClick()
        {
            options.SetBenchmark(() => new EcsliteV1OneComponentBenchmark(options));
            StartBenchmark();
        }

        private void ComponentsCount_onValueChanged(string value)
        {
            if (int.TryParse(value, out var newVal))
            {
                options.ComponentsCount = newVal;
            }
        }

        private void StartBenchmark()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}