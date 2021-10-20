using System.Collections.Generic;
using GenericNodes.Visual.GenericFields;
using GenericNodes.Visual.Popups.GenericNodes.Visual.Views;
using L10n;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Popups {
    public class SelectLocalizationKeyPopup : MonoBehaviour {
        [SerializeField] private RectTransform rtrListRoot;
        [SerializeField] private Button buttonClose;
        [SerializeField] private Button buttonApply;
        [SerializeField] private Button buttonNewKey;
        [SerializeField] private Button buttonEditKey;
        [SerializeField] private Button buttonChangeLanguage;
        [SerializeField] private TextMeshProUGUI textButtonChangeLanguage;
        [SerializeField] private TMP_InputField textInputSearch;
        [SerializeField] private TextMeshProUGUI textSelectedKey;

        [SerializeField] private CreateLocalizationKeyPopup popupCreateKey;

        [SerializeField] private List<LocalizedKeyListEntry> keyEntries = new List<LocalizedKeyListEntry>();

        private List<LocalizedKeyDescription> localizedKeys = new List<LocalizedKeyDescription>();
        
        private void Awake() {
            buttonApply.onClick.AddListener(ProcessApplyButtonClick);
            buttonNewKey.onClick.AddListener(ProcessNewKeyButtonClick);
            buttonClose.onClick.AddListener(ProcessCloseButtonClick);
            
            L10N.EventLanguageChanged += UpdateActiveLanguage;
            buttonChangeLanguage.onClick.AddListener(ChangeLanguage);
        }

        private void OnDestroy() {
            buttonApply.onClick.RemoveAllListeners();
            buttonNewKey.onClick.RemoveAllListeners();
            buttonClose.onClick.RemoveAllListeners();
            
            L10N.EventLanguageChanged -= UpdateActiveLanguage;
            buttonChangeLanguage.onClick.RemoveAllListeners();
        }

        public void Show(LocalizedTextGenericField localizedTextGenericField) {
            gameObject.SetActive(true);
            
            Refresh();
        }
        
        public void Hide() {
            gameObject.SetActive(false);
            ResetList();
        }

        public void Refresh() {
            ResetList();
            L10N.BuildKeyList(ref localizedKeys);
        }

        private void ResetList() {
            for (int i = 0; i < keyEntries.Count; ++i) {
                keyEntries[i].Reset();
                keyEntries[i].gameObject.SetActive(false);
            }
        }

        private void ProcessNewKeyButtonClick() {
            popupCreateKey.Show(this);
        }

        private void ProcessCloseButtonClick() {
            Hide();
        }

        private void ProcessApplyButtonClick() {
            
        }
        
        private void ChangeLanguage() {
            int currentIndex = Mathf.Max(0, L10N.Config.Languages.FindIndex(data => data.Id.Equals(L10N.ActiveLanguage)));
            if (++currentIndex >= L10N.Config.Languages.Count) {
                currentIndex = 0;
            }
            L10N.SetActiveLanguage(L10N.Config.Languages[currentIndex].Id);
        }
        
        private void UpdateActiveLanguage() {
            buttonChangeLanguage.gameObject.SetActive(true);
            textButtonChangeLanguage.text = L10N.ActiveLanguage;
        }
    }
}