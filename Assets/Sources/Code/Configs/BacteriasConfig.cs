using System.Collections.Generic;
using UnityEngine;

namespace Petri.Configs
{
    [CreateAssetMenu(fileName = "Bacterias", menuName = "Petri/Bacterias", order = 0)]
    public class BacteriasConfig : ScriptableObject
    {
        [field: SerializeField] public List<BacteriaData> Bacterias { get; private set; }
    }

    [System.Serializable]
    public class BacteriaData
    {
        public string DefaultName;
        public string DefaultDescription;
        public Sprite Icon;
        public float GrowthRate;
        public float BaseHealth;
    }
}