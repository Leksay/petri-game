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
        public FormulaPropertyType _propertyType;
        public Sprite Icon;
        public float Value;
    }
}