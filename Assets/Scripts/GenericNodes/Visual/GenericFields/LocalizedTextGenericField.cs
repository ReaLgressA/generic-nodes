using System;
using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using GenericNodes.Visual.Popups;
using L10n;
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

        public LocalizedTextDataField Field { get; private set; }

        private void Awake() {
            inputFieldContent.lineType = TMP_InputField.LineType.MultiLineNewline;
            inputFieldContent.onEndEdit.AddListener(ProcessEndEdit);
            buttonSetKey.onClick.AddListener(ProcessSetKeyClick);
        }

        private void OnDestroy() {
            inputFieldContent.onEndEdit.RemoveAllListeners();
            buttonSetKey.onClick.RemoveAllListeners();
        }

        private void OnEnable() {
            L10N.EventLanguageChanged += RefreshInputFieldText;
            L10N.EventKeyTranslationChanged += ProcessEventKeyTranslationChange;
        }

        private void OnDisable() {
            L10N.EventLanguageChanged -= RefreshInputFieldText;
            L10N.EventKeyTranslationChanged -= ProcessEventKeyTranslationChange;
        }

        public void SetData(LocalizedTextDataField field) {
            Field = field;
            textLabel.text = Field.DisplayName;
            SetLocalizationKey(Field.Value);
        }

        public void SetLocalizationKey(string localizationKey) {
            Field.SetValue(localizationKey);
            textKey.text = string.IsNullOrWhiteSpace(Field.Value) ? "<b>NULL!</b>" : Field.Value;
            RefreshInputFieldText();
        }

        public void SetData(NodeVisual nodeVisual, DataField data, IGenericFieldParent fieldParent) {
            SetData(data as LocalizedTextDataField);
        }

        public void Destroy() {
            Field = null;
            GameObject.Destroy(gameObject);
        }
            
        public void RebuildLinks() { }
        public void ResetLinksIfTargetNodeNotExist() { }

        private void RefreshInputFieldText() {
            bool hasValidKey = L10N.DoesKeyExist(Field.Value);
            inputFieldContent.interactable = hasValidKey;
            inputFieldContent.SetTextWithoutNotify(hasValidKey ? L10N.Translate(Field.Value) : string.Empty);
        }
        
        private void ProcessEndEdit(string value) {
            Debug.Log($"End edit '{Field?.Name}' with value '{value}'");
            if (L10N.DoesKeyExist(Field.Value)) {
                L10N.SetKeyTranslation(Field.Value, value);
            }
        }
        
        private void ProcessSetKeyClick() {
            PopupManager.GetPopup<SelectLocalizationKeyPopup>().Show(this);
        }
        
        private void ProcessEventKeyTranslationChange(string localizationKey, string translation) {
            if (Field.Value.Equals(localizationKey, StringComparison.Ordinal)) {
                inputFieldContent.SetTextWithoutNotify(translation);
            }
        }
    }
}