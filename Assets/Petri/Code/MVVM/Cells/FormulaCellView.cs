using System;
using Petri.Configs;
using Petri.Formula;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Petri.UI
{
    public class FormulaCellView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public ReactiveProperty<ReagentView> EnteredDragObject { get; } = new();
        public ReactiveCommand Cleared { get; } = new();

        [field: SerializeField] public int CellX { get; private set; }
        [field: SerializeField] public int CellY { get; private set; }
        [field: SerializeField] public bool HasReagent { get; private set; }

        [SerializeField] private Image _background;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _iconBackground;
        [SerializeField] private Image _outOfChainImage;
        [SerializeField] private Button _clearButton;

        [Header("Connections")]
        [SerializeField] private Image _upConnection;
        [SerializeField] private Image _downConnection;
        [SerializeField] private Image _leftConnection;
        [SerializeField] private Image _rightConnection;

        [SerializeField] private Color _backgroundColor;
        private ReagentConnectionView _reagentConnectionView;

        #if UNITY_EDITOR
        [Header("Debug")] 
        [SerializeField] private bool _showStats;
        [SerializeField] private TextMeshProUGUI _damage;
        #endif

        private void Awake()
        {
            _reagentConnectionView = GetComponentInChildren<ReagentConnectionView>(true);
            _clearButton.OnClickAsObservable().Subscribe(_ =>
            {
                if (!HasReagent)
                {
                    return;
                }

                Cleared.Execute(Unit.Default);
            });
        }

        public void Initialize(int cellX, int cellY)
        {
            CellX = cellX;
            CellY = cellY;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var reagent = eventData.pointerDrag?.GetComponent<ReagentView>();
            if (!reagent)
            {
                return;
            }

            EnteredDragObject.Value = reagent;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EnteredDragObject.Value = null;
        }

        public void SetBackgroundColor(Color cellColor)
        {
            _background.color = cellColor;
        }

        public void SetNormalColor() => _background.color = _backgroundColor;

        public void SetDefaultBackgroundColor(Color color) => _backgroundColor = color;

        public void SetConnectionView(bool isActive) => _reagentConnectionView.gameObject.SetActive(isActive);

        public void SetReagentToCell(Reagent reagent)
        {
            HasReagent = reagent != null;

            _iconBackground.color = reagent?.Modifier.Parameter.ReagentGroup.GetColor() ?? Color.clear;
            _icon.sprite = reagent?.Modifier.Parameter.Icon;

            _icon.gameObject.SetActive(HasReagent);
            _iconBackground.gameObject.SetActive(HasReagent);
            _reagentConnectionView.gameObject.SetActive(HasReagent);
            _clearButton.gameObject.SetActive(HasReagent);

            if (HasReagent)
            {
                _reagentConnectionView.SetConnections(reagent.ConnectionType);
            }
        }

        public void SetConnection(ConnectionTypes connectionType, FormulaNodeColors colors)
        {
            _upConnection.gameObject.SetActive((connectionType & ConnectionTypes.Up) > 0);
            _downConnection.gameObject.SetActive((connectionType & ConnectionTypes.Down) > 0);
            _leftConnection.gameObject.SetActive((connectionType & ConnectionTypes.Left) > 0);
            _rightConnection.gameObject.SetActive((connectionType & ConnectionTypes.Right) > 0);

            _upConnection.color = colors.Up.GetColor();
            _downConnection.color = colors.Down.GetColor();
            _leftConnection.color = colors.Left.GetColor();
            _rightConnection.color = colors.Right.GetColor();
        }

        public void SetChainStatus(bool isCellInChain)
        {
            if (HasReagent && !isCellInChain)
            {
                _outOfChainImage.gameObject.SetActive(true);
                return;
            }

            _outOfChainImage.gameObject.SetActive(false);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            _damage.gameObject.SetActive(_showStats);
        }

        public void SetStats(FormulaData cellData)
        {
            if (!_showStats)
            {
                return;
            }

            _damage.gameObject.SetActive(true);
            _damage.text = cellData.Damage.ToString();
        }
#endif
    }
}