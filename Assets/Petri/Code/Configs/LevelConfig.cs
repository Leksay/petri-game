using System.Collections.Generic;
using UnityEngine;

namespace Petri.Configs
{
    [CreateAssetMenu(fileName = "LevelsConfig", menuName = "Petri/LevelsConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        public List<LevelData> Levels;
    }

    [System.Serializable]
    public class LevelData
    {
        public string LevelName;
        public List<LevelBacteriasSetup> Bacterias;
        public int MaxBacteriasCount;
    }

    [System.Serializable]
    public class LevelBacteriasSetup
    {
        public BacteriaType Type;
        public int Count;
    }
}