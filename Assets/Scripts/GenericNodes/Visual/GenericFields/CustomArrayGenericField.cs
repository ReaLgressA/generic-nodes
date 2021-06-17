using System;
using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using TMPro;
using UnityEngine;

namespace GenericNodes.Visual.GenericFields {
    public class CustomArrayGenericField : MonoBehaviour, IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        
        private void Awake() {
            
        }

        public GenericArrayDataField Field { get; private set; }

        public void SetData(GenericArrayDataField data) {
            Field = data;
            textLabel.text = Field.Name;
        }
        
        public void SetData(DataField data) {
            SetData(data as GenericArrayDataField);
        }

        public void Destroy() {
            Field = null;
            GameObject.Destroy(gameObject);
        }
    }
}