using System;
using TMPro;
using UnityEngine;

namespace GenericNodes.Visual.Views {
    public class DirectoryViewEntry : ClickableEntry,
                                      IFilePathEntry {
        [SerializeField] private TextMeshProUGUI textFileName;

        private string directoryPath;

        public event Action<IFilePathEntry> SelectEntry;
        public event Action<string> OpenDirectory;

        public string DirectoryPath => directoryPath;
        
        public bool IsDirectory => true;
        public string Path => DirectoryPath;
        
        public DirectoryViewEntry Setup(string directoryPath, string directoryName, RectTransform rtrRoot) {
            Reset();
            this.directoryPath = directoryPath;
            textFileName.text = directoryName;
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
                OpenDirectory?.Invoke(directoryPath);
            } else {
                SelectEntry?.Invoke(this);
            }
        }
    }
}