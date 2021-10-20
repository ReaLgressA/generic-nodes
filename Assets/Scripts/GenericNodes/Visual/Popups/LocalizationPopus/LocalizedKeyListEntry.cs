using System;
using GenericNodes.Visual.Views;
using L10n;
using TMPro;
using UnityEngine;

namespace GenericNodes.Visual.Popups {
    namespace GenericNodes.Visual.Views {
        public class LocalizedKeyListEntry : ClickableEntry {
            [SerializeField] private TextMeshProUGUI textContent;
            
            public event Action<LocalizedKeyListEntry> SelectKey;

            public string Key { get; private set; }
            
            public LocalizedKeyListEntry Setup(string key, RectTransform rtrRoot) {
                Reset();
                Key = key;
                transform.SetParent(rtrRoot);
                transform.localScale = Vector3.one;
                gameObject.SetActive(true);
                RefreshText();
                return this;
            }
    
            private void Awake() {
                Click += ProcessClick;
                L10N.EventLanguageChanged += RefreshText;
            }
    
            private void OnDestroy() {
                Click -= ProcessClick;
                L10N.EventLanguageChanged -= RefreshText;
            }

            private void RefreshText() {
                textContent.text = $"<b>@{Key}</b> | {L10N.Translate(Key)}";
            }
    
            private void ProcessClick() {
                if (IsSelected) {
                    //OpenFile?.Invoke(filePath);
                } else {
                    SelectKey?.Invoke(this);
                }
            }
        }
    }
}