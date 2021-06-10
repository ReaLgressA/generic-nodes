using Mech.Fields;
using TMPro;
using UnityEngine;

namespace Visual.GenericFields {
    public class TextGenericField : MonoBehaviour, IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private TMP_InputField inputFieldContent;

        public TextDataField Field { get; private set; }

        private void Awake() {
            inputFieldContent.lineType = TMP_InputField.LineType.MultiLineNewline;
            inputFieldContent.onEndEdit.AddListener(ProcessEndEdit);
        }

        public void SetData(TextDataField field) {
            Field = field;
            textLabel.text = Field.Name;
            inputFieldContent.text = Field.Value;
        }

        public void SetData(DataField data) {
            SetData(data as TextDataField);
        }

        public void Destroy() {
            Field = null;
            GameObject.Destroy(gameObject);
        }
        
        private void ProcessEndEdit(string value) {
            Debug.Log($"End edit '{Field?.Name}' with value '{value}'");
            Field?.SetValue(value);
        }
    }
}