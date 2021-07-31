using GenericNodes.Mech.Data;
using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.GenericFields {
    
    public class NodeIdGenericField : MonoBehaviour,
                                      INodeIdSocketContainer,
                                      IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private NodeSocketVisual linkSocket;
        [SerializeField] private Button buttonLink;
        
        private RectTransform rectTransform;

        public NodeIdDataField Field { get; private set; }
        public NodeVisual MasterNode { get; private set; }
        public NodeId NodeId => MasterNode.NodeId;
        public Vector2 ParentPositionShift => Transform.anchoredPosition + Parent.ParentPositionShift;
        public IGenericFieldParent Parent { get; private set; }

        public RectTransform Transform => rectTransform ??= GetComponent<RectTransform>();
        
        private void Awake() {
            buttonLink.onClick.AddListener(ProcessLinkButtonClick);
            // linkSocket.SocketLinked += ProcessSocketLinked;
        }

        private void OnDisable() {
            UnlinkSocket();
        }

        private void OnDestroy() {
            // linkSocket.SocketLinked -= ProcessSocketLinked;
            //
        }

        public void SetData(NodeIdDataField field) {
            if (Field != null) {
                Field.ValueChanged -= ProcessLinkedNodeIdChanged;    
            }
            Field = field;
            Field.ValueChanged += ProcessLinkedNodeIdChanged;
            textLabel.text = Field.Name;
            linkSocket.Initialize(this);
            
            UnlinkSocket();
        }

        private void Update() {
            textLabel.text = Field == null ? "NULL" : $"{Field.Name}[{Field.Value.Id}]";
        }

        public void SetLinkedNodeId(NodeId nodeId) {
            Field.SetId(nodeId);
        }

        public void SetData(NodeVisual nodeVisual, DataField data, IGenericFieldParent fieldParent) {
            MasterNode = nodeVisual;
            Parent = fieldParent;
            SetData(data as NodeIdDataField);
        }

        public void Destroy() {
            UnlinkSocket();
            Field = null;
            GameObject.Destroy(gameObject);
        }

        public void RebuildLinks() {
            UnlinkSocket();
            if (Field.Value != NodeId.None) {
                NodeVisual targetNode = MasterNode.Workspace.GetNode(Field.Value);
                if (targetNode == null) {
                    Field.SetId(NodeId.None);
                    return;
                }
                MasterNode.Workspace.LinkSystem.LinkSocketToNode(linkSocket, Field.Value);
            }
        }
    
        private void ProcessLinkButtonClick() {
            UnlinkSocket();
            MasterNode.Workspace.LinkSystem.ProcessLinkSocketClick(linkSocket);
        }

        private void UnlinkSocket() {
            MasterNode?.Workspace?.LinkSystem?.UnlinkSocket(linkSocket);
        }
        
        private void ProcessLinkedNodeIdChanged(NodeIdDataField dataField) {
            // UnlinkSocket();
            // if (dataField.Value != NodeId.None) {
            //     MasterNode.Workspace.LinkSystem.LinkSocketToNode(linkSocket, dataField.Value);
            // }
        }
    }
    
}