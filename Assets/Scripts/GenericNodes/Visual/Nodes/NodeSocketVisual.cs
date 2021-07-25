using System;
using GenericNodes.Mech.Data;
using GenericNodes.Visual.GenericFields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Links;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Nodes {

    public enum NodeSocketMode {
        Input,
        Output
    }
    
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RectTransform))]
    public class NodeSocketVisual : MonoBehaviour,
                                    INodeLinkSocket {
        private static readonly Color socketColor = new Color(0.5f, 0, 1f);
        
        [SerializeField] private NodeSocketMode mode = NodeSocketMode.Input;

        private Image iconSocket;
        private RectTransform rectTransform = null;
        
        private Vector2 lastTrackedPosition = Vector2.zero;

        public Image IconSocket => iconSocket ??= GetComponent<Image>();
        public RectTransform Transform => rectTransform ??= GetComponent<RectTransform>();

        public NodeSocketMode Mode => mode;
        //public event Action<INodeLinkSocket, NodeId> SocketLinked;
        public event Action PositionChanged;

        public IGenericFieldParent FieldParent { get; private set; }
        public NodeId Id => FieldParent.NodeId;
        
        //public NodeId LinkedToId { get; private set; } = NodeId.None;
        
        public Vector2 Position => Transform.anchoredPosition + FieldParent.ParentPositionShift;

        public Color LinkColor => socketColor;

        private void Update() {
            if ((Position - lastTrackedPosition).sqrMagnitude > 0.01f) {
                PositionChanged?.Invoke();
                lastTrackedPosition = Position;
            }
        }

        public void Initialize(NodeVisual nodeVisual) {
            FieldParent = nodeVisual;
        }

        public void Initialize(NodeIdGenericField nodeIdField) {
            FieldParent = nodeIdField;
        }

        // public void LinkSocketTo(NodeId nodeId) {
        //     //LinkedToId = nodeId;
        //     SocketLinked?.Invoke(this, nodeId);
        // }
    }
}