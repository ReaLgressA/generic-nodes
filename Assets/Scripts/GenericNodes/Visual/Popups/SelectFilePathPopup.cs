using System;
using System.Collections.Generic;
using System.IO;
using GenericNodes.Utility;
using GenericNodes.Visual.Views;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace GenericNodes.Visual.Popups {
    public class SelectFilePathPopup : MonoBehaviour {
        [SerializeField] private DirectoryViewEntry prefabDirectoryView;
        [SerializeField] private FileViewEntry prefabFileView;
        
        [FormerlySerializedAs("textFilePath")] 
        [SerializeField] private TextMeshProUGUI textActiveDirectoryPath;
        [SerializeField] private TextMeshProUGUI textActionButton;
        
        [SerializeField] private Button buttonAction;
        [SerializeField] private Button buttonClose;

        [SerializeField] private RectTransform rtrPrefabPoolsRoot;
        [SerializeField] private RectTransform rtrContentRoot;

        private FileSelectionPolicy selectionPolicy;
        private Action<string> selectionAction;

        private PrefabPool<FileViewEntry> poolFileViews;
        private PrefabPool<DirectoryViewEntry> poolDirectoryViews;
        
        private string fileExtensionFilter = null;
        
        private string activeDirectoryPath;
        private string selectedEntryPath = null;

        private readonly List<DirectoryViewEntry> directoryEntries = new List<DirectoryViewEntry>();

        private void Awake() {
            buttonAction.onClick.AddListener(ProcessActionButtonClick);
            buttonClose.onClick.AddListener(ProcessCloseButtonClick);
        }
        
        public void Show(Action<string> action, string actionName, string openDirectoryPath,
                         FileSelectionPolicy policy, string fileExtensionFilter = null) {
            selectionAction = action;
            selectionPolicy = policy;
            activeDirectoryPath = openDirectoryPath;
            this.fileExtensionFilter = fileExtensionFilter;
            
            //poolFileViews = new PrefabPool<FileViewEntry>(prefabFileView, rtrPrefabPoolsRoot, 16);
            poolDirectoryViews = new PrefabPool<DirectoryViewEntry>(prefabDirectoryView, rtrPrefabPoolsRoot, 16);

            textActionButton.text = actionName;
            OpenDirectory(openDirectoryPath);
            gameObject.SetActive(true);
        }

        private void SelectFile(string filePath) {
            selectedEntryPath = filePath;
        }

        private void OpenDirectory(string directoryPath) {
            activeDirectoryPath = directoryPath;
            selectedEntryPath = directoryPath;
            textActiveDirectoryPath.text = activeDirectoryPath;
            RefreshDirectoryContent();
        }
        
        public void Hide() {
            gameObject.SetActive(false);
        }
        
        private void RefreshDirectoryContent() {
            DirectoryInfo dirInfo = new DirectoryInfo(activeDirectoryPath);
            DirectoryInfo[] directories = dirInfo.GetDirectories();
            Debug.Log($"RefreshDirectoryContent: {activeDirectoryPath}");
            UnsubscribeFromEntryEvents();
            directoryEntries.Clear();
            poolDirectoryViews.ReleaseAll();
            if (dirInfo.Parent != null) {
                directoryEntries.Add(poolDirectoryViews.Request().Setup(dirInfo.Parent.FullName, "..", rtrContentRoot));
            }
            for (int i = 0; i < directories.Length; ++i) {
                directoryEntries.Add(poolDirectoryViews.Request().Setup(directories[i].FullName, directories[i].Name, rtrContentRoot));
            }
            FileInfo[] files = dirInfo.GetFiles();
            for (int i = 0; i < files.Length; ++i) {
                Debug.Log($"File: {files[i].Name}");
            }
            SubscribeToEntryEvents();
        }
        
        private void UnsubscribeFromEntryEvents() {
            for (int i = 0; i < directoryEntries.Count; ++i) {
                directoryEntries[i].OpenDirectory -= OpenDirectory;
                directoryEntries[i].SelectDirectory -= SelectDirectory;
            }
        }

        private void SubscribeToEntryEvents() {
            for (int i = 0; i < directoryEntries.Count; ++i) {
                directoryEntries[i].OpenDirectory += OpenDirectory;
                directoryEntries[i].SelectDirectory += SelectDirectory;
            }
        }

        private void SelectDirectory(string directoryPath) {
            Debug.Log($"Select directory: {directoryPath}");
            for (int i = 0; i < directoryEntries.Count; ++i) {
                if (directoryEntries[i].DirectoryPath.Equals(directoryPath, StringComparison.Ordinal)) {
                    directoryEntries[i].SetSelected(true);
                } 
            }
        }

        private void ProcessActionButtonClick() {
            
        }
        
        private void ProcessCloseButtonClick() {
            Hide();
        }

        public enum FileSelectionPolicy {
            None = 0,
            OpenFile = 1,
            OpenDirectory = 2,
            CreateFile = 4,
            CreateDirectory = 8,
            OpenAny = OpenDirectory | OpenFile
        }
    }
}