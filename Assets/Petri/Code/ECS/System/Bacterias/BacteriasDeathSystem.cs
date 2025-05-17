using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Petri.ECS
{
    public class BacteriasDeathSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BacteriaIsDead>> _deadBacterias = default;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            foreach (var entity in _deadBacterias.Value)
            {
                world.DelEntity(entity);
            }
        }
    }
}