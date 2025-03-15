using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Petri.Configs;

namespace Petri.ECS.Bacterias
{
    public class BacteriasInitSystem : IEcsRunSystem
    {
        private readonly EcsCustomInject<BacteriasConfig> _config;
        private readonly EcsFilterInject<Inc<Bacteria, >>
        
        public void Run(IEcsSystems systems)
        {
            
        }
    }
}