using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VaultKeeper.Data.PackageContent;

namespace GenericNodes.Visual.AdditionalViews {
    public class SpriteAssetEntry : MonoBehaviour {
        [SerializeField] private Image imageBackground;
        [SerializeField] private Image imagePreview;
        [SerializeField] private TextMeshProUGUI textName;
        [SerializeField] private Button buttonPreview;
        [SerializeField] private Color colorBackgroundSelected;
        [SerializeField] private Color colorBackgroundNormal;
        
        private bool isSelected = false;

        public int Index { get; set; } = -1;
        public VaultPackageContentSprites.SpriteSettings SpriteSettings { get; private set; } = null;

        public event Action<SpriteAssetEntry> Selected;

        private void Awake() {
            buttonPreview.onClick.AddListener(SelectEntry);
        }

        public void Setup(int index, VaultPackageContentSprites.SpriteSettings spriteSettings) {
            Index = index;
            SpriteSettings = spriteSettings;
            imagePreview.sprite = SpriteSettings.sprite;
            textName.text = SpriteSettings.id;
            gameObject.SetActive(true);
        }
        
        public void Reset() {
            Index = -1;
            SpriteSettings = null;
            imagePreview.sprite = null;
            textName.text = null;
        }
        
        public void SetSelected(bool isSelected) {
            this.isSelected = isSelected;
            imageBackground.color = isSelected ? colorBackgroundSelected : colorBackgroundNormal;
        }
        
        private void SelectEntry() {
            Selected?.Invoke(this);
            SetSelected(true);
        }
    }
}