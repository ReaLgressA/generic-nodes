using System.Collections.Generic;
using GenericNodes.Mech.Data;
using GenericNodes.Visual.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Nodes
{
    [RequireComponent(typeof(RectTransform))]
    public class NodeVisual : DraggableEventTrigger, 
                              IHoldable {
        
        [SerializeField] private Image imageBackground;
        [SerializeField] private TextMeshProUGUI textHeader;
        [SerializeField] private RectTransform rtrContentRoot;

        private RectTransform rTransform;
        private readonly List<NodeSocketVisual> inputSockets = new List<NodeSocketVisual>();
        
        private readonly List<IGenericField> fields = new List<IGenericField>();

        public DraggableEventTrigger EventTrigger => this;
        public HoldableType HoldableType => HoldableType.Node;
        public RectTransform Transform => rTransform;
        public NodeData Data { get; private set; } = null; 
        
        protected override void Awake() {
            base.Awake();
            rTransform = GetComponent<RectTransform>();
        }

        public void SetupData(NodeData data) {
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
                field.SetData(data.Fields[i]);
                fields.Add(field);
            }
        }

        private void ClearFields() {
            for (int i = 0; i < fields.Count; ++i) {
                fields[i].Destroy();
            }
            fields.Clear();
        }

        public void SetPosition(Vector2 position) {
            Transform.anchoredPosition = position;
        }
    }
}
