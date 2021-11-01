using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.GenericFields {
    public class TextGenericField : MonoBehaviour, IGenericField {
        [SerializeField]
        private TextMeshProUGUI textLabel;
        [SerializeField]
        private TMP_InputField inputFieldContent;
        [SerializeField]
        private Toggle toggleIsOptional;
        [SerializeField]
        private RectTransform rtrContentRoot;
        
        public TextDataField Field { get; private set; }

        private void Awake() {
            inputFieldContent.lineType = TMP_InputField.LineType.MultiLineNewline;
            inputFieldContent.onEndEdit.AddListener(ProcessEndEdit);
            toggleIsOptional.onValueChanged.AddListener(ProcessIsOptionAllowedValueUpdate);
        }

        public void SetData(TextDataField field) {
            Field = field;
            textLabel.text = Field.DisplayName;
            inputFieldContent.text = Field.Value;
            toggleIsOptional.gameObject.SetActive(field.IsOptional);
            if (field.IsOptional) {
                toggleIsOptional.SetIsOnWithoutNotify(field.IsOptionAllowed);    
            }
            RefreshContentVisibility();
        }

        public void SetData(NodeVisual nodeVisual, DataField data, IGenericFieldParent fieldParent) {
            SetData(data as TextDataField);
        }

        public void Destroy() {
            Field = null;
            GameObject.Destroy(gameObject);
        }
        
        public void RebuildLinks() { }
        public void ResetLinksIfTargetNodeNotExist() { }
        
        private void ProcessEndEdit(string value) {
            Debug.Log($"End edit '{Field?.DisplayName}' with value '{value}'");
            Field?.SetValue(value);
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