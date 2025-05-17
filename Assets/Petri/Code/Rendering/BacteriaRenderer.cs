using System.Collections.Generic;
using Leopotam.EcsLite;
using Petri.Configs;
using Petri.ECS;
using Petri.Infrostructure;
using UnityEngine;

namespace Petri.Rendering
{
    public class BacteriaRenderer : MonoBehaviour
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        [SerializeField] private Material _bacteriaMaterial;
        [SerializeField] private Mesh _bacteriaMesh;
        [SerializeField] private float _bacteriaScale;
        [SerializeField] private Vector3 _rotationVector;
        [SerializeField] private Transform _petriPlane;

        private EcsPool<Bacteria> _bacteriaPool;
        private EcsFilter _filter;

        private Vector3 _scale;
        private bool _isInitialized;
        private Quaternion _rotation;

        private readonly Dictionary<BacteriaType, Matrix4x4[]> _matrices = new();
        private readonly Dictionary<BacteriaType, int> _indexes = new();
        private readonly Dictionary<BacteriaType, RenderParams> _renderParams = new();
        private readonly List<BacteriaType> _allTypes = new();

        public void Initialize(EcsWorld world)
        {
            _bacteriaPool = world.GetPool<Bacteria>();
            //todo: inject max count from config
            _filter = world.Filter<Bacteria>().End(1024);
            _rotation = Quaternion.LookRotation(_rotationVector);
            _scale = Vector3.one * _bacteriaScale;

            _isInitialized = true;

            foreach (var bacteriaData in ServiceLocator.Get<ConfigService>().BacteriasConfig.Bacterias)
            {
                _matrices.Add(bacteriaData.Type, new Matrix4x4[Game.CurrentLevelData.MaxBacteriasCount]);

                var propertyBlock = new MaterialPropertyBlock();
                propertyBlock.SetColor(BaseColor, bacteriaData.Color);

                _renderParams[bacteriaData.Type] = new RenderParams(_bacteriaMaterial)
                {
                    matProps = propertyBlock
                };

                _allTypes.Add(bacteriaData.Type);
            }
        }

        private void Update()
        {
            if(!_isInitialized) 
                return;

            ClearIndexes();

            foreach (var entity in _filter)
            {
                if (!_bacteriaPool.Has(entity))
                {
                    continue;
                }

                ref var bacteria = ref _bacteriaPool.Get(entity);
                bacteria.Position.y = _petriPlane.position.y;

                var type = bacteria.Type;
                var index = _indexes[type];

                _matrices[type][index] = Matrix4x4.TRS(bacteria.Position, _rotation, _scale);
                _indexes[type]++;
            }


            foreach (var bacteriaType in _allTypes)
            {
                if (_indexes[bacteriaType] == 0)
                {
                    continue;
                }

                Graphics.RenderMeshInstanced(
                    _renderParams[bacteriaType],
                    _bacteriaMesh,
                    0,
                    _matrices[bacteriaType],
                    _indexes[bacteriaType]
                );
            }
        }

        private void ClearIndexes()
        {
            foreach (var bacteriaType in _allTypes) 
                _indexes[bacteriaType] = 0;
        }
    }
}