using System;
using System.Collections.Generic;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.Fields;
using GenericNodes.Utility;
using GenericNodes.Visual.AdditionalViews;
using UnityEngine;
using UnityEngine.UI;
using VaultKeeper.Data.PackageContent;

namespace GenericNodes.Visual.Popups {
    public class SelectSpriteAssetPopup : MonoBehaviour {
        [SerializeField] private SpriteAssetEntry prefabSpriteEntry;
        [SerializeField] private RectTransform rtrSpriteEntriesRoot;
        [SerializeField] private Button buttonClose;
        [SerializeField] private Button buttonApply;

        private List<SpriteAssetEntry> entries = new List<SpriteAssetEntry>();
        private PrefabPool<SpriteAssetEntry> poolSpriteEntries;
        private readonly List<VaultPackageContentSprites.SpriteSettings> spriteSettingsList 
            = new List<VaultPackageContentSprites.SpriteSettings>();
        
        public VaultProvider VaultProvider { get; set; }

        private int SelectedEntryIndex { get; set; } = -1;
        private SpriteAssetDataField Field { get; set; }

        private void Awake() {
            poolSpriteEntries = new PrefabPool<SpriteAssetEntry>(prefabSpriteEntry, rtrSpriteEntriesRoot, 0);
            buttonClose.onClick.AddListener(Hide);
            buttonApply.onClick.AddListener(ApplySelected);
            Hide(); //Hide on first start
        }

        private void OnDestroy() {
            buttonClose.onClick.RemoveAllListeners();
            buttonApply.onClick.RemoveAllListeners();
        }

        public void Show(SpriteAssetDataField field) {
            Field = field;
            gameObject.SetActive(true);
            VaultProvider.GetSprites(Field.PackageLabel, spriteSettingsList);
            poolSpriteEntries.ReleaseAll();
            for (int i = 0; i < entries.Count; ++i) {
                entries[i].Reset();
                entries[i].Selected -= ProcessEntrySelection;
            }
            entries.Clear();
            SelectedEntryIndex = -1;
            if (!string.IsNullOrWhiteSpace(Field.Value)) {
                for (int i = 0; i < spriteSettingsList.Count; ++i) {
                    if (Field.Value.Equals(spriteSettingsList[i].id, StringComparison.Ordinal)) {
                        SelectedEntryIndex = i;
                        break;
                    }
                }
            }
            for (int i = 0; i < spriteSettingsList.Count; ++i) {
                SpriteAssetEntry spriteAssetEntry = poolSpriteEntries.Request();
                spriteAssetEntry.Setup(i, spriteSettingsList[i]);
                spriteAssetEntry.SetSelected(i == SelectedEntryIndex);
                spriteAssetEntry.Selected += ProcessEntrySelection;
                entries.Add(spriteAssetEntry);    
            }
        }

        private void ProcessEntrySelection(SpriteAssetEntry entry) {
            if (SelectedEntryIndex != -1) {
                entries[SelectedEntryIndex].SetSelected(false);
            }
            SelectedEntryIndex = entry.Index;
        }

        public void Hide() {
            Field = null;
            gameObject.SetActive(false);
        }
        
        private void ApplySelected() {
            if (SelectedEntryIndex == -1) {
                Field.SetValue(null);
            } else {
                Field.SetValue(spriteSettingsList[SelectedEntryIndex].id);
            }
            Hide();
        }
    }
}