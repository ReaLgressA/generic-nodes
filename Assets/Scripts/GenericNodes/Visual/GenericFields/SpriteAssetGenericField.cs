using GenericNodes.Mech.Fields;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using GenericNodes.Visual.Popups;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GenericNodes.Visual.GenericFields {
    public class SpriteAssetGenericField : MonoBehaviour,
                                           IGenericField {
        [SerializeField] private TextMeshProUGUI textLabel;
        [FormerlySerializedAs("textAssetPath")] [SerializeField] private TextMeshProUGUI textAssetId;
        [SerializeField] private Image imagePreview;
        [SerializeField] private Button buttonSelectAsset;

        public SpriteAssetDataField Field { get; private set; }

        private void Awake() {
            buttonSelectAsset.onClick.AddListener(SelectSpriteAsset);
        }

        private void OnDestroy() {
            buttonSelectAsset.onClick.RemoveAllListeners();
        }

        public void SetData(SpriteAssetDataField field) {
            if (Field != null) {
                Field.ValueChanged -= RefreshPreview;
            }
            Field = field;
            if (Field != null) {
                Field.ValueChanged += RefreshPreview;
                textLabel.text = Field.DisplayName;
                textAssetId.text = Field.Value;
                RefreshPreview();
            }
        }

        public void SetData(NodeVisual nodeVisual, DataField data, IGenericFieldParent fieldParent) {
            SetData(data as SpriteAssetDataField);
        }

        public void Destroy() {
            SetData(null);
            GameObject.Destroy(gameObject);
        }
        
        public void RebuildLinks() { }
        public void ResetLinksIfTargetNodeNotExist() { }

        private void SelectSpriteAsset() {
            PopupManager.GetPopup<SelectSpriteAssetPopup>().Show(Field);
        }
        
        private void RefreshPreview() {
            var vaultProvider = PopupManager.GetPopup<SelectSpriteAssetPopup>().VaultProvider;
            var spriteSettings = vaultProvider.GetSprite(Field.Value);
            imagePreview.sprite = spriteSettings?.sprite;
            textAssetId.text = spriteSettings?.id ?? Field.PackageLabel;
        }
    }
}