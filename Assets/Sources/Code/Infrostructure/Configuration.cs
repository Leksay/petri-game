using UnityEngine;

namespace Petri.Infrostructure
{
    [CreateAssetMenu(menuName = "Petri/Create Configuration", fileName = "Configuration", order = 0)]
    public class Configuration : ScriptableObject
    {
        public int BacteriaMaxAmount;
    }
}