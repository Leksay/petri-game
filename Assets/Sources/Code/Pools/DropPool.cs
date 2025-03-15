using Petri.Configs;
using Petri.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Petri.Pools
{
    public class DropPool : Pool<DropView>
    {
        private static float _dropBaseScale;

        private DropConfig _config;

        public DropPool(DropConfig config, Transform parent)
            : base(() =>
            {
                var view = Object.Instantiate(config.Prefab);
                view.transform.SetParent(parent, false);

                return view;
            }, GetDrop, ReturnDrop, config.PreloadCount)
        {
            _config = config;
            _dropBaseScale = _config.BaseScale;
        }

        private static void GetDrop(DropView view) => view.gameObject.SetActive(true);
        private static void ReturnDrop(DropView view)
        {
            view.gameObject.SetActive(false);
            view.SetOpacity(1);
            view.Transform.localScale = Vector3.one * _dropBaseScale;
            view.SkinnedMeshRenderer.SetBlendShapeWeight(1, 0);
        }
    }
}