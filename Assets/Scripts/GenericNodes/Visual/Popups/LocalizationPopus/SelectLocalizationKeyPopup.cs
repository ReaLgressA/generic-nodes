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

        private LocalizedKeyListEntry CurrentlySelectedKey { get; set; } = null;
        private LocalizedTextGenericField Field { get; set; }
        
        private void Awake() {
            buttonApply.onClick.AddListener(ProcessApplyButtonClick);
            buttonNewKey.onClick.AddListener(ProcessNewKeyButtonClick);
            buttonClose.onClick.AddListener(ProcessCloseButtonClick);
            
            L10N.EventLanguageChanged += UpdateActiveLanguage;
            buttonChangeLanguage.onClick.AddListener(ChangeLanguage);
            textInputSearch.onValueChanged.AddListener(RefreshSearchPattern);
        }

        private void OnDestroy() {
            buttonApply.onClick.RemoveAllListeners();
            buttonNewKey.onClick.RemoveAllListeners();
            buttonClose.onClick.RemoveAllListeners();
            
            L10N.EventLanguageChanged -= UpdateActiveLanguage;
            buttonChangeLanguage.onClick.RemoveAllListeners();
            textInputSearch.onValueChanged.RemoveAllListeners();
        }

        public void Show(LocalizedTextGenericField localizedTextGenericField) {
            Field = localizedTextGenericField;
            gameObject.SetActive(true);
            Refresh();
            SelectL10nKeyEntry(null);
        }
        
        public void Hide() {
            gameObject.SetActive(false);
            ResetList();
        }

        public void Refresh() {
            ResetList();
            L10N.BuildKeyList(ref localizedKeys);
            for (int i = 0; i < localizedKeys.Count; ++i) {
                if (i >= keyEntries.Count) {
                    SpawnExtraKeyEntry();
                }
                keyEntries[i].Setup(localizedKeys[i].LocalizationKey, rtrListRoot);
                keyEntries[i].EventSelectKey += SelectL10nKeyEntry;
                keyEntries[i].EventApplyKey += ApplyL10nKeyEntry;
            }
            RefreshSearchPattern(textInputSearch.text);
        }
        
        private void RefreshSearchPattern(string searchPattern) {
            for (int i = 0; i < keyEntries.Count; ++i) {
                keyEntries[i].ApplySearchPattern(searchPattern);
            }
        }

        private void SpawnExtraKeyEntry() {
            LocalizedKeyListEntry extraEntry = Instantiate(keyEntries[0], rtrListRoot);
            keyEntries.Add(extraEntry);
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
            Field.SetLocalizationKey(CurrentlySelectedKey?.Key);
            Hide();
        }
        
        private void ChangeLanguage() {
            int currentIndex = Mathf.Max(0, L10N.Config.Languages.FindIndex(data => data.Id.Equals(L10N.ActiveLanguageId)));
            if (++currentIndex >= L10N.Config.Languages.Count) {
                currentIndex = 0;
            }
            L10N.SetActiveLanguage(L10N.Config.Languages[currentIndex].Id);
        }
        
        private void UpdateActiveLanguage() {
            buttonChangeLanguage.gameObject.SetActive(true);
            textButtonChangeLanguage.text = L10N.ActiveLanguageId;
        }

        private void ApplyL10nKeyEntry(LocalizedKeyListEntry keyEntry) {
            if (CurrentlySelectedKey == keyEntry) {
                ProcessApplyButtonClick();
            }
        }
        
        private void SelectL10nKeyEntry(LocalizedKeyListEntry keyEntry) {
            CurrentlySelectedKey?.SetSelected(false);
            CurrentlySelectedKey = keyEntry;
            keyEntry?.SetSelected(true);
            RefreshUserSelectionStatus();
        }

        private void RefreshUserSelectionStatus() {
            buttonEditKey.interactable = CurrentlySelectedKey != null;
            buttonApply.interactable = CurrentlySelectedKey != null;
            textSelectedKey.text = CurrentlySelectedKey == null ? "NONE!" : CurrentlySelectedKey.Key;
        }
    }
}