using UnityEngine;

namespace Petri.Formula
{
    [System.Serializable]
    public class FormulaParameter
    {
        public string Name;
        public FormulaModifierApplyType ApplyType;
        public FormulaOperationType OperationType;
        public ReagentGroup ReagentGroup;
        public FormulaProperty Property;
        public Sprite Icon;
        public float Value;
    }
}