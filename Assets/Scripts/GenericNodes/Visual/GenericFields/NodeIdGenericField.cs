using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.GenericFields {
    
    public class NodeIdGenericField : MonoBehaviour, IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [SerializeField] private NodeSocketVisual linkSocket;
        [SerializeField] private Button buttonLink;
        
        private RectTransform rectTransform;

        public NodeIdDataField Field { get; private set; }
        public NodeVisual MasterNode { get; private set; }

        public RectTransform Transform => rectTransform ??= GetComponent<RectTransform>();
        
        private void Awake() {
            buttonLink.onClick.AddListener(ProcessLinkButtonClick);
        }

        public void SetData(NodeIdDataField field) {
            Field = field;
            textLabel.text = Field.Name;
            linkSocket.Initialize(this);
        }

        public void SetData(NodeVisual nodeVisual, DataField data) {
            MasterNode = nodeVisual;
            SetData(data as NodeIdDataField);
        }
        
        

        public void Destroy() {
            Field = null;
            GameObject.Destroy(gameObject);
        }
    
        private void ProcessLinkButtonClick() {
            MasterNode.Workspace.LinkSystem.ProcessLinkSocketClick(linkSocket);
        }
    }
    
}