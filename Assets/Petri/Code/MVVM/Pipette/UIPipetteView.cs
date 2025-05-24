using System.Collections.Generic;
using JetBrains.Annotations;
using JfranMora.Inspector;
using Petri.Events;
using Petri.Formula;
using Petri.Infrostructure;
using Petri.UI.Events;
using R3;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Petri.UI
{
    public class UIPipetteView : MonoBehaviour, IEventReceiver<DragEndEvent>
    {
        public ReactiveCommand<(int x, int y, ReagentView reagentView)> OnReagentDropped { get; } = new();
        public ReactiveCommand<(int x, int y)> CellCleared { get; } = new();

        [SerializeField] private Color _normalCellColor;
        [SerializeField] private Color _availableCellColor;

        [Header("Cells Configuration")]
        [field: SerializeField]
        public PipetteSizeConfiguration SizeConfiguration { get; private set; }

        [SerializeField] private RectTransform _cellsParent;
        [SerializeField] private FormulaCellView _cellPrefab;

        private EventBus _eventBus;
        private IEventReceiver<DragEndEvent> _eventReceiverImplementation;
        private ReagentView _draggingReagent;
        private FormulaCellView _lastHoveredFormulaCell;
        private GridLayoutCellCalculator _cellCalculator;
        private GridLayoutGroup _grid;

        private FormulaCellView[,] _cells;

        private void Start()
        {
            _eventBus = ServiceLocator.Get<EventBus>();
            _eventBus.Register<DragEndEvent>(this);
            _grid = _cellPrefab.GetComponent<GridLayoutGroup>();
            _cellCalculator = GetComponentInChildren<GridLayoutCellCalculator>();

            Initialize();
        }

        private void Initialize()
        {
            var childCells = GetComponentsInChildren<FormulaCellView>();
            var (x, y) = SizeConfiguration.GetSize();
            _cells = new FormulaCellView[x, y];

            foreach (var cell in childCells)
            {
                cell.EnteredDragObject.Subscribe(reagentView =>
                {
                    _draggingReagent = reagentView;
                    _lastHoveredFormulaCell = cell;
                    if (reagentView && CanReagentBePlacedAt(cell, reagentView))
                    {
                        cell.SetBackgroundColor(_availableCellColor);
                    }
                    else
                    {
                        cell.SetNormalColor();
                    }

                    _cells[cell.CellX, cell.CellY] = cell;
                });

                cell.Cleared.Subscribe(_ =>
                {
                    cell.SetNormalColor();
                    CellCleared.Execute((cell.CellX, cell.CellY));
                });
            }

            UpdateCellsColor();
        }

        [Button]
        private void UpdateCells()
        {
            var (x, y) = SizeConfiguration.GetSize();
            var childCount = _cellsParent.childCount;

            _grid ??= GetComponentInChildren<GridLayoutGroup>();

            if (childCount != x * y)
            {
                CreateCells();
            }
        }

        private void CreateCells()
        {
            while (_cellsParent.childCount > 0)
            {
                DestroyImmediate(_cellsParent.GetChild(0).gameObject);
            }

            var (xSize, ySize) = SizeConfiguration.GetSize();
            _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _grid.constraintCount = xSize;

            var darkCellColor = _normalCellColor * 0.9f;
            darkCellColor.a = 1f;

#if UNITY_EDITOR
            var prefabSettings = new ConvertToPrefabInstanceSettings();
#endif

            for (var x = 0; x < xSize; x++)
            {
                for (var y = 0; y < ySize; y++)
                {
                    var newCell = Instantiate(_cellPrefab, _cellsParent);
#if UNITY_EDITOR
                    PrefabUtility.ConvertToPrefabInstance(newCell.gameObject, _cellPrefab.gameObject, prefabSettings,
                        InteractionMode.AutomatedAction);
#endif
                    newCell.Initialize(x, y);
                    newCell.gameObject.name = $"Cell {x}, {y}";
                    newCell.SetNormalColor();
                }
            }

            _cellCalculator ??= GetComponentInChildren<GridLayoutCellCalculator>();
            _cellCalculator.UpdateSizes();
        }

        private void UpdateCellsColor()
        {
            var (xSize, ySize) = SizeConfiguration.GetSize();
            var darkCellColor = _normalCellColor * 0.9f;
            darkCellColor.a = 1f;

            for (var x = 0; x < xSize; x++)
            {
                for (var y = 0; y < ySize; y++)
                {
                    _cells[x, y].SetDefaultBackgroundColor(y % 2 == 0 ? _normalCellColor : darkCellColor);
                    _cells[x, y].SetNormalColor();
                }
            }
        }

        private static bool CanReagentBePlacedAt(FormulaCellView cell, ReagentView reagentView)
        {
            //todo: Check if the reagent can be placed at the cell
            return true;
        }

        public void OnEvent(DragEndEvent @event)
        {
            if (_draggingReagent && _lastHoveredFormulaCell)
            {
                OnReagentDropped.Execute((_lastHoveredFormulaCell.CellX, _lastHoveredFormulaCell.CellY, _draggingReagent));
            }
        }

        public UniqueId Id { get; } = new();

        public void SetReagentToCell(int x, int y, Reagent reagent)
        {
            _cells[x, y].SetReagentToCell(reagent);
        }

        public void UpdateConnections(ConnectionTypes[,] connectionMatrix, FormulaNodeColors[,] colorMatrix)
        {
            for (var x = 0; x < connectionMatrix.GetLength(0); x++)
            {
                for (var y = 0; y < connectionMatrix.GetLength(1); y++)
                {
                    _cells[x, y].SetConnection(connectionMatrix[x, y], colorMatrix[x, y]);
                }
            }
        }

        public void SetChainsTails(List<Vector2Int?> modelChainTails)
        {
            foreach (var cellView in _cells)
            {
                cellView.SetConnectionView(false);
            }

            foreach (var tail in modelChainTails)
            {
                if (tail == null)
                {
                    continue;
                }

                _cells[tail.Value.x, tail.Value.y].SetConnectionView(true);
            }
        }

        public void SetCellChainStatus(int x, int y, bool isCellInChain) => _cells[x, y].SetChainStatus(isCellInChain);

#if UNITY_EDITOR
        public void SetCellStats(int x, int y, [CanBeNull] FormulaData cellData)
        {
            if (cellData == null)
            {
                return;
            }

            _cells[x, y].SetStats(cellData);
        }
#endif
    }
}