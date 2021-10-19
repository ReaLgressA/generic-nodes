using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Views {
    public class FileViewEntry : ClickableEntry,
                                 IFilePathEntry {
        [SerializeField] 
        private TextMeshProUGUI textFileName;
        [SerializeField] 
        private Image iconFileExtension;

        private string filePath;

        public event Action<IFilePathEntry> SelectEntry;
        public event Action<string> OpenFile;

        public string FilePath => filePath;
        
        public bool IsDirectory => false;
        public string Path => FilePath;
        
        public FileViewEntry Setup(string filePath, string fileName, string extension, RectTransform rtrRoot) {
            Reset();
            this.filePath = filePath;
            textFileName.text = fileName;
            iconFileExtension.enabled = false; //TODO: add & show extensions icon
            transform.SetParent(rtrRoot);
            transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            return this;
        }

        private void Awake() {
            Click += ProcessClick;
        }

        private void OnDestroy() {
            Click -= ProcessClick;
        }

        private void ProcessClick() {
            if (IsSelected) {
                OpenFile?.Invoke(filePath);
            } else {
                SelectEntry?.Invoke(this);
            }
        }
    }
}