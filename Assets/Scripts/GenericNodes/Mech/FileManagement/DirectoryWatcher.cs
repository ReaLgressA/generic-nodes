using System;
using System.IO;
using UnityEngine;

namespace GenericNodes.Mech.FileManagement {
    public class DirectoryWatcher : IDisposable {
        private readonly FileSystemWatcher watcher;

        public event Action RefreshDirectory;
        
        public DirectoryWatcher(string path) {
            watcher = new FileSystemWatcher(path) {
                NotifyFilter = (NotifyFilters)(~0),
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;
        }
        
        public void Dispose()
        {
            watcher?.Dispose();
        }

        private void OnError(object sender, ErrorEventArgs args) {
            Debug.LogError($"DirectoryWatcher error: {args.GetException()}");
        }

        private void OnRenamed(object sender, RenamedEventArgs args) {
            RefreshDirectory?.Invoke();
        }
        
        private void OnDeleted(object sender, FileSystemEventArgs args) {
            RefreshDirectory?.Invoke();
        }

        private void OnCreated(object sender, FileSystemEventArgs args) {
            RefreshDirectory?.Invoke();
        }

        private void OnChanged(object sender, FileSystemEventArgs args) {
            RefreshDirectory?.Invoke();
        }
    }
}