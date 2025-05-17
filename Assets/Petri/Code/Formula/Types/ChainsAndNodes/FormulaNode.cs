using System.Collections.Generic;
using UnityEngine;

namespace Petri.Formula
{
    public class FormulaNode
    {
        public FormulaNode Output;
        public HashSet<FormulaNode> Input;
        public Reagent Reagent;
        public Vector2Int Position;

        /// <summary>
        /// if next node is bound node
        /// </summary>
        public bool IsBoundNode;
        public bool IsPointsToOtherChain;

        //todo: здесь должны быть разные данные пипетки
        public float Value;

        public FormulaNode(Reagent reagent, Vector2Int position)
        {
            Input = new ();
            Reagent = reagent;
            Position = position;
        }
    }
}