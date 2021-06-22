using System.Collections.Generic;
using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.GenericFields {
    public class CustomArrayGenericField : MonoBehaviour, IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private RectTransform rtrArrayElementsRoot;
        [SerializeField] private Button buttonAddElement;
        [SerializeField] private Button buttonRemoveElement;

        private List<CustomObjectDataField> arrayElements = new List<CustomObjectDataField>();
        
        public GenericArrayDataField Field { get; private set; }

        private void Awake() {
            buttonAddElement.onClick.AddListener(AddNewElement);
            buttonRemoveElement.onClick.AddListener(RemoveLastElement);
        }

        private void OnDestroy() {
            buttonAddElement.onClick.RemoveAllListeners();
            buttonRemoveElement.onClick.RemoveAllListeners();
        }

        public void SetData(GenericArrayDataField data) {
            Field = data;
            textLabel.text = Field.Name;
        }
        
        public void SetData(NodeVisual nodeVisual, DataField data, IGenericFieldParent fieldParent) {
            SetData(data as GenericArrayDataField);
        }

        public void Destroy() {
            Field = null;
            GameObject.Destroy(gameObject);
        }
        
        private void AddNewElement() {
            
        }
        
        private void RemoveLastElement() {
            
        }
    }
}