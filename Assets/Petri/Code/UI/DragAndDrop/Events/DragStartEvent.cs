using Petri.Events;

namespace Petri.UI.Events
{
    public struct DragStartEvent : IEvent
    {
        public readonly IDragTarget DragTarget;

        public DragStartEvent(IDragTarget dragTarget)
        {
            DragTarget = dragTarget;
        }
    }
}