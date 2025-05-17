using Petri.Formula;
using UnityEngine;

namespace Petri.Configs
{
    [CreateAssetMenu(fileName = "FormulaModifierConfig", menuName = "Petri/Formula/FormulaModifierConfig")]
    public class FormulaModifierConfig : ScriptableObject
    {
        public FormulaParameter Parameter;
    }
}