using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Petri.UI
{
    public class HoverColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _image;
        
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _hoverColor;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _image.color = _hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.color = _defaultColor;
        }
    }
}