using Petri.Events;
using Petri.Infrostructure;
using Petri.UI.Events;
using UnityEngine;

namespace Petri.UI
{
    public class DragAndDropProcessor : MonoBehaviour, IEventReceiver<DragStartEvent>, IEventReceiver<DragEndEvent>
    {
        [SerializeField] private Canvas _canvas;

        private IDragTarget _currentDragInfo;
        private bool _isDragging;

        private void Start()
        {
            var eventBus = ServiceLocator.Get<EventBus>();
            eventBus.Register<DragStartEvent>(this);
            eventBus.Register<DragEndEvent>(this);
        }

        public void OnEvent(DragStartEvent @event)
        {
            _currentDragInfo = @event.DragTarget.CopyForPreview();
            _currentDragInfo.SetBlockRaycasts(false);
            var rectTransform = _currentDragInfo.GameObject.GetComponent<RectTransform>();

            rectTransform.SetParent(_canvas.transform);
            _isDragging = true;
        }

        public void OnEvent(DragEndEvent @event)
        {
            _currentDragInfo.Destroy();
            _currentDragInfo = null;
            _isDragging = false;
        }

        private void Update()
        {
            if (!_isDragging)
            {
                return;
            }

            _currentDragInfo.SetPosition(Input.mousePosition);
        }

        public UniqueId Id { get; } = new();
    }
}