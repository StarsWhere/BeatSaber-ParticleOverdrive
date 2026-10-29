using ParticleOverdrive.Misc;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

namespace ParticleOverdrive.Controllers
{
    public class DustParticleController : IInitializable
    {
        [Inject] private readonly ParticleConfig _config;
        [Inject] private readonly ICoroutineStarter _coroutineStarter;

        [InjectOptional] private readonly EnvironmentSceneSetupData _environmentData;

        private ParticleSystem _dustPS = null;
        private string _dustParticlesName;

        public void Initialize()
        {
            _dustParticlesName = _environmentData switch
            {
                null => "DustPS",
                _ => _environmentData.environmentSerializedName switch
                {
                    "BritneyEnvironment" => "DustBritney",
                    _ => "DustPS"
                }
            };

            // only has to be done in a coroutine because of MultiPlayer being like a frame too early...
            _coroutineStarter.StartCoroutine(InitializeCoroutine());
        }

        private IEnumerator InitializeCoroutine()
        {
            yield return new WaitUntil(() => FindDustParticleSystem() != null);
            SetDustParticlesActive(_config.DustParticles);
        }

        private ParticleSystem FindDustParticleSystem() =>
            _dustPS = Object.FindObjectsOfType<ParticleSystem>().FirstOrDefault(p => p.name == _dustParticlesName);

        private void SetDustParticlesActive(bool active)
        {
            if (_dustPS) _dustPS.gameObject.SetActive(active);
        }
    }
}
