using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Petri.ECS
{
    public class BacteriasReproductiveSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Bacteria, ReproductiveTimer>> _filter;
        private EcsPoolInject<WantToReproduce> _wantToReproduce;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var reproductive = ref _filter.Pools.Inc2.Get(entity);
                reproductive.TimeToReproduce -= Time.deltaTime;

                if (reproductive.TimeToReproduce <= 0 && _filter.Pools.Inc1.GetRawDenseItemsCount() < Game.CurrentLevelData.MaxBacteriasCount)
                {
                    reproductive.TimeToReproduce = Random.value + 1f / reproductive.ReproductiveRate;
                    _wantToReproduce.Value.Add(entity);
                }
            }
        }
    }
}