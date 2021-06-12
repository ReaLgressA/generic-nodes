using Mech.Fields;
using TMPro;
using UnityEngine;

namespace Visual.GenericFields {
    public class StringGenericField : MonoBehaviour, IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private TMP_InputField inputFieldContent;

        public StringDataField Field { get; private set; }

        private void Awake() {
            inputFieldContent.onEndEdit.AddListener(ProcessEndEdit);
        }

        public void SetData(StringDataField field) {
            Field = field;
            textLabel.text = Field.Name;
            inputFieldContent.text = Field.Value;
        }

        public void SetData(DataField data) {
            SetData(data as StringDataField);
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