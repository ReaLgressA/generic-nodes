using System;
using System.Collections.Generic;
using System.IO;
using GenericNodes.Utility;
using TMPro;
using UnityEngine;

namespace GenericNodes.Visual.Views {
    public class DirectoryTreeViewEntry : ClickableEntry, 
                                          IFilePathEntry {
        [SerializeField] private TextMeshProUGUI textDirectoryName;

        private string directoryPath;
        private PrefabPool<DirectoryTreeViewEntry> poolDirectories;
        private PrefabPool<FileTreeViewEntry> poolFiles;
        private List<DirectoryTreeViewEntry> directoryEntries = new List<DirectoryTreeViewEntry>();
        private List<FileTreeViewEntry> fileEntries = new List<FileTreeViewEntry>();

        private bool isUnfolded = true;
        private RectTransform rtrContentRoot;

        public event Action<IFilePathEntry> SelectEntry;
        public event Action<string> UnfoldDirectory;

        public string DirectoryPath => directoryPath;
        
        public bool IsDirectory => true;
        public string Path => DirectoryPath;

        public DirectoryTreeViewEntry Parent { get; private set; } = null; 
        public int SubdirectoryLevel { get; private set; } = 0;

        public bool IsUnfolded {
            get => isUnfolded && (Parent == null || Parent.isUnfolded);
            private set {
                isUnfolded = value;
            }
        }
        
        public DirectoryTreeViewEntry Setup(string directoryPath, string directoryName, RectTransform rtrContentRoot,
                                            PrefabPool<DirectoryTreeViewEntry> poolDirectories,
                                            PrefabPool<FileTreeViewEntry> poolFiles,
                                            DirectoryTreeViewEntry parent = null) {
            Reset();
            this.directoryPath = directoryPath;
            this.poolDirectories = poolDirectories;
            this.poolFiles = poolFiles;
            this.rtrContentRoot = rtrContentRoot;
            Parent = parent;
            textDirectoryName.text = directoryName;
            transform.SetParent(rtrContentRoot);
            transform.localScale = Vector3.one;
            transform.SetAsLastSibling();
            gameObject.SetActive(true);
            if (Parent != null) {
                SubdirectoryLevel = Parent.SubdirectoryLevel + 1;
            }
            SetupSubdirectories();
            SetupFiles();
            return this;
        }

        private void SetupSubdirectories() {
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
            DirectoryInfo[] directories = dirInfo.GetDirectories();
            UnsubscribeFromEntryEvents();
            for (int i = 0; i < directories.Length; ++i) {
                if ((directories[i].Attributes & FileAttributes.Directory) == 0 
                    || (directories[i].Attributes & FileAttributes.Hidden) > 0) {
                    continue;
                }
                directoryEntries.Add(poolDirectories.Request().Setup(directories[i].FullName,
                                                                     directories[i].Name, rtrContentRoot,
                                                                     poolDirectories, poolFiles, this));
            }
            //TODO: file entries setup
            SubscribeToEntryEvents();
        }

        private void SubscribeToEntryEvents() {
            
        }

        private void UnsubscribeFromEntryEvents() {
            
        }

        private void SetupFiles() {
            
        }

        public override void Reset() {
            directoryEntries.Clear();
            fileEntries.Clear();
            base.Reset();
        }

        private void Awake() {
            Click += ProcessClick;
        }

        private void OnDestroy() {
            Click -= ProcessClick;
        }

        private void ProcessClick() {
            if (IsSelected) {
                UnfoldDirectory?.Invoke(directoryPath);
            } else {
                SelectEntry?.Invoke(this);
            }
        }
    }
}