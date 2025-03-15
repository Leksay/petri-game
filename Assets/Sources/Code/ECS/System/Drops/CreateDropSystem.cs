using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Petri.Infrostructure;
using Petri.Pools;
using Petri.Services;
using UnityEngine;

namespace Petri.ECS
{
    public class CreateDropSystem : IEcsRunSystem
    {
        private static readonly int DropParamId = Animator.StringToHash("Drop");
        private readonly IInputService _inputService = ServiceLocator.Get<IInputService>();
        private readonly EcsCustomInject<SceneData> _sceneData = default;
        private readonly EcsPoolInject<Drop> _dropPool = default;
        private readonly EcsPoolInject<Lifetime> _lifetimePool = default;
        private readonly EcsCustomInject<DropPool> _dropViewPool = default;

        public void Run(IEcsSystems systems)
        {
            if(!_inputService.MouseDown)
                return;

            var dropEntity = systems.GetWorld().NewEntity();

            var dropView = _dropViewPool.Value.Get();
            dropView.Transform.position = _sceneData.Value.PipetteView.Pointer.position;

            ref var drop = ref _dropPool.Value.Add(dropEntity);
            drop.View = dropView;
            drop.Position = dropView.Transform.position;
            drop.CurrentSpread = drop.View.Transform.localScale.x;
            drop.TargetSpread = Random.Range(_sceneData.Value.DropConfig.MinMaxRadius.x, _sceneData.Value.DropConfig.MinMaxRadius.y);

            dropView.Transform.localScale = Vector3.one;

            ref var lifetime = ref _lifetimePool.Value.Add(dropEntity);
            lifetime.TimeLeft = Random.Range(_sceneData.Value.DropConfig.MinMaxLifetime.x, _sceneData.Value.DropConfig.MinMaxLifetime.y);
            
            _sceneData.Value.PipetteView.Animator.SetTrigger(DropParamId);
        }
    }
}