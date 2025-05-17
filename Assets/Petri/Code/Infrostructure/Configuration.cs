using UnityEngine;

namespace Petri.Infrostructure
{
    [CreateAssetMenu(menuName = "Petri/Create Configuration", fileName = "Configuration", order = 0)]
    public class Configuration : ScriptableObject
    {
        /// <summary>
        /// Время между нанесением урона каплей.
        /// </summary>
        public float DropsDpsTime = 4f;
    }
}