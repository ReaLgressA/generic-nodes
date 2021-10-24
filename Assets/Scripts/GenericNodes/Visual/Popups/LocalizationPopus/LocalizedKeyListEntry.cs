using System;
using System.Security.Cryptography;
using GenericNodes.Visual.Views;
using L10n;
using TMPro;
using UnityEngine;

namespace GenericNodes.Visual.Popups {
    namespace GenericNodes.Visual.Views {
        public class LocalizedKeyListEntry : ClickableEntry {
            [SerializeField] private TextMeshProUGUI textContent;
            
            public event Action<LocalizedKeyListEntry> EventSelectKey;
            public event Action<LocalizedKeyListEntry> EventApplyKey;

            public string Key { get; private set; }
            
            public void Setup(string key, RectTransform rtrRoot) {
                Reset();
                Key = key;
                transform.SetParent(rtrRoot);
                transform.localScale = Vector3.one;
                gameObject.SetActive(true);
                RefreshText();
            }

            public override void Reset() {
                base.Reset();
                EventSelectKey = null;
                EventApplyKey = null;
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
                if (!gameObject.activeSelf) {
                    return;
                }
                textContent.text = $"<b>{Key}</b> | {L10N.Translate(Key)}";
            }
    
            private void ProcessClick() {
                if (IsSelected) {
                    EventApplyKey?.Invoke(this);
                } else {
                    EventSelectKey?.Invoke(this);
                }
            }
        }
    }
}