using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Petri.Formula
{
    public static class FormulaApplier
    {
        private static readonly Dictionary<FormulaPropertyType, FieldInfo> FormulaFields = new()
        {
            [FormulaPropertyType.Damage] = typeof(NodeData).GetField(nameof(NodeData.Damage)),
            [FormulaPropertyType.Size] = typeof(NodeData).GetField(nameof(NodeData.Size)),
        };

        private static readonly Dictionary<FormulaOperationType, Func<float, float, float>> Operations = new()
        {
            [FormulaOperationType.Add] = (current, value) => current + value,
            [FormulaOperationType.Multiply] = (current, value) => current * value,
        };

        public static void ApplyParameter(this NodeData nodeData, FormulaParameter parameter)
        {
            var field = FormulaFields[parameter._propertyType];
            var currentValue = (float)field.GetValue(nodeData);
            var resultValue = Operations[parameter.OperationType].Invoke(currentValue, parameter.Value);

            // var multiplier = nodeData.Multipliers[parameter._propertyType];
            // resultValue *= multiplier;
            
            field.SetValue(nodeData, resultValue);
        }

        public static void ApplyModifier(this NodeData nodeData, FormulaParameter parameter)
        {
            var currentValue = nodeData.Multipliers[parameter._propertyType] + parameter.Value;
            nodeData.Multipliers[parameter._propertyType] = currentValue;
        }
        
        public static void AddAll(this NodeData baseData, NodeData additionalData)
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