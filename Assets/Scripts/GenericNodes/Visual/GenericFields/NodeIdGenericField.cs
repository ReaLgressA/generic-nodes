using System;
using System.Threading.Tasks;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Links;
using GenericNodes.Visual.Nodes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.GenericFields {
    
    public class NodeIdGenericField : MonoBehaviour,
                                      IGenericFieldParent,
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
            UnlinkSocketIfNeeded();
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
            
            UnlinkSocketIfNeeded();
            
        }

        public void SetData(NodeVisual nodeVisual, DataField data, IGenericFieldParent fieldParent) {
            MasterNode = nodeVisual;
            Parent = fieldParent;
            SetData(data as NodeIdDataField);
        }

        public void Destroy() {
            UnlinkSocketIfNeeded();
            Field = null;
            GameObject.Destroy(gameObject);
        }

        public void RebuildLinks() {
            UnlinkSocketIfNeeded();
            if (Field.Value != NodeId.None) {
                MasterNode.Workspace.LinkSystem.LinkSocketToNode(linkSocket, Field.Value);
            }
        }
    
        private void ProcessLinkButtonClick() {
            UnlinkSocketIfNeeded();
            MasterNode.Workspace.LinkSystem.ProcessLinkSocketClick(linkSocket);
        }

        private void UnlinkSocketIfNeeded() {
            //if (Field != null && Field.Value != NodeId.None) {
            MasterNode?.Workspace?.LinkSystem?.UnlinkSocket(linkSocket);
            //}
        }
        
        // private void ProcessSocketLinked(INodeLinkSocket socket, NodeId nodeId) {
        //     Field.SetId(nodeId);
        // }
        
        private void ProcessLinkedNodeIdChanged(NodeIdDataField dataField) {
            //if (linkSocket.LinkedToId != dataField.Value) {
            UnlinkSocketIfNeeded();
            MasterNode.Workspace.LinkSystem.LinkSocketToNode(linkSocket, dataField.Value);
            //}
        }
    }
    
}