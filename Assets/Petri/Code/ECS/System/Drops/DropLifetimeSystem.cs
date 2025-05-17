using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Petri.Pools;
using UnityEngine;

namespace Petri.ECS
{
    public class DropLifetimeSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<Lifetime, Drop>> _lifetimeFilter = default;
        private EcsCustomInject<DropPool> _dropViewPool = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _lifetimeFilter.Value)
            {
                ref var lifetime = ref _lifetimeFilter.Pools.Inc1.Get(entity);
                lifetime.TimeLeft -= Time.deltaTime;
                if (lifetime.TimeLeft > 2f)
                    return;

                ref var drop = ref _lifetimeFilter.Pools.Inc2.Get(entity);
                drop.Opacity = Mathf.Min(1f, lifetime.TimeLeft);

                if (lifetime.TimeLeft <= 0.005f)
                {
                    _dropViewPool.Value.Return(drop.View);
                    systems.GetWorld().DelEntity(entity);
                }
            }
        }
    }
}