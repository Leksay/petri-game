using UnityEngine;

namespace Petri.Pools
{
    public class GameObjectPool : Pool<GameObject>
    {
        public GameObjectPool(GameObject prefab,
            int preloadCount
        ) : base(() =>Preload(prefab), GetGameObject, ReturnGameObject, preloadCount)
        { }

        private static GameObject Preload(GameObject prefab) => GameObject.Instantiate(prefab);
        private static void GetGameObject(GameObject gameObject) => gameObject.SetActive(true);
        private static void ReturnGameObject(GameObject gameObject) => gameObject.SetActive(false);
    }
}