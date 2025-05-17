using System;
using System.Collections.Generic;
using System.Reflection;

namespace Petri.Formula
{
    public static class FormulaApplier
    {
        private static readonly Dictionary<FormulaProperty, FieldInfo> FormulaFields = new()
        {
            [FormulaProperty.Damage] = typeof(FormulaData).GetField(nameof(FormulaData.Damage)),
            [FormulaProperty.Size] = typeof(FormulaData).GetField(nameof(FormulaData.Size)),
        };

        private static readonly Dictionary<FormulaOperationType, Func<float, float, float>> Operations = new()
        {
            [FormulaOperationType.Add] = (current, value) => current + value,
            [FormulaOperationType.Subtract] = (current, value) => current - value,
            [FormulaOperationType.Multiply] = (current, value) => current * value,
            [FormulaOperationType.Divide] = (current, value) => current / value,
        };

        public static void ApplyParameter(this FormulaData formulaData, FormulaParameter parameter)
        {
            var field = FormulaFields[parameter.Property];
            var currentValue = (float)field.GetValue(formulaData);
            var resultValue = Operations[parameter.OperationType].Invoke(currentValue, parameter.Value);
            
            field.SetValue(formulaData, resultValue);
        }
    }
}