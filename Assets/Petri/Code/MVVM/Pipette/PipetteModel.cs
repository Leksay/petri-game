using System.Collections.Generic;
using Petri.Formula;
using R3;
using UnityEngine;

namespace Petri.Models
{
    public class PipetteModel
    {
        public Reagent[,] Reagents;
        public ReactiveCommand<(int x, int y, Reagent reagent)> ReagentChanged { get; } = new();
        
        public ConnectionTypes[,] ConnectionMatrix { get; private set;}
        public List<Vector2Int?> ChainTails { get; private set; }

        private FormulaConstructor _formulaConstructor;

        public PipetteModel(PipetteSizeConfiguration sizeConfiguration)
        {
            var (x, y) = sizeConfiguration.GetSize();
            Reagents = new Reagent[x, y];
            _formulaConstructor = new FormulaConstructor().CreateFormula(x, y);
        }

        public void PlaceReagent(int x, int y, Reagent reagent)
        {
            Reagents[x, y] = reagent;
            _formulaConstructor.AddReagent(reagent, x, y);
            ConnectionMatrix = _formulaConstructor.GetConnectionMatrix();
            ChainTails = _formulaConstructor.GetChainsTails();

            ReagentChanged.Execute((x, y, reagent));
        }

        public void RemoveReagent(int x, int y)
        {
            Reagents[x, y] = null;
            _formulaConstructor.RemoveReagent(x, y);
            ConnectionMatrix = _formulaConstructor.GetConnectionMatrix();
            ChainTails = _formulaConstructor.GetChainsTails();

            ReagentChanged.Execute((x, y, null));
        }

        public bool[,] GetChainedCellsMatrix() => _formulaConstructor.GetChainedCellsMatrix();
    }
}