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
    public class CustomArrayGenericField : MonoBehaviour,
                                           IGenericFieldParent,
                                           IGenericField {
        [SerializeField] 
        private TextMeshProUGUI textLabel;
        [SerializeField]
        private RectTransform rtrArrayElementsRoot;
        [SerializeField]
        private Button buttonAddElement;
        [SerializeField]
        private Button buttonRemoveElement;

        private RectTransform rtrRoot;
        private readonly List<CustomObjectGenericField> arrayElements = new List<CustomObjectGenericField>();
        
        private NodeVisual MasterNode { get; set; }
        public AbstractArrayDataField Field { get; private set; }
        public NodeId NodeId => MasterNode.NodeId;
        public Vector2 ParentPositionShift => RtrRoot.anchoredPosition + rtrArrayElementsRoot.anchoredPosition 
                                                                       + Parent.ParentPositionShift;
        public IGenericFieldParent Parent { get; private set; }
        private RectTransform RtrRoot => rtrRoot ??= GetComponent<RectTransform>();

        private void Awake() {
            buttonAddElement.onClick.AddListener(AddNewElement);
            buttonRemoveElement.onClick.AddListener(RemoveLastElement);
        }

        private void OnDestroy() {
            buttonAddElement.onClick.RemoveAllListeners();
            buttonRemoveElement.onClick.RemoveAllListeners();
        }

        public void SetData(AbstractArrayDataField data) {
            Field = data;
            textLabel.text = Field.DisplayName;
            Field.ElementsUpdated += RefreshElementsList;
            RefreshElementsList();
        }

        public void SetData(NodeVisual nodeVisual, DataField data, IGenericFieldParent fieldParent) {
            MasterNode = nodeVisual;
            Parent = fieldParent;
            SetData(data as AbstractArrayDataField);
            RefreshElementsList();
        }

        public void Destroy() {
            Field.ElementsUpdated -= RefreshElementsList;
            Field = null;
            GameObject.Destroy(gameObject);
        }
        
        public void RebuildLinks() {
            for (int i = 0; i < Field.Elements.Count; ++i) {
                arrayElements[i].RebuildLinks();
            }
        }

        public void ResetLinksIfTargetNodeNotExist() {
            for (int i = 0; i < Field.Elements.Count; ++i) {
                arrayElements[i].ResetLinksIfTargetNodeNotExist();
            }
        }
        
        private void AddNewElement() {
            Field.AddElement();
        }
        
        private void RemoveLastElement() {
            Field.RemoveElementAt(Field.Elements.Count - 1);
        }
        
        private void RefreshElementsList() {
            ResetElements();
            while (arrayElements.Count < Field.Elements.Count) {
                SpawnExtraArrayElement();
            }
            for (int i = 0; i < Field.Elements.Count; ++i) {
                Field.Elements[i].Name = $"#{i}";
                
                arrayElements[i].SetData(MasterNode, Field.Elements[i], this);
                arrayElements[i].gameObject.SetActive(true);
            }
            RebuildLinks();
        }

        private void ResetElements() {
            for (int i = 0; i < arrayElements.Count; ++i) {
                arrayElements[i].gameObject.SetActive(false);
            }
        }

        private void SpawnExtraArrayElement() {
            GameObject goField = Instantiate(PrefabDatabase.GetFieldPrefab(DataType.CustomObject));
            RectTransform rtr = goField.GetComponent<RectTransform>();
            rtr.SetParent(rtrArrayElementsRoot);
            rtr.localScale = Vector3.one;
            rtr.SetAsLastSibling();
            arrayElements.Add(goField.GetComponent<CustomObjectGenericField>());
        }
    }
}