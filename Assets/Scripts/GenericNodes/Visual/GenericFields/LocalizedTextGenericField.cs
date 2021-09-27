using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.GenericFields {
    public class LocalizedTextGenericField : MonoBehaviour,
                                             IGenericField {
        [SerializeField] private Button buttonSetKey;
        [SerializeField] private TextMeshProUGUI textKey;
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
            Debug.Log($"End edit '{Field?.Name}' with value '{value}'");
            Field?.SetValue(value);
        }
    }
}