using System;
using System.Collections.Generic;
using System.IO;
using GenericNodes.Utility;
using GenericNodes.Visual.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace GenericNodes.Visual.Popups {
    public class SelectFilePathPopup : MonoBehaviour {
        [SerializeField] private DirectoryViewEntry prefabDirectoryView;
        [SerializeField] private FileViewEntry prefabFileView;

        [SerializeField] private TMP_InputField textInputActiveFilename; 
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
        private IFilePathEntry selectedEntry = null;

        private readonly List<DirectoryViewEntry> directoryEntries = new List<DirectoryViewEntry>();
        private readonly List<FileViewEntry> fileEntries = new List<FileViewEntry>();
        
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
            
            poolFileViews = new PrefabPool<FileViewEntry>(prefabFileView, rtrPrefabPoolsRoot, 16);
            poolDirectoryViews = new PrefabPool<DirectoryViewEntry>(prefabDirectoryView, rtrPrefabPoolsRoot, 16);

            textActionButton.text = actionName;
            OpenDirectory(openDirectoryPath);
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
            ResetContent();
        }
        
        private void OpenDirectory(string directoryPath) {
            activeDirectoryPath = directoryPath;
            selectedEntry = null;
            textActiveDirectoryPath.text = activeDirectoryPath.Reverse();
            RefreshDirectoryContent();
            RefreshActionButtonState();
        }

        private void OpenFile(string filePath) {
            if (selectionPolicy == FileSelectionPolicy.OpenFile) {
                ProcessActionButtonClick();
            }
        }
        
        private void RefreshDirectoryContent() {
            DirectoryInfo dirInfo = new DirectoryInfo(activeDirectoryPath);
            DirectoryInfo[] directories = dirInfo.GetDirectories();
            Debug.Log($"RefreshDirectoryContent: {activeDirectoryPath}");
            ResetContent();
            if (dirInfo.Parent != null) {
                directoryEntries.Add(poolDirectoryViews.Request().Setup(dirInfo.Parent.FullName,
                                                                        "..", rtrContentRoot));
            }
            for (int i = 0; i < directories.Length; ++i) {
                if ((directories[i].Attributes & FileAttributes.Directory) == 0 
                    || (directories[i].Attributes & FileAttributes.Hidden) > 0) {
                    continue;
                }
                directoryEntries.Add(poolDirectoryViews.Request().Setup(directories[i].FullName,
                                                                        directories[i].Name, rtrContentRoot));
            }
            FileInfo[] files = dirInfo.GetFiles();
            for (int i = 0; i < files.Length; ++i) {
                if ((files[i].Attributes & FileAttributes.Hidden) > 0) {
                    continue;   
                }
                fileEntries.Add(poolFileViews.Request().Setup(files[i].FullName, files[i].Name,
                                                              files[i].Extension, rtrContentRoot));
            }
            SubscribeToEntryEvents();
        }

        private void ResetContent() {
            UnsubscribeFromEntryEvents();
            directoryEntries.Clear();
            fileEntries.Clear();
            poolFileViews.ReleaseAll();
            poolDirectoryViews.ReleaseAll();
        }

        private void UnsubscribeFromEntryEvents() {
            for (int i = 0; i < directoryEntries.Count; ++i) {
                directoryEntries[i].OpenDirectory -= OpenDirectory;
                directoryEntries[i].SelectEntry -= SelectEntry;
            }
            for (int i = 0; i < fileEntries.Count; ++i) {
                fileEntries[i].OpenFile -= OpenFile;
                fileEntries[i].SelectEntry -= SelectEntry;
            }
        }

        private void SubscribeToEntryEvents() {
            for (int i = 0; i < directoryEntries.Count; ++i) {
                directoryEntries[i].OpenDirectory += OpenDirectory;
                directoryEntries[i].SelectEntry += SelectEntry;
            }
            for (int i = 0; i < fileEntries.Count; ++i) {
                fileEntries[i].OpenFile += OpenFile;
                fileEntries[i].SelectEntry += SelectEntry;
            }
        }

        private void SelectEntry(IFilePathEntry fileEntry) {
            selectedEntry?.SetSelected(false);
            selectedEntry = fileEntry;
            fileEntry.SetSelected(true);
            RefreshActionButtonState();
        }

        private void RefreshActionButtonState() {

            bool notNull = selectedEntry != null;
            bool isInteractable = ((selectionPolicy & FileSelectionPolicy.OpenFile) > 0 
                                   && notNull && !selectedEntry.IsDirectory) 
                                  || ((selectionPolicy & FileSelectionPolicy.OpenDirectory) > 0 
                                      && notNull &&  selectedEntry.IsDirectory)
                                  || ((selectionPolicy & FileSelectionPolicy.CreateAny) > 0 && IsActiveFilenameValid());
            buttonAction.interactable = isInteractable;
        }

        private bool IsActiveFilenameValid() {
            return textInputActiveFilename.text.Length > 0;
        }

        private void ProcessActionButtonClick() {
            if ((selectionPolicy & FileSelectionPolicy.CreateAny) > 0) {
                selectionAction?.Invoke(Path.Combine(activeDirectoryPath, textInputActiveFilename.text));
            } else {
                selectionAction?.Invoke(selectedEntry.Path);   
            }
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
            OpenAny = OpenDirectory | OpenFile,
            CreateAny = CreateFile | CreateDirectory,
            CustomValidator = 1024
        }
    }
}