using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;
using Image = UnityEngine.UI.Image;

namespace Visual {
    public class WorkspaceArea : MonoBehaviour, 
                                 IPointerClickHandler,
                                 IPointerDownHandler,
                                 IPointerUpHandler {
        [SerializeField] private Image imageBackground;
        [SerializeField] private RectTransform rTrNodesRoot;
        [SerializeField] private CanvasScaler canvasScaler;

        [SerializeField] private float zoomSpeed = 0.5f;
        [SerializeField] private Vector2 zoomBounds = new Vector2(0.1f, 2f);
        
        private RectTransform rTransform;
        private RectTransform rTrBackground;
        private bool isMmbHeld = false;
        private Vector2 fixedNodesRootPosition;
        private Vector2 mmbStartHoldPosition;
        
        public event Action<Vector2> OnAreaRmbClick;
        public event Action OnInterruptRmbClick;

        public RectTransform NodesRoot => rTrNodesRoot;
        public float CanvasScale => canvasScaler.scaleFactor;

        private void Awake() {
            rTransform = GetComponent<RectTransform>();
            rTrBackground = imageBackground.GetComponent<RectTransform>();
        }

        private void LateUpdate() {
            if (isMmbHeld) {
                ProcessMmbHold(Input.mousePosition);
            }
            if (Mathf.Abs(Input.mouseScrollDelta.y) > 0 && !KeyboardInputManager.IsAnyGameObjectSelected) {
                float zoomDelta = Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
                canvasScaler.scaleFactor = Mathf.Clamp(canvasScaler.scaleFactor + zoomDelta, zoomBounds.x, zoomBounds.y);
            }
        }

        public void OnPointerClick (PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Right) {
                Debug.Log ($"Right Mouse Button Clicked position: {eventData.position}");
                OnAreaRmbClick?.Invoke(eventData.position + rTransform.anchoredPosition);
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Middle) {
                if (!isMmbHeld) {
                    isMmbHeld = true;
                    fixedNodesRootPosition = rTrNodesRoot.anchoredPosition;
                    mmbStartHoldPosition = eventData.position;    
                }
                ProcessMmbHold(eventData.position);
            }
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (isMmbHeld && eventData.button == PointerEventData.InputButton.Middle) {
                isMmbHeld = false;
                ProcessMmbRelease(eventData.position);
            }
        }

        private void ProcessMmbHold(Vector2 pointerPosition) {
            rTrNodesRoot.anchoredPosition = fixedNodesRootPosition - (mmbStartHoldPosition - pointerPosition) / canvasScaler.scaleFactor;
            OnInterruptRmbClick?.Invoke();
        }
        
        private void ProcessMmbRelease(Vector2 pointerPosition) {
            rTrNodesRoot.anchoredPosition = fixedNodesRootPosition - (mmbStartHoldPosition - pointerPosition) / canvasScaler.scaleFactor;
            OnInterruptRmbClick?.Invoke();
        }
    }
}