using System.IO;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.FileManagement;
using GenericNodes.Utility;
using UnityEngine;

namespace GenericNodes.Visual.Views.Project {
    public class ProjectHierarchyUIView : MonoBehaviour {

        private DirectoryWatcher directoryWatcher = null;

        private GenericNodesProjectInfo Info { get; set; }

        [SerializeField] private RectTransform rtrDirectoriesRoot;
        
        [SerializeField] private DirectoryTreeViewEntry prefabDirectoryEntry;
        [SerializeField] private FileTreeViewEntry prefabFileEntry;

        private PrefabPool<DirectoryTreeViewEntry> poolDirectoryEntries;
        private PrefabPool<FileTreeViewEntry> poolFileEntries;
        private DirectoryTreeViewEntry rootDirectoryEntry = null;

        private void Awake() {
            poolDirectoryEntries = new PrefabPool<DirectoryTreeViewEntry>(prefabDirectoryEntry, rtrDirectoriesRoot, 0);
            //poolFiles = new PrefabPool<FileTreeViewEntry>(prefabFileEntry, rtrDirectoriesRoot, 0);
        }

        public void Setup(GenericNodesProjectInfo info) {
            Reset();
            Info = info;
            if (info != null) {
                directoryWatcher = new DirectoryWatcher(info.RootPath);
            }
            RefreshTreeView();
        }

        private void RefreshTreeView() {
            SetupRootDirectory(Info.RootPath);
        }

        private void SetupRootDirectory(string rootPath) {
            DirectoryInfo dirInfo = new DirectoryInfo(rootPath);
            UnsubscribeFromEntryEvents();
            rootDirectoryEntry?.Reset();
            poolDirectoryEntries.ReleaseAll();
            rootDirectoryEntry = poolDirectoryEntries.Request().Setup(rootPath, dirInfo.Name, rtrDirectoriesRoot,
                                                                      poolDirectoryEntries, poolFileEntries);
            SubscribeToEntryEvents();
        }

        private void SubscribeToEntryEvents() {
            
        }

        private void UnsubscribeFromEntryEvents() {
            
        }

        private void Reset() {
            directoryWatcher?.Dispose();
            directoryWatcher = null;
        }
    }
}