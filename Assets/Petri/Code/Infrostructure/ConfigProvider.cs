using Petri.Configs;
using Petri.Services;
using UnityEngine;

namespace Petri.Infrostructure
{
    public class ConfigProvider : MonoBehaviour
    {
        [SerializeField] private DropConfig _dropConfig;
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private BacteriasConfig _bacteriasConfig;

        private void Awake()
        {
            ServiceLocator.Register(new ConfigService(_dropConfig, _levelConfig, _bacteriasConfig));
        }
    }

    public class ConfigService : IService
    {
        public DropConfig DropConfig { get; }
        public LevelConfig LevelConfig { get; }
        public BacteriasConfig BacteriasConfig { get; }

        public ConfigService(DropConfig dropConfig, LevelConfig levelConfig, BacteriasConfig bacteriasConfig)
        {
            DropConfig = dropConfig;
            LevelConfig = levelConfig;
            BacteriasConfig = bacteriasConfig;
        }
    }
}