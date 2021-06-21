using System.Collections.Generic;
using GenericNodes.Mech.Data;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Links;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Nodes
{
    [RequireComponent(typeof(RectTransform))]
    public class NodeVisual : DraggableEventTrigger,
                              INodeLinkSocketProvider,
                              IGenericFieldParent,
                              IHoldable {
        
        [SerializeField] private Image imageBackground;
        [SerializeField] private TextMeshProUGUI textHeader;
        [SerializeField] private RectTransform rtrContentRoot;
        [SerializeField] private NodeSocketArray socketArray;
        
        private readonly List<IGenericField> fields = new List<IGenericField>();
        private RectTransform rTransform;

        public DraggableEventTrigger EventTrigger => this;
        public HoldableType HoldableType => HoldableType.Node;
        public RectTransform Transform => rTransform ??= GetComponent<RectTransform>();
        public NodeData Data { get; private set; } = null;
        public NodeId NodeId => Data.NodeId;
        public Vector2 ParentPositionShift => Transform.anchoredPosition;
        public IGenericFieldParent Parent => null;
        public WorkspaceArea Workspace { get; private set; }
        
        protected override void Awake() {
            base.Awake();
            socketArray.Initialize(this);
        }

        public void SetupData(WorkspaceArea workspaceArea, NodeData data) {
            Workspace = workspaceArea;
            Data = data;
            textHeader.text = data.NodeType;
            ClearFields();
            for (int i = 0; i < data.Fields.Count; ++i) {
                GameObject goField = Instantiate(PrefabDatabase.GetFieldPrefab(data.Fields[i].Type));
                RectTransform rtr = goField.GetComponent<RectTransform>();
                rtr.SetParent(rtrContentRoot);
                rtr.localScale = Vector3.one;
                rtr.SetAsLastSibling();
                IGenericField field = goField.GetComponent<IGenericField>();
                field.SetData(this, data.Fields[i], this);
                fields.Add(field);
            }
        }
        
        public void SetPosition(Vector2 position) {
            Transform.anchoredPosition = position;
        }

        public INodeLinkSocket GetLinkSocket() {
            return socketArray.GetFreeSocket();
        }

        private void ClearFields() {
            for (int i = 0; i < fields.Count; ++i) {
                fields[i].Destroy();
            }
            fields.Clear();
        }

        public void ReleaseSocket(INodeLinkSocket linkSocket) {
            socketArray.ReleaseSocket(linkSocket);
        }
    }
}
