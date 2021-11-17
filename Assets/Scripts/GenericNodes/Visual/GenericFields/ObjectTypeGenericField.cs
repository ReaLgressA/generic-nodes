using System.Collections.Generic;
using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using TMPro;
using UnityEngine;

namespace GenericNodes.Visual.GenericFields {
    public class ObjectTypeGenericField : MonoBehaviour,
                                          IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private RectTransform rtrContentRoot;

        public ObjectTypeDataField Field { get; private set; }

        private void Awake() {
            dropdown.onValueChanged.AddListener(ProcessDropdownSelection);
        }

        public void SetData(ObjectTypeDataField field) {
            Field = field;
            textLabel.text = Field.DisplayName;
            dropdown.ClearOptions();
            dropdown.AddOptions(new List<string>(field.AllowedTypes));
            dropdown.SetValueWithoutNotify(field.SelectedIndex);
            RefreshContentVisibility();
        }

        public void SetData(NodeVisual nodeVisual, DataField data, IGenericFieldParent fieldParent) {
            SetData(data as ObjectTypeDataField);
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