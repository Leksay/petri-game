using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
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
        [SerializeField] private SceneData _sceneData;
        [SerializeField] private Configuration _configuration;

        private EcsSystems _systems;

        private void Start()
        {
            RegisterServices();

            var world = new EcsWorld();
            _systems = new EcsSystems(world);

            var dropPool = new DropPool(_sceneData.DropConfig, new GameObject("Drops").transform);
            
            _systems
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Add(new PetriMovementSystem())
                .Add(new CreateDropSystem())
                .Add(new DropFallSystem())
                .Add(new DropLifetimeSystem())
                .Inject(_sceneData, _sceneData.DropConfig, dropPool)
                .Init();
        }

        private static void RegisterServices()
        {
            ServiceLocator.Register(new DesktopInputService(), typeof(IInputService));
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems?.GetWorld()?.Destroy();
            _systems = null;
            
            ServiceLocator.Clear();
        }
    }
}