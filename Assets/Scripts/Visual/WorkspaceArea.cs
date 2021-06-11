using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Visual {
    public class WorkspaceArea : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private Image imageBackground;
        
        public event Action<Vector2> OnAreaRightClicked;

        private RectTransform rTransform;
        
        private void Awake() {
            rTransform = GetComponent<RectTransform>();
        }

        public void OnPointerClick (PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Right) {
                Debug.Log ($"Right Mouse Button Clicked position: {eventData.position}");
                OnAreaRightClicked?.Invoke(eventData.position + rTransform.anchoredPosition);
            }
        }
    }
}