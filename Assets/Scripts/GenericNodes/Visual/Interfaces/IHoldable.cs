using UnityEngine;

namespace GenericNodes.Visual.Interfaces {
    public interface IHoldable {
        DraggableEventTrigger EventTrigger { get; }
        HoldableType HoldableType { get; } 
        RectTransform Transform { get; }
    }
}