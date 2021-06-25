using System;
using System.Collections.Generic;
using GenericNodes.Mech;
using GenericNodes.Mech.Data;
using GenericNodes.Utility;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Links;
using GenericNodes.Visual.Nodes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GenericNodes.Visual {
    public class WorkspaceArea : MonoBehaviour, 
                                 IPointerClickHandler, 
                                 IPointerDownHandler, 
                                 IPointerUpHandler {
        
        [SerializeField] private RectTransform rTrNodesRoot;
        [SerializeField] private CanvasScaler canvasScaler;
        [SerializeField] private float zoomSpeed = 0.5f;
        [SerializeField] private Vector2 zoomBounds = new Vector2(0.1f, 2f);
        [SerializeField] private NodeLinkSystem nodeLinkSystem;

        //private RectTransform rTransform;
        private bool isLmbHeld = false;
        private Vector2 fixedNodesRootPosition;
        private Vector2 mmbStartHoldPosition;
        private int lastNodeId = 0;
        
        private readonly List<NodeVisual> nodes = new List<NodeVisual>();
        public NodeLinkSystem LinkSystem => nodeLinkSystem;
        
        public event Action<Vector2> OnAreaLmbClick;
        public event Action<Vector2> OnAreaRmbClick;
        public event Action OnInterruptRmbClick;
        public event Action OnInterruptLmbClick;

        public RectTransform NodesRoot => rTrNodesRoot;
        public float CanvasScale => canvasScaler.scaleFactor;
        public UserHand Hand { get; set; }

        private void Awake() {
            //rTransform = GetComponent<RectTransform>();
            Hand = new UserHand(this);
        }

        private void LateUpdate() {
            Hand.Update(Time.deltaTime);
            if (isLmbHeld) {
                ProcessLmbHold(Input.mousePosition);
            }
            if (Mathf.Abs(Input.mouseScrollDelta.y) > 0 && !KeyboardInputManager.IsAnyGameObjectSelected) {
                float zoomDelta = Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
                canvasScaler.scaleFactor =
                    Mathf.Clamp(canvasScaler.scaleFactor + zoomDelta, zoomBounds.x, zoomBounds.y);
            }
        }

        public NodeId GetNextNodeId() {
            return new NodeId(lastNodeId++);
        }
        
        public Vector2 GetWorldPosition(Vector2 screenPosition) {
            return fixedNodesRootPosition - (mmbStartHoldPosition - screenPosition) / canvasScaler.scaleFactor;
        }

        public Vector2 GetFixedWorldPosition(Vector2 screenPosition) {
            return (screenPosition) / canvasScaler.scaleFactor - rTrNodesRoot.anchoredPosition;
        }

        public void OnPointerClick (PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                OnInterruptRmbClick?.Invoke();
                OnAreaLmbClick?.Invoke(eventData.position);
            }
            if (eventData.button == PointerEventData.InputButton.Right) {
                OnAreaRmbClick?.Invoke(eventData.position);
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                OnInterruptRmbClick?.Invoke();
                if (!isLmbHeld) {
                    isLmbHeld = true;
                    fixedNodesRootPosition = rTrNodesRoot.anchoredPosition;
                    mmbStartHoldPosition = eventData.position;    
                }
                ProcessLmbHold(eventData.position);
            }
            if (eventData.button == PointerEventData.InputButton.Right) {
                OnInterruptLmbClick?.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (isLmbHeld && eventData.button == PointerEventData.InputButton.Left) {
                OnInterruptRmbClick?.Invoke();
                isLmbHeld = false;
                fixedNodesRootPosition = rTrNodesRoot.anchoredPosition;
                mmbStartHoldPosition = eventData.position;
                ProcessMmbRelease(eventData.position);
            }
            if (eventData.button == PointerEventData.InputButton.Right) {
                OnInterruptLmbClick?.Invoke();
            }
        }

        private void ProcessLmbHold(Vector2 pointerPosition) {
            UpdateWorkspaceShift(GetWorldPosition(pointerPosition));
            OnInterruptRmbClick?.Invoke();
        }
        
        private void ProcessMmbRelease(Vector2 pointerPosition) {
            UpdateWorkspaceShift(GetWorldPosition(pointerPosition));
            OnInterruptRmbClick?.Invoke();
        }

        public void RegisterNode(NodeVisual node) {
            nodes.Add(node);
            node.OnActionClick += ProcessNodeClick;
            node.OnActionBeginDrag += ProcessNodeBeginDrag;
            node.OnActionEndDrag += ProcessNodeEndDrag;
        }

        private void ProcessNodeEndDrag(IHoldable holdable) {
            Hand.Reset();
        }
        
        private void ProcessNodeClick(IHoldable holdable) {
            if (Hand.NodeLink != null) {
                NodeVisual node = holdable as NodeVisual;
                INodeLinkSocket linkSocket = node.GetLinkSocket();
                if (!LinkSystem.ProcessLinkSocketClick(linkSocket)) {
                    node.ReleaseSocket(linkSocket);
                }
            }
        }

        private void ProcessNodeBeginDrag(IHoldable holdable) {
            if (Hand.Holdable == null) {
                Hand.SetHoldableItem(holdable);
            }
        }

        private void UpdateWorkspaceShift(Vector2 worldPosition) {
            rTrNodesRoot.anchoredPosition = worldPosition;
            nodeLinkSystem.Transform.anchoredPosition = worldPosition;
        }

        public NodeVisual GetNode(NodeId nodeId) {
            for (int i = 0; i < nodes.Count; ++i) {
                if (nodes[i].NodeId == nodeId) {
                    return nodes[i];
                }
            }
            return null;
        }
    }
}