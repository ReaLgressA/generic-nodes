using System;
using GenericNodes.Visual.Views;
using GenericNodes.Visual.Views.Project;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.GenericElements {
    public class FileTreeViewEntry : ClickableEntry, 
                                     IFilePathEntry {
        [SerializeField] private TextMeshProUGUI textFileName;
        [SerializeField] private Image iconFileExtension;
        [SerializeField] private LayoutElement layoutIndentSpacer;

        private int subdirectoryLevel = 0;
        private string filePath;

        public event Action<IFilePathEntry> SelectEntry;
        public event Action<string> OpenFile;

        public string FilePath => filePath;
        
        public bool IsDirectory => false;
        public string Path => FilePath;
        private IFilePathEntryManager FileManager { get; set; }
        
        public int SubdirectoryLevel {
            get => subdirectoryLevel;
            private set {
                subdirectoryLevel = value;
                layoutIndentSpacer.minWidth = 16 * (subdirectoryLevel - 1);
            }
        }
        
        public FileTreeViewEntry Setup(string filePath, string fileName, string extension, RectTransform rtrRoot,
                                       IFilePathEntryManager fileManager, int subdirectoryLevel) {
            Reset();
            this.filePath = filePath;
            FileManager = fileManager;
            textFileName.text = fileName;
            SubdirectoryLevel = subdirectoryLevel;
            iconFileExtension.enabled = true; //TODO: add & show extensions icon
            transform.SetParent(rtrRoot);
            transform.SetAsLastSibling();
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
                //OpenFile?.Invoke(filePath);
                FileManager.OpenFile(this);
            } else {
                //SelectEntry?.Invoke(this);
                FileManager.SelectEntry(this);
            }
        }
    }
}