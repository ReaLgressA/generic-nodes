using System;
using System.Collections;
using System.IO;
using GenericNodes.Mech;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.FileManagement;
using GenericNodes.Utility;
using GenericNodes.Visual.GenericElements;
using GenericNodes.Visual.Popups;
using JsonParser;
using UnityEngine;
using UnityEngine.UI;

namespace GenericNodes.Visual.Views.Project {
    public class ProjectHierarchyUIView : MonoBehaviour,
                                          IFilePathEntryManager {
        private DirectoryWatcher directoryWatcher = null;

        public GenericNodesProjectInfo Info { get; private set; }
        private ProjectUIView ProjectView { get; set; }

        [SerializeField] private NodeEditorController nodeEditorController;
        [SerializeField] private RectTransform rtrDirectoriesRoot;
        
        [SerializeField] private DirectoryTreeViewEntry prefabDirectoryEntry;
        [SerializeField] private FileTreeViewEntry prefabFileEntry;

        [SerializeField] private Button buttonCloseProject;
        [SerializeField] private Button buttonCreateGraph;
        [SerializeField] private Button buttonRefresh;

        private PrefabPool<DirectoryTreeViewEntry> poolDirectoryEntries;
        private PrefabPool<FileTreeViewEntry> poolFileEntries;
        private DirectoryTreeViewEntry rootDirectoryEntry = null;
        private bool hasAnyFileSystemUpdates = false;

        private IFilePathEntry selectedEntry = null;
        
        private void Awake() {
            poolDirectoryEntries = new PrefabPool<DirectoryTreeViewEntry>(prefabDirectoryEntry, rtrDirectoriesRoot, 0);
            poolFileEntries = new PrefabPool<FileTreeViewEntry>(prefabFileEntry, rtrDirectoriesRoot, 0);
            
            buttonRefresh.onClick.AddListener(RefreshTreeView);
            buttonCreateGraph.onClick.AddListener(CreateGraph);
            buttonCloseProject.onClick.AddListener(CloseProject);
        }

        private void OnDestroy() {
            buttonRefresh.onClick.RemoveAllListeners();
            buttonCreateGraph.onClick.RemoveAllListeners();
            buttonCloseProject.onClick.RemoveAllListeners();
        }

        public void Setup(ProjectUIView projectUIView) {
            ProjectView = projectUIView;
            selectedEntry = null;
        }

        public void SetProject(GenericNodesProjectInfo info) {
            Reset();
            Info = info;
            if (info != null) {
                directoryWatcher = new DirectoryWatcher(Info.AbsoluteRootPath);
                directoryWatcher.RefreshDirectory += ProcessDirectoryWatcherEvent;
            }
            RefreshTreeView();
        }

        private void RefreshAvailableSchemes() {
            Info.BuildProviders();
        }

        private void ProcessDirectoryWatcherEvent() {
            hasAnyFileSystemUpdates = true;
        }

        private void Update() {
            if (hasAnyFileSystemUpdates) {
                hasAnyFileSystemUpdates = false;
                RefreshTreeView();
            }

            RefreshButtonStates();
        }

        private void RefreshButtonStates() {
            buttonCreateGraph.interactable = selectedEntry != null
                                             && selectedEntry.IsDirectory && Info.SchemeProvider.Schemes.Count > 0;
        }

        public void RefreshTreeView() {
            if (Info != null) {
                SetupRootDirectory(Info.AbsoluteRootPath);
                RefreshAvailableSchemes();
            }
        }
        
        public void SelectEntry(IFilePathEntry entry) {
            selectedEntry?.SetSelected(false);
            selectedEntry = entry;
            selectedEntry?.SetSelected(true);
        }

        public void OpenFile(FileTreeViewEntry entry) {
            string json = File.ReadAllText(entry.FilePath);
            Hashtable htTargetFile = MiniJSON.JsonDecode(json) as Hashtable;
            if (htTargetFile != null) {
                string graphType = htTargetFile.GetStringSafe("Type", null);
                if (graphType == null) {
                    Debug.LogError($"Failed to open file, type not specified: {entry.FilePath}");        
                }
                GraphScheme scheme = Info.SchemeProvider.Schemes.Find(
                    graphScheme => graphScheme.Type.Equals(graphType, StringComparison.Ordinal));
                if (scheme == null) {
                    Debug.LogError($"Failed to open file, type {graphType} doe: {entry.FilePath}");
                    return;
                }
                Debug.Log($"Open file {graphType} {entry.FilePath}");
                GraphData graph = scheme.CreateGraph(entry.FilePath);
                graph.FromJson(htTargetFile);
                nodeEditorController.OpenGraph(graph);
                return;
            }
            Debug.LogError($"Failed to open file: {entry.FilePath}");
        }

        private void SetupRootDirectory(string rootPath) {
            DirectoryInfo dirInfo = new DirectoryInfo(rootPath);
            rootDirectoryEntry?.Reset();
            poolDirectoryEntries.ReleaseAll();
            poolFileEntries.ReleaseAll();
            rootDirectoryEntry = poolDirectoryEntries.Request().Setup(rootPath, dirInfo.Name, rtrDirectoriesRoot,
                                                                      poolDirectoryEntries, poolFileEntries, this);
        }

        private void Reset() {
            if (directoryWatcher != null) {
                directoryWatcher.RefreshDirectory -= ProcessDirectoryWatcherEvent;
                directoryWatcher.Dispose();
                directoryWatcher = null;
            }
        }
        
        private void CreateGraph() {
            
            PopupManager.GetPopup<CreateGraphPopup>().Show(selectedEntry.Path, Info.SchemeProvider, ProjectView);
        }
        
        private void CloseProject() {
            ProjectView.CloseProject();
        }
    }

    public interface IFilePathEntryManager {
        void RefreshTreeView();
        void SelectEntry(IFilePathEntry entry);
        void OpenFile(FileTreeViewEntry entry);
    }
}