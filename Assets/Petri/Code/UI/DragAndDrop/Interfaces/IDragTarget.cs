using UnityEngine;

namespace Petri.UI
{
    public interface IDragTarget
    {
        public GameObject GameObject { get; }
        public void SetPosition(Vector2 position);
        public void Destroy();
        public IDragTarget CopyForPreview();
        public void SetBlockRaycasts(bool value);

        public object Payload { get; }
    }
}