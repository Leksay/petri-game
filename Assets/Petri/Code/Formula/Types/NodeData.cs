using System.Collections.Generic;

namespace Petri.Formula
{
    public class NodeData
    {
        public float Damage;
        public float Size;

        // public float DamageMultiplier;
        // public float SizeMultiplier;
        
        // /// <summary>
        // /// All multipliers should be equal to 1 by default.
        // /// </summary>
        public Dictionary<FormulaPropertyType, float> Multipliers = new()
        {
            [FormulaPropertyType.Damage] = 0f,
            [FormulaPropertyType.Size] = 0f,
        };
        //
        // public Queue<(FormulaPropertyType propertyType, float value)> MultipliersQueue = new();
        
        public override string ToString() => $"Damage: {Damage}, Size: {Size}";
        public NodeData Copy() => new(){Damage = this.Damage, Size = this.Size};

        public void Clear()
        {
            Damage = 0;
            Size = 0;
        }
    }

    public enum FormulaPropertyType
    {
        Damage,
        Size,
    }
}