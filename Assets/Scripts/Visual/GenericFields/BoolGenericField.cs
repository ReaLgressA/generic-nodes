using Mech.Fields;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Visual.GenericFields {
    public class BoolGenericField : MonoBehaviour, IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private Toggle toggleCheckmark;

        public BoolDataField Field { get; private set; }

        private void Awake() {
            toggleCheckmark.onValueChanged.AddListener(ProcessToggle);
        }

        public void SetData(BoolDataField field) {
            Field = field;
            textLabel.text = Field.Name;
            toggleCheckmark.isOn = Field.Value;
        }

        public void SetData(DataField data) {
            SetData(data as BoolDataField);
        }

        public void Destroy() {
            Field = null;
            GameObject.Destroy(gameObject);
        }
        
        private void ProcessToggle(bool value) {
            Field?.SetValue(value);   
        }
    }
}