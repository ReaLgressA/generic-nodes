using System;
using GenericNodes.Visual.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GenericNodes.Visual
{
    public class DraggableEventTrigger : MonoBehaviour, 
                                         IPointerClickHandler, 
                                         IBeginDragHandler, 
                                         IDragHandler, 
                                         IEndDragHandler, 
                                         IPointerEnterHandler, 
                                         IPointerExitHandler, 
                                         IDropHandler {
    
        public event Action<IHoldable> OnActionLeftClick;
        public event Action<IHoldable> OnActionRightClick;
        public event Action<IHoldable> OnActionBeginDrag;
        public event Action<IHoldable> OnActionDrag;
        public event Action<IHoldable> OnActionEndDrag;
        public event Action<IHoldable> OnActionDrop;
        public event Action<IHoldable> OnActionHoverAcquired;
        public event Action<IHoldable> OnActionHoverLost;

        private IHoldable holdable;

        protected virtual void Awake() {
            holdable = GetComponent<IHoldable>();
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                OnActionLeftClick?.Invoke(holdable);
            }
            if (eventData.button == PointerEventData.InputButton.Right) {
                OnActionRightClick?.Invoke(holdable);
            }
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                OnActionBeginDrag?.Invoke(holdable);
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                OnActionEndDrag?.Invoke(holdable);
            }
        }

        public void OnDrop(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                OnActionDrop?.Invoke(holdable);
            }
        }

        public void OnDrag(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                OnActionDrag?.Invoke(holdable);
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData) {
            OnActionHoverAcquired?.Invoke(holdable);
        }

        public void OnPointerExit(PointerEventData eventData) {
            OnActionHoverLost?.Invoke(holdable);
        }
    }

    public enum HoldableType {
        Node
    }
}