using System;
using GenericNodes.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GenericNodes.Visual {
    public class WorkspaceArea : MonoBehaviour,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler {
        [SerializeField]
        private RectTransform rTrNodesRoot;

        [SerializeField]
        private CanvasScaler canvasScaler;

        [SerializeField]
        private float zoomSpeed = 0.5f;

        [SerializeField]
        private Vector2 zoomBounds = new Vector2(0.1f, 2f);

        private RectTransform rTransform;
        private bool isMmbHeld = false;
        private Vector2 fixedNodesRootPosition;
        private Vector2 mmbStartHoldPosition;

        public event Action<Vector2> OnAreaRmbClick;
        public event Action OnInterruptRmbClick;

        public RectTransform NodesRoot => rTrNodesRoot;
        public float CanvasScale => canvasScaler.scaleFactor;

        private void Awake() {
            rTransform = GetComponent<RectTransform>();
        }

        private void LateUpdate() {
            if (isMmbHeld) {
                ProcessMmbHold(Input.mousePosition);
            }

            if (Mathf.Abs(Input.mouseScrollDelta.y) > 0 && !KeyboardInputManager.IsAnyGameObjectSelected) {
                float zoomDelta = Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
                canvasScaler.scaleFactor =
                    Mathf.Clamp(canvasScaler.scaleFactor + zoomDelta, zoomBounds.x, zoomBounds.y);
            }
        }

        public Vector2 GetWorldPosition(Vector2 screenPosition) {
            return fixedNodesRootPosition - (mmbStartHoldPosition - screenPosition) / canvasScaler.scaleFactor;
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
            rTrNodesRoot.anchoredPosition = GetWorldPosition(pointerPosition);
            OnInterruptRmbClick?.Invoke();
        }
        
        private void ProcessMmbRelease(Vector2 pointerPosition) {
            rTrNodesRoot.anchoredPosition = GetWorldPosition(pointerPosition);
            OnInterruptRmbClick?.Invoke();
        }
    }
}