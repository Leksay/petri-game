using System.Collections.Generic;

namespace Petri.Formula
{
    public class FormulaChain
    {
        public FormulaNode StartNode;
        public FormulaNode EndNode;
        public List<FormulaNode> AllNodes = new();

        public bool EndsInOtherChain => EndNode?.IsPointsToOtherChain ?? false;
        public bool EndsInBound => EndNode?.IsBoundNode ?? false;

        public void Clear()
        {
            StartNode = EndNode = null;
            AllNodes.ForEach(node => node.Data?.Clear());
            AllNodes.Clear();
        }

        public bool ContainsNode(FormulaNode node) => AllNodes?.Contains(node) ?? false;
    }
}