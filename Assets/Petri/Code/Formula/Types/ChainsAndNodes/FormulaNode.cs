using System.Collections.Generic;
using UnityEngine;

namespace Petri.Formula
{
    public class FormulaNode
    {
        public FormulaNode OutputNode;
        public HashSet<FormulaNode> InputNodes;
        public Reagent Reagent;
        public Vector2Int Position;
        public NodeData Data;

        public FormulaParameter Parameter => Reagent?.Modifier.Parameter;
        public FormulaModifierApplyType ApplyType => Reagent.Modifier.Parameter.ApplyType;
        
        /// <summary>
        /// if next node is bound node
        /// </summary>
        public bool IsBoundNode;
        public bool IsPointsToOtherChain;

        public FormulaNode(Reagent reagent, Vector2Int position)
        {
            InputNodes = new ();
            Reagent = reagent;
            Position = position;
            Data = new NodeData();
        }
    }
}