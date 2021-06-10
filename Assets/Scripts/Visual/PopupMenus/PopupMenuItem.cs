using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Visual.PopupMenus {
    public class PopupMenuItem : MonoBehaviour {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI textName;

        private Action itemAction = null;

        public virtual void Initilize(string name, Action action) {
            textName.text = name;
            itemAction = action;
        }
        
        private void Awake() {
            button.onClick.AddListener(ProcessClick);
        }

        private void OnDestroy() {
            button.onClick.RemoveAllListeners();
        }
        
        private void ProcessClick() {
            itemAction?.Invoke();   
        }
    }
}