using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Petri.Configs;
using Petri.Helpers;
using Petri.Infrostructure;
using UnityEngine;

namespace Petri.ECS
{
    public class BacteriasSpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsPoolInject<Bacteria> _bacteriasPool = default;
        private readonly EcsPoolInject<Movement> _movementPool = default;

        private readonly EcsPoolInject<ReproductiveTimer> _reproductivePool = default;
        private readonly EcsFilterInject<Inc<WantToReproduce>> _wantToReproduceFilter = default;

        private EcsWorld _world;
        private LevelData _levelConfig;
        private BacteriasConfig _bacteriasConfig;

        public void Init(IEcsSystems systems)
        {
            _levelConfig = Game.CurrentLevelData;
            _bacteriasConfig = ServiceLocator.Get<ConfigService>().BacteriasConfig;
            
            _world = systems.GetWorld();

            foreach (var bacteriasSetup in _levelConfig.Bacterias)
                for (var i = 0; i < bacteriasSetup.Count; i++) 
                    CreateBacteriaEntity(type: bacteriasSetup.Type);
        }

        private void CreateBacteriaEntity(Vector3 position = default, BacteriaType type = BacteriaType.Red)
        {
            var config = ServiceLocator.Get<ConfigService>().BacteriasConfig.Bacterias.First(x => x.Type == type);
            var entity = _world.NewEntity();

            ref var bacteria = ref _bacteriasPool.Value.Add(entity);
            ref var movement = ref _movementPool.Value.Add(entity);
            ref var reproductive = ref _reproductivePool.Value.Add(entity);

            bacteria.Set(config);

            bacteria.Position = position;
            movement.Speed = config.Speed * (0.5f + Random.value);
            movement.Direction = Vector3Utils.RandomDirectionXZ();

            reproductive.ReproductiveRate = config.GrowthRate;
            reproductive.TimeToReproduce = 1f / config.GrowthRate;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _wantToReproduceFilter.Value)
            {
                _wantToReproduceFilter.Pools.Inc1.Del(entity);
                ref var bacteria = ref _bacteriasPool.Value.Get(entity);
                
                CreateBacteriaEntity(bacteria.Position, bacteria.Type);
            }
        }
    }
}