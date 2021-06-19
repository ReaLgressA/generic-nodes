using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using TMPro;
using UnityEngine;

namespace GenericNodes.Visual.GenericFields {
    public class IntGenericField : MonoBehaviour, IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private TMP_InputField inputFieldContent;

        public IntDataField Field { get; private set; }

        private void Awake() {
            inputFieldContent.contentType = TMP_InputField.ContentType.IntegerNumber;
            inputFieldContent.onEndEdit.AddListener(ProcessEndEdit);
        }

        public void SetData(IntDataField field) {
            Field = field;
            textLabel.text = Field.Name;
            inputFieldContent.text = Field.Value.ToString();
        }

        public void SetData(NodeVisual nodeVisual, DataField data) {
            SetData(data as IntDataField);
        }

        public void Destroy() {
            Field = null;
            GameObject.Destroy(gameObject);
        }
        
        private void ProcessEndEdit(string value) {
            Debug.Log($"End edit '{Field?.Name}' with value '{value}'");
            Field?.SetValue(int.TryParse(value, out int intValue) ? intValue : 0);
        }
    }
}