using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Petri.Configs;
using Petri.ECS;
using Petri.Infrostructure;
using Petri.Pools;
using Petri.Services;
using UnityEngine;

namespace Petri
{
    /// <summary>
    /// ECS initializer.
    /// </summary>
    public class Game : MonoBehaviour
    {
        public static LevelData CurrentLevelData { get; private set; }

        [SerializeField] private SceneData _sceneData;
        [SerializeField] private Configuration _configuration;

        /// <summary>
        /// Системы, которые выполняются в update.
        /// </summary>
        private EcsSystems _systems;

        /// <summary>
        /// Системы, которые выполняются в damage update.
        /// </summary>
        private EcsSystems _damageSystems;

        private CustomUpdate _damageUpdate;

        private void Start()
        {
            RegisterServices();

            var world = new EcsWorld();

            //todo: create shared data
            var dropsData = new SharedData
            {
                Damage = 11.5f,
                Radius = _sceneData.DropConfig.BaseScale * _sceneData.DropConfig.ScaleToRadius
            };
            
            _systems = new EcsSystems(world, dropsData);
            _damageSystems = new EcsSystems(world, dropsData);

            var dropPool = new DropPool(_sceneData.DropConfig, new GameObject("Drops").transform);
            CurrentLevelData = ServiceLocator.Get<ConfigService>().LevelConfig.Levels[0];
            
            _systems
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Add(new PipetteMovementSystem())
                .Add(new PipettePointerSystem(_sceneData))
                .Add(new CreateDropSystem())
                .Add(new DropFallSystem())
                .Add(new DropLifetimeSystem())
                .Add(new BacteriasSpawnSystem())
                .Add(new BacteriasMovementSystem())
                .Add(new BacteriasReproductiveSystem())
                .Add(new BacteriasDeathSystem())
                .Inject(_sceneData, _sceneData.DropConfig, dropPool)
                .Init();

            _damageSystems
                .Add(new BacteriasDamageSystem())
                .Inject(_sceneData.DropConfig)
                .Init();

            _sceneData.BacteriaRenderer.Initialize(world);

            _damageUpdate = new CustomUpdate(_configuration.DropsDpsTime, _damageSystems.Run);

            InitializeConstants();
        }

        private void InitializeConstants()
        {
            Shader.SetGlobalFloat("_PetriRadius", _sceneData.PetriRadius);
        }

        private static void RegisterServices()
        {
            ServiceLocator.Register(new DesktopInputService(), typeof(IInputService));
        }

        private void Update()
        {
            _systems.Run();
            _damageUpdate.Update();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems?.GetWorld()?.Destroy();
            _damageSystems?.Destroy();
            _systems = null;

            ServiceLocator.Clear();
        }
    }
}