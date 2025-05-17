using Petri.Formula;
using Petri.Models;
using Petri.UI;
using R3;
using UnityEngine;

namespace Petri.ViewModels
{
    public class PipetteViewModel
    {
        public PipetteViewModel(UIPipetteView view, PipetteModel model)
        {
            view.OnReagentDropped.Subscribe(tuple =>
            {
                //todo:
                //1. Check that reagent can be placed
                //2. Place reagent

                Debug.Log($"Dropped: {tuple.x}, {tuple.y} and reagent {tuple.reagentView.Payload}");
                model.PlaceReagent(tuple.x, tuple.y, (Reagent)tuple.reagentView.Payload);
            });

            view.CellCleared.Subscribe(xy => model.RemoveReagent(xy.x, xy.y));

            model.ReagentChanged.Subscribe(tuple =>
            {
                view.SetReagentToCell(tuple.x, tuple.y, tuple.reagent);
                view.UpdateConnections(model.ConnectionMatrix);
                view.SetChainsTails(model.ChainTails);

                var chainedCells = model.GetChainedCellsMatrix();
                for (var x = 0; x < chainedCells.GetLength(0); x++)
                {
                    for (var y = 0; y < chainedCells.GetLength(1); y++)
                    {
                        view.SetCellChainStatus(x, y, chainedCells[x, y]);
                    }
                }
            });
        }
    }
}