using System;
using System.Collections.Generic;
using System.Reflection;

namespace Petri.Formula
{
    public static class FormulaApplier
    {
        private static readonly Dictionary<FormulaPropertyType, FieldInfo> FormulaFields = new()
        {
            [FormulaPropertyType.Damage] = typeof(FormulaData).GetField(nameof(FormulaData.Damage)),
            [FormulaPropertyType.Size] = typeof(FormulaData).GetField(nameof(FormulaData.Size)),
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
            var field = FormulaFields[parameter._propertyType];
            var currentValue = (float)field.GetValue(formulaData);
            var resultValue = Operations[parameter.OperationType].Invoke(currentValue, parameter.Value);
            
            field.SetValue(formulaData, resultValue);
        }

        public static void AddAll(this FormulaData baseData, FormulaData additionalData)
        {
            foreach (var (_, fieldInfo) in FormulaFields)
            {
                var baseValue = (float)fieldInfo.GetValue(baseData);
                var additionalValue = (float)fieldInfo.GetValue(additionalData);

                fieldInfo.SetValue(baseData, baseValue + additionalValue);
            }
        }
    }
}