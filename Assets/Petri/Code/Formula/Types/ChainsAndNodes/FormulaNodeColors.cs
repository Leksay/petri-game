namespace Petri.Formula
{
    public struct FormulaNodeColors
    {
        public ReagentGroup Up, Down, Left, Right;

        public static bool operator ==(FormulaNodeColors left, FormulaNodeColors right)
        {
            return left.Up == right.Up && left.Down == right.Down && left.Left == right.Left &&
                   left.Right == right.Right;
        }

        public static bool operator !=(FormulaNodeColors left, FormulaNodeColors right)
        {
            return left.Up != right.Up || left.Down != right.Down || left.Left != right.Left ||
                   left.Right != right.Right;
        }
    }
}