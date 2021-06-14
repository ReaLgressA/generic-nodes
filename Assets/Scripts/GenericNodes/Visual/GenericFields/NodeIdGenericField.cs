using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.GenericFields {
    
    public class NodeIdGenericField : MonoBehaviour, IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private Image imageLinkSlot;
        [SerializeField] private Button buttonLink;
        
        
        public NodeIdDataField Field { get; private set; }
        
        private void Awake() {
            buttonLink.onClick.AddListener(ProcessLinkButtonClick);    
        }

        public void SetData(NodeIdDataField field) {
            Field = field;
            textLabel.text = Field.Name;
        }

        public void SetData(DataField data) {
            SetData(data as NodeIdDataField);
        }

        public void Destroy() {
            Field = null;
            GameObject.Destroy(gameObject);
        }
    
        private void ProcessLinkButtonClick() {
            
            //Debug.Log($"End edit '{Field?.Name}' with value '{value}'");
            //Field?.SetValue(int.TryParse(value, out int intValue) ? intValue : 0);
        }
    }
    
}