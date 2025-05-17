using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Petri.DamageCalculations;
using Petri.Infrostructure;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Petri.ECS
{
    public class BacteriasDamageSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<Bacteria>> _bacteriasFilter;
        private EcsFilterInject<Inc<Drop>> _dropFilter;

        private EcsPoolInject<BacteriaIsDead> _bacteriaIsDead;

        private float _sqrRadius;
        private float _damage;

        public void Init(IEcsSystems systems)
        {
            _damage = systems.GetShared<SharedData>().Damage;
            _sqrRadius = systems.GetShared<SharedData>().Radius;
            _sqrRadius *= _sqrRadius;
        }

        public void Run(IEcsSystems systems)
        {
            var bacteriasCount = _bacteriasFilter.Value.GetEntitiesCount();
            var dropsCount = _dropFilter.Value.GetEntitiesCount();

            if (dropsCount == 0)
            {
                return;
            }
            var bacteriasData = new NativeArray<BacteriasData>(bacteriasCount, Allocator.TempJob);
            var dropsData = new NativeArray<float3>(dropsCount, Allocator.TempJob);

            var index = 0;
            foreach (var entity in _bacteriasFilter.Value)
            {
                bacteriasData[index] = new BacteriasData(
                    entity,
                    _bacteriasFilter.Pools.Inc1.Get(entity).Position
                );

                index++;
            }

            index = 0;
            foreach (var entity in _dropFilter.Value)
            {
                dropsData[index] = _dropFilter.Pools.Inc1.Get(entity).Position;
                index++;
            }

            var results = new NativeArray<BacteriaDamageResult>(bacteriasCount, Allocator.TempJob);
            var damageJob = new DamageCheckJob
            {
                Bacterias = bacteriasData,
                Drops = dropsData,
                Results = results,
                SquaredRadius = _sqrRadius
            };

            var handle = damageJob.Schedule(bacteriasCount, Mathf.Max(32, bacteriasCount));
            handle.Complete();
            

            foreach (var result in results)
            {
                if (result.IntersectionCount <= 0)
                {
                    continue;
                }

                ref var bacteria = ref _bacteriasFilter.Pools.Inc1.Get(result.BacteriaEntity);
                bacteria.Hp -= _damage * result.IntersectionCount;

                if (bacteria.Hp <= 0)
                {
                    _bacteriaIsDead.Value.Add(result.BacteriaEntity);
                }
            }

            bacteriasData.Dispose();
            dropsData.Dispose();
            results.Dispose();
        }
    }
}