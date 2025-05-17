using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Petri.Infrostructure;
using UnityEngine;

namespace Petri.ECS
{
    public class DropFallSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Drop>> _dropFilter;
        private readonly EcsCustomInject<SceneData> _sceneData;
        private float _fallSpeed;
        private float _petriHeight;
        private float _spreadSpeed;

        public void Init(IEcsSystems systems)
        {
            _fallSpeed = _sceneData.Value.DropConfig.FallSpeed;
            _spreadSpeed = _sceneData.Value.DropConfig.AnimationSpeed;
            _petriHeight = _sceneData.Value.PetriView.HeightPlaneTransform.position.y;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _dropFilter.Value)
            {
                ref var drop = ref _dropFilter.Pools.Inc1.Get(entity);
                if (!drop.ReachPetri)
                {
                    drop.Position -= Vector3.up * (_fallSpeed * Time.deltaTime);
                    drop.View.Transform.position = drop.Position;

                    if (drop.Position.y <= _petriHeight)
                    {
                        drop.View.Transform.position = new Vector3(drop.Position.x, _petriHeight, drop.View.Transform.position.z);
                        drop.ReachPetri = true;
                        drop.Spreading = true;
                    }
                }

                if (drop.Spreading)
                {
                    if (drop.CurrentSpread <= drop.TargetSpread)
                    {
                        drop.CurrentSpread += _spreadSpeed * Time.deltaTime;
                        drop.View.Transform.localScale = Vector3.one * drop.CurrentSpread;
                    }

                    if(drop.BlendShapeValue < 100)
                        drop.BlendShapeValue += _spreadSpeed * Time.deltaTime * 25;
                    
                    drop.Spreading = drop.BlendShapeValue < 100 || drop.CurrentSpread <= drop.TargetSpread;
                }
            }
        }
    }
}