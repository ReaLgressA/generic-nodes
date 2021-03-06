using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using TMPro;
using UnityEngine;

namespace GenericNodes.Visual.GenericFields {
    public class FloatGenericField : MonoBehaviour,
                                     IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private TMP_InputField inputFieldContent;

        public FloatDataField Field { get; private set; }

        private void Awake() {
            inputFieldContent.contentType = TMP_InputField.ContentType.DecimalNumber;
            inputFieldContent.onEndEdit.AddListener(ProcessEndEdit);
        }

        public void SetData(FloatDataField field) {
            Field = field;
            textLabel.text = Field.DisplayName;
            inputFieldContent.text = Field.Value.ToString();
        }

        public void SetData(NodeVisual nodeVisual, DataField data, IGenericFieldParent fieldParent) {
            SetData(data as FloatDataField);
        }

        public void Destroy() {
            Field = null;
            GameObject.Destroy(gameObject);
        }
        
        public void RebuildLinks() { }
        public void ResetLinksIfTargetNodeNotExist() { }
        
        private void ProcessEndEdit(string value) {
            Debug.Log($"End edit '{Field?.Name}' with value '{value}'");
            Field?.SetValue(float.TryParse(value, out float floatValue) ? floatValue : 0f);
        }
    }
}