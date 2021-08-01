using System;
using System.Collections.Generic;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.GenericFields {
    public class CustomObjectGenericField : MonoBehaviour,
                                            IGenericFieldParent,
                                            IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private RectTransform rtrContentRoot;
        [SerializeField] private Toggle toggleIsOptional;
        
        private RectTransform rtrRoot;
        
        private readonly List<IGenericField> genericFields = new List<IGenericField>();
        
        public NodeVisual MasterNode { get; private set; }
        public CustomObjectDataField Field { get; private set; }
        public NodeId NodeId => MasterNode.NodeId;
        public Vector2 ParentPositionShift => RtrRoot.anchoredPosition + rtrContentRoot.anchoredPosition + Parent.ParentPositionShift;
        public IGenericFieldParent Parent { get; private set; }

        private RectTransform RtrRoot => rtrRoot ??= GetComponent<RectTransform>();

        private void Awake() {
            toggleIsOptional.onValueChanged.AddListener(ProcessIsOptionAllowedValueUpdate);
        }

        public void SetData(CustomObjectDataField field) {
            textLabel.text = field.Name;
            Field = field;
            ClearFields();
            toggleIsOptional.gameObject.SetActive(field.IsOptional);
            if (field.IsOptional) {
                toggleIsOptional.SetIsOnWithoutNotify(field.IsOptionAllowed);    
            }
            for (int i = 0; i < Field.Fields.Length; ++i) {
                DataField dataField = Field.Fields[i];
                GameObject goField = Instantiate(PrefabDatabase.GetFieldPrefab(dataField.Type));
                RectTransform rtr = goField.GetComponent<RectTransform>();
                rtr.SetParent(rtrContentRoot);
                rtr.localScale = Vector3.one;
                rtr.SetAsLastSibling();
                IGenericField objectField = goField.GetComponent<IGenericField>();
                objectField.SetData(MasterNode, dataField, this);
                genericFields.Add(objectField);
            }
            RefreshContentVisibility();
        }
        
        public void SetData(NodeVisual nodeVisual, DataField data, IGenericFieldParent fieldParent) {
            MasterNode = nodeVisual;
            Parent = fieldParent;
            SetData(data as CustomObjectDataField);
        }

        public void Destroy() {
            Field = null;
            Destroy(gameObject);
        }
        
        public void RebuildLinks() {
            for (int i = 0; i < genericFields.Count; ++i) {
                genericFields[i].RebuildLinks();
            }
        }

        public void ResetLinksIfTargetNodeNotExist() {
            for (int i = 0; i < genericFields.Count; ++i) {
                genericFields[i].ResetLinksIfTargetNodeNotExist();
            }
        }
        
        private void ClearFields() {
            for (int i = 0; i < genericFields.Count; ++i) {
                genericFields[i].Destroy();
            }
            genericFields.Clear();
        }
        
        private void ProcessIsOptionAllowedValueUpdate(bool isAllowed) {
            Field.IsOptionAllowed = isAllowed;
            RefreshContentVisibility();
        }

        private void RefreshContentVisibility() {
            bool isVisible = !Field.IsOptional || Field.IsOptionAllowed;
            rtrContentRoot.gameObject.SetActive(isVisible);
        }
    }
}