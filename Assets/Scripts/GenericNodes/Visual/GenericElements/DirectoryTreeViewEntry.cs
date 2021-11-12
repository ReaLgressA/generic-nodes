using System;
using System.Collections.Generic;
using System.IO;
using GenericNodes.Utility;
using GenericNodes.Visual.Views;
using GenericNodes.Visual.Views.Project;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.GenericElements {
    public class DirectoryTreeViewEntry : ClickableEntry, 
                                          IFilePathEntry {
        [SerializeField] private TextMeshProUGUI textDirectoryName;
        [SerializeField] private Sprite iconFoldoutOpen;
        [SerializeField] private Sprite iconFoldoutClosed;
        [SerializeField] private Image imageFoldout;
        [SerializeField] private LayoutElement layoutIndentSpacer;

        private int subdirectoryLevel = 0;
        private string directoryPath;
        private PrefabPool<DirectoryTreeViewEntry> poolDirectories;
        private PrefabPool<FileTreeViewEntry> poolFiles;
        private List<DirectoryTreeViewEntry> directoryEntries = new List<DirectoryTreeViewEntry>();
        private List<FileTreeViewEntry> fileEntries = new List<FileTreeViewEntry>();

        private bool isUnfolded = false;
        private RectTransform rtrContentRoot;

        public event Action<IFilePathEntry> SelectEntry;
        public event Action<string> UnfoldDirectory;

        public string DirectoryPath => directoryPath;
        
        public bool IsDirectory => true;
        public string Path => DirectoryPath;

        public DirectoryTreeViewEntry Parent { get; private set; } = null;

        public int SubdirectoryLevel {
            get => subdirectoryLevel;
            private set {
                subdirectoryLevel = value;
                layoutIndentSpacer.minWidth = 16 * subdirectoryLevel;
            }
        }

        public bool IsUnfolded {
            get => isUnfolded && (Parent == null || Parent.isUnfolded);
            private set {
                isUnfolded = value || Parent == null;
                imageFoldout.sprite = isUnfolded ? iconFoldoutOpen : iconFoldoutClosed;
            }
        }
        
        private IFilePathEntryManager FileManager { get; set; }
        
        public DirectoryTreeViewEntry Setup(string directoryPath,
                                            string directoryName,
                                            RectTransform rtrContentRoot,
                                            PrefabPool<DirectoryTreeViewEntry> poolDirectories,
                                            PrefabPool<FileTreeViewEntry> poolFiles,
                                            IFilePathEntryManager fileManager,
                                            DirectoryTreeViewEntry parent = null) {
            Reset();
            this.directoryPath = directoryPath;
            this.poolDirectories = poolDirectories;
            this.poolFiles = poolFiles;
            this.rtrContentRoot = rtrContentRoot;
            FileManager = fileManager;
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

        public void SetFoldoutStateForChildren(bool isUnfolded) {
            foreach (var dir in directoryEntries) {
                dir.IsUnfolded = isUnfolded;
            }
        }

        public void ToggleFoldoutState() {
            IsUnfolded = !IsUnfolded;
        }

        private void SetupSubdirectories() {
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
            UnsubscribeFromEntryEvents();
            if (IsUnfolded) {
                DirectoryInfo[] directories = dirInfo.GetDirectories();
                for (int i = 0; i < directories.Length; ++i) {
                    if ((directories[i].Attributes & FileAttributes.Directory) == 0
                        || (directories[i].Attributes & FileAttributes.Hidden) > 0) {
                        continue;
                    }

                    directoryEntries.Add(poolDirectories.Request().Setup(directories[i].FullName,
                                                                         directories[i].Name, rtrContentRoot,
                                                                         poolDirectories, poolFiles, FileManager,
                                                                         this));
                }
            }
            
            SubscribeToEntryEvents();
        }

        private void SubscribeToEntryEvents() {
            
        }

        private void UnsubscribeFromEntryEvents() {
            
        }

        private void SetupFiles() {
            if (!IsUnfolded) {
                return;
            }
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
            FileInfo[] files = dirInfo.GetFiles();
            for (int i = 0; i < files.Length; ++i) {
                if ((files[i].Attributes & FileAttributes.Hidden) > 0) {
                    continue;   
                }
                fileEntries.Add(poolFiles.Request().Setup(files[i].FullName, files[i].Name,
                                                          files[i].Extension, rtrContentRoot,
                                                          FileManager, SubdirectoryLevel + 1));
            }
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
                ToggleFoldoutState();
                //UnfoldDirectory?.Invoke(directoryPath);
                FileManager.RefreshTreeView();
            } else {
                FileManager.SelectEntry(this);
                //SelectEntry?.Invoke(this);
            }
        }
    }
}