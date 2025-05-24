namespace Petri.Formula
{
    /// <summary>
    /// Как модификатор будет применяться к формуле.
    /// </summary>
    public enum FormulaModifierApplyType
    {
        Once,
        OnceNext,
        // OnceBefore,
        AllAfter,
        // AllBefore,
    }
    
    /// <summary>
    /// Тип операции над формулой.
    /// </summary>
    public enum FormulaOperationType
    {
        None,
        Add,
        Subtract,
        Multiply,
        Divide,
    }

    /// <summary>
    /// Default sizes of pipette cells.
    /// </summary>
    public enum PipetteSizeConfiguration
    {
        //2x8
        Small_2x8,
        //3x8
        Medium_3x8,
        //4x8
        Big_4x8,
        //5x8
        Large_5x8
    }

    public static class FormulaModifierApplyTypeExtensions
    {
        public static bool IsAffectNextNode(this FormulaModifierApplyType type) => type is FormulaModifierApplyType.OnceNext or FormulaModifierApplyType.AllAfter;
    }
}