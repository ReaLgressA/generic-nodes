using System.Collections.Generic;
using Mech.Fields;
using TMPro;
using UnityEngine;

namespace Visual.GenericFields {
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

        public void SetData(DataField data) {
            SetData(data as EnumDataField);
        }

        public void Destroy() {
            Field = null;
            GameObject.Destroy(gameObject);
        }
        
        private void ProcessDropdownSelection(int selectedIndex) {
            Debug.Log($"End edit '{Field?.Name}' with value '{selectedIndex}'");
            Field?.SetValue(selectedIndex);
        }
    }
}