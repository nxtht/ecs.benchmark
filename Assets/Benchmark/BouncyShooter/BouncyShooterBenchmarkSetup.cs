using System.Text;
using Benchmark.BouncyShooter.Ecslite.V1;
using Benchmark.BouncyShooter.LeoEcs.Resize;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Benchmark.BouncyShooter
{
    // TODO: Replace manual configuration of the buttons with autogeneration by reflection
    // TODO: Add autogeneration of UI
    public class BouncyShooterBenchmarkSetup : MonoBehaviour
    {
        [SerializeField] private InputField actorTypesCountInputField;
        [SerializeField] private InputField actorsCountInputField;
        [SerializeField] private InputField bulletLifeTimeInputField;
        [SerializeField] private InputField rateOfFireInputField;
        [SerializeField] private InputField bulletSpeedInputField;
        [SerializeField] private InputField actorSpeedInputField;
        [SerializeField] private InputField actorRadiusInputField;
        [SerializeField] private InputField bulletRadiusInputField;
        [SerializeField] private Toggle isRenderingToggle;
        [SerializeField] private Text statsText;


        [SerializeField] private string sceneName;
        [SerializeField] private BouncyShooterOptions options;

        private void Start()
        {
            actorTypesCountInputField.text = options.ActorTypesCount.ToString();
            actorsCountInputField.text = options.ActorsCount.ToString();
            bulletLifeTimeInputField.text = options.BulletLifeTime.ToString();
            rateOfFireInputField.text = options.RateOfFire.ToString();
            bulletSpeedInputField.text = options.BulletSpeed.ToString();
            actorSpeedInputField.text = options.ActorSpeed.ToString();
            actorRadiusInputField.text = options.ActorRadius.ToString();
            bulletRadiusInputField.text = options.BulletRadius.ToString();
            isRenderingToggle.isOn = options.IsRendering;

            actorTypesCountInputField.onValueChanged.AddListener(ActorTypesCount_onValueChanged);
            actorsCountInputField.onValueChanged.AddListener(ActorsCount_onValueChanged);
            bulletLifeTimeInputField.onValueChanged.AddListener(BulletLifetime_onValueChanged);
            rateOfFireInputField.onValueChanged.AddListener(RateOfFire_onValueChanged);
            bulletSpeedInputField.onValueChanged.AddListener(BulletSpeed_onValueChanged);
            actorSpeedInputField.onValueChanged.AddListener(ActorSpeed_onValueChanged);
            actorRadiusInputField.onValueChanged.AddListener(ActorRadius_onValueChanged);
            bulletRadiusInputField.onValueChanged.AddListener(BulletRadius_onValueChanged);
            isRenderingToggle.onValueChanged.AddListener(IsRendering_onValueChanged);
            RefreshStats();
        }

        private void OnDestroy()
        {
            actorTypesCountInputField.onValueChanged.RemoveListener(ActorTypesCount_onValueChanged);
            actorTypesCountInputField.onValueChanged.RemoveListener(ActorsCount_onValueChanged);
            bulletLifeTimeInputField.onValueChanged.RemoveListener(BulletLifetime_onValueChanged);
            rateOfFireInputField.onValueChanged.RemoveListener(RateOfFire_onValueChanged);
            bulletSpeedInputField.onValueChanged.RemoveListener(BulletSpeed_onValueChanged);
            actorSpeedInputField.onValueChanged.RemoveListener(ActorSpeed_onValueChanged);
            actorRadiusInputField.onValueChanged.RemoveListener(ActorRadius_onValueChanged);
            bulletRadiusInputField.onValueChanged.RemoveListener(BulletRadius_onValueChanged);
            isRenderingToggle.onValueChanged.RemoveListener(IsRendering_onValueChanged);
        }

        public void LeoEcsButton_onClick()
        {
            options.SetBenchmark(() => new LeoEcsResizeBouncyShooterBenchmark(options));
            StartBenchmark();
        }

        public void EcsliteButton_onClick()
        {
            options.SetBenchmark(() => new EcsliteV1BouncyShooterBenchmark(options));
            StartBenchmark();
        }

        private void ActorTypesCount_onValueChanged(string value)
        {
            if (int.TryParse(value, out var newVal))
            {
                if (newVal > options.ActorTypesCountMax)
                {
                    newVal = options.ActorTypesCountMax;
                    actorTypesCountInputField.text = newVal.ToString();
                }

                options.ActorTypesCount = newVal;
                RefreshStats();
            }
        }
        
        private void ActorsCount_onValueChanged(string value)
        {
            if (int.TryParse(value, out var newVal))
            {
                options.ActorsCount = newVal;
                RefreshStats();
            }
        }

        private void BulletLifetime_onValueChanged(string value)
        {
            if (float.TryParse(value, out var newVal))
            {
                options.BulletLifeTime = newVal;
                RefreshStats();
            }
        }

        private void RateOfFire_onValueChanged(string value)
        {
            if (int.TryParse(value, out var newVal))
            {
                options.RateOfFire = newVal;
                RefreshStats();
            }
        }

        private void BulletSpeed_onValueChanged(string value)
        {
            if (float.TryParse(value, out var newVal))
            {
                options.BulletSpeed = newVal;
                RefreshStats();
            }
        }

        private void ActorSpeed_onValueChanged(string value)
        {
            if (float.TryParse(value, out var newVal))
            {
                options.ActorSpeed = newVal;
                RefreshStats();
            }
        }

        private void ActorRadius_onValueChanged(string value)
        {
            if (float.TryParse(value, out var newVal))
            {
                options.ActorRadius = newVal;
                RefreshStats();
            }
        }

        private void BulletRadius_onValueChanged(string value)
        {
            if (float.TryParse(value, out var newVal))
            {
                options.BulletRadius = newVal;
                RefreshStats();
            }
        }

        private void IsRendering_onValueChanged(bool value)
        {
            options.IsRendering = value;
        }


        private void StartBenchmark()
        {
            SceneManager.LoadScene(sceneName);
        }

        private void RefreshStats()
        {
            var sb = new StringBuilder();
            sb.Append($"Entities max = {options.EntitiesMax}; ");
            sb.Append($"Entity creations per second = {options.EntityCreationsPerSecond}; ");

            statsText.text = sb.ToString();
        }
    }
}