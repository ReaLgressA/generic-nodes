using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.PopupMenus {
    public class PopupMenuItem : MonoBehaviour {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI textName;

        private string actionName;
        private Action<string> itemAction = null;

        public virtual void Initilize(string actionName, Action<string> action) {
            this.actionName = actionName;
            textName.text = this.actionName;
            itemAction = action;
        }
        
        private void Awake() {
            button.onClick.AddListener(ProcessClick);
        }

        private void OnDestroy() {
            button.onClick.RemoveAllListeners();
        }
        
        private void ProcessClick() {
            itemAction?.Invoke(actionName);   
        }
    }
}