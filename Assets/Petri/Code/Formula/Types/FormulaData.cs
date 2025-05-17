using System.Collections.Generic;
using Petri.Configs;

namespace Petri.Formula
{
    public class FormulaData
    {
        public float Damage;
        public float Size;
        
        private readonly List<FormulaModifierConfig> _modifiers = new();

        public void AddModifier(FormulaModifierConfig modifier)
        {
            _modifiers.Add(modifier);
        }
    }

    public enum FormulaProperty
    {
        Damage,
        Size,
    }
}