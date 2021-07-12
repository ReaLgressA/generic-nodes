using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GenericNodes.Visual.Views {
    public abstract class ClickableEntry : MonoBehaviour,
                                           IPointerEnterHandler,
                                           IPointerClickHandler,
                                           IPointerExitHandler {

        [Header("Clickable Entry Setup")]
        [SerializeField] private Color colorNormal;
        [SerializeField] private Color colorHover;
        [SerializeField] private Color colorSelected;
        [SerializeField] private Image imageBackground;

        private bool isSelected = false;
        private bool isHover = false;

        public event Action Click;

        public bool IsSelected => isSelected;

        public void SetSelected(bool isSelected) {
            this.isSelected = isSelected;
            RefreshVisual();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            isHover = true;
            RefreshVisual();
        }

        public void OnPointerClick(PointerEventData eventData) {
            Click?.Invoke();   
        }

        public void OnPointerExit(PointerEventData eventData) {
            isHover = false;
            RefreshVisual();
        }

        public virtual void Reset() {
            isHover = false;
            isSelected = false;
            RefreshVisual();
        }
        
        private void RefreshVisual() {
            imageBackground.color = isHover
                ? colorHover
                : isSelected
                    ? colorSelected
                    : colorNormal;
        }
    }
}