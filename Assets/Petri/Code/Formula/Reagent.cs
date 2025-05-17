using Petri.Configs;
using UnityEngine;

namespace Petri.Formula
{
    [System.Serializable]
    public class Reagent
    {
        [field: SerializeField] public bool IsAvailable { get; set; }
        [field: SerializeField] public FormulaModifierConfig Modifier { get; set; }
        [field: SerializeField] public ConnectionTypes ConnectionType { get; set; }
    }
}