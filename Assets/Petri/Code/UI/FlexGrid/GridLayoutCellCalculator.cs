using UnityEngine;
using UnityEngine.UI;

namespace Petri.UI
{
    [RequireComponent(typeof(GridLayoutGroup))]
    [ExecuteInEditMode]
    public class GridLayoutCellCalculator : MonoBehaviour
    {
        private GridLayoutGroup GridLayoutGroup => _gridLayoutGroup ??= GetComponent<GridLayoutGroup>();
        private GridLayoutGroup _gridLayoutGroup;

        private RectTransform GridRectTransform
        {
            get
            {
                if (!_gridRectTransform)
                {
                    _gridRectTransform = GetComponent<RectTransform>();
                }

                return _gridRectTransform;
            }
        }

        private RectTransform _gridRectTransform;

        private Vector2 _lastSize;

        private void Update()
        {
            if (GridRectTransform.rect.size == _lastSize)
            {
                return;
            }

            UpdateSizes();
        }

        public void UpdateSizes()
        {
            var gridHeight = GridRectTransform.rect.height;
            var cellCount = GridLayoutGroup.transform.childCount;
            var rowCount = (float)cellCount / GridLayoutGroup.constraintCount;

            var height = gridHeight / rowCount - GridLayoutGroup.spacing.y + GridLayoutGroup.spacing.y / (rowCount - 1);
            GridLayoutGroup.cellSize = new Vector2(height, height);
            _lastSize = GridRectTransform.rect.size;
        }
    }
}