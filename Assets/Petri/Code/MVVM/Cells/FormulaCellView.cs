using Petri.Configs;
using Petri.Formula;
using R3;
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
        [SerializeField] private GameObject _upConnection;
        [SerializeField] private GameObject _downConnection;
        [SerializeField] private GameObject _leftConnection;
        [SerializeField] private GameObject _rightConnection;

        private Color _backgroundColor;
        private ReagentConnectionView _reagentConnectionView;

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
            _backgroundColor = cellColor;
        }

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

        public void SetConnection(ConnectionTypes connectionType)
        {
            _rightConnection.SetActive((connectionType & ConnectionTypes.Right) > 0);
            _leftConnection.SetActive((connectionType & ConnectionTypes.Left) > 0);
            _upConnection.SetActive((connectionType & ConnectionTypes.Up) > 0);
            _downConnection.SetActive((connectionType & ConnectionTypes.Down) > 0);
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
    }
}