using GenericNodes.Visual.GenericFields;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Popups {
    public class SelectLocalizationKeyPopup : MonoBehaviour {
        [SerializeField] private RectTransform rtrListRoot;
        [SerializeField] private Button buttonClose;
        [SerializeField] private Button buttonApply;
        [SerializeField] private Button buttonNewKey;
        [SerializeField] private TMP_InputField textInputSearch;
        [SerializeField] private TextMeshProUGUI textSelectedKey;

        private void Awake() {
            buttonApply.onClick.AddListener(ProcessApplyButtonClick);
            buttonNewKey.onClick.AddListener(ProcessNewKeyButtonClick);
            buttonClose.onClick.AddListener(ProcessCloseButtonClick);
        }

        private void OnDestroy() {
            buttonApply.onClick.RemoveAllListeners();
            buttonNewKey.onClick.RemoveAllListeners();
            buttonClose.onClick.RemoveAllListeners();
        }

        public void Show(LocalizedTextGenericField localizedTextGenericField) {
            gameObject.SetActive(true);
        }
        
        public void Hide() {
            gameObject.SetActive(false);
        }
        
        private void ProcessNewKeyButtonClick() {
            
        }

        private void ProcessCloseButtonClick() {
            Hide();
        }

        private void ProcessApplyButtonClick() {
            
        }
    }
}