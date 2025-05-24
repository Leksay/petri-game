namespace Petri.Formula
{
    public class FormulaData
    {
        public static FormulaData Empty { get; } = new();

        public float Damage;
        public float Size;

        public override string ToString() => $"Damage: {Damage}, Size: {Size}";

        public FormulaData Copy() => new(){Damage = this.Damage, Size = this.Size};

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