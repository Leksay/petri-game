using System.Collections;
using Petri.Configs;
using Petri.Events;
using Petri.Formula;
using Petri.Infrostructure;
using Petri.UI.Events;
using TLab.UI.SDF;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Petri.UI
{
    public class ReagentView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDragTarget
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _parameter;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private SDFQuad _outline;
        [SerializeField] private GameObject _availableMask;

        private ReagentConnectionView _reagentConnectionView;
        private Reagent _payload;
        private RectTransform _rectTransform;
        private Vector2 _sizeDelta;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
            _canvasGroup = GetComponent<CanvasGroup>();
            _reagentConnectionView = GetComponentInChildren<ReagentConnectionView>();
        }

        private IEnumerator Start()
        {
            yield return null;
            var rect = GetComponent<RectTransform>().rect;
            _sizeDelta = new Vector2(rect.width, rect.height);
        }

        public void Initialize(Reagent payload)
        {
            _background.color = payload.Modifier.Parameter.ReagentGroup.GetColor();
            _icon.sprite = payload.Modifier.Parameter.Icon;
            _name.text = payload.IsAvailable ? payload.Modifier.Parameter.Name : "???";
            _payload = payload;
            _availableMask.SetActive(!payload.IsAvailable);
            _reagentConnectionView.SetConnections(payload.ConnectionType);
        }

        #region drag

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_payload.IsAvailable)
            {
                ServiceLocator.Get<EventBus>().Raise(new DragStartEvent(this));
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_payload.IsAvailable)
            {
                ServiceLocator.Get<EventBus>().Raise(new DragEndEvent());
            }
        }

        #endregion

        #region IDragTarget

        public GameObject GameObject => gameObject;
        public object Payload => _payload;

        public void SetPosition(Vector2 position) => _rectTransform.anchoredPosition = position;

        public void Destroy() => Destroy(gameObject);

        public IDragTarget CopyForPreview()
        {
            var copy = Instantiate(gameObject);
            var reagentView = copy.GetComponent<ReagentView>();
            var rectTransform = copy.GetComponent<RectTransform>();

            rectTransform.localScale *= 0.8f;
            reagentView._outline.enabled = true;
            reagentView._name.gameObject.SetActive(false);
            reagentView._parameter.gameObject.SetActive(false);

            rectTransform.sizeDelta = _sizeDelta;

            return reagentView;
        }

        public void SetBlockRaycasts(bool value) => _canvasGroup.blocksRaycasts = value;

        #endregion
    }
}