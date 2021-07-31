using System.Collections.Generic;
using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using TMPro;
using UnityEngine;

namespace GenericNodes.Visual.GenericFields {
    public class EnumGenericField : MonoBehaviour, IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private TMP_Dropdown dropdown;

        public EnumDataField Field { get; private set; }

        private void Awake() {
            dropdown.onValueChanged.AddListener(ProcessDropdownSelection);
        }

        public void SetData(EnumDataField field) {
            Field = field;
            textLabel.text = Field.Name;
            dropdown.ClearOptions();
            dropdown.AddOptions(new List<string>(field.EnumDescription.Enumeration));
            dropdown.SetValueWithoutNotify(field.SelectedIndex);
        }

        public void SetData(NodeVisual nodeVisual, DataField data, IGenericFieldParent fieldParent) {
            SetData(data as EnumDataField);
        }

        public void Destroy() {
            Field = null;
            GameObject.Destroy(gameObject);
        }
        
        public void RebuildLinks() { }
        public void ResetLinksIfTargetNodeNotExist() { }
        
        private void ProcessDropdownSelection(int selectedIndex) {
            Debug.Log($"End edit '{Field?.Name}' with value '{selectedIndex}'");
            Field?.SetValue(selectedIndex);
        }
    }
}