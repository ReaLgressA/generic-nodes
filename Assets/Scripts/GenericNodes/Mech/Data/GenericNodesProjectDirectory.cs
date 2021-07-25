using System.Collections;
using System.Collections.Generic;
using System.IO;
using JsonParser;

namespace GenericNodes.Mech.Data {
    public class GenericNodesProjectDirectory : IJsonInterface {
        public string Name { get; set; } = "NewDirectory";
        public bool IsOpen { get; set; } = false;
        public List<GenericNodesProjectDirectory> Directories { get; private set; } = new List<GenericNodesProjectDirectory>();
        public List<GenericNodesProjectFile> Files { get; private set; } = new List<GenericNodesProjectFile>();
        
        public bool TryGetPath(GenericNodesProjectDirectory directory, out string path) {
            for (int i = 0; i < Directories.Count; ++i) {
                if (Directories[i] == directory) {
                    path = Name;
                    return true;
                }   
            }
            for (int i = 0; i < Directories.Count; ++i) {
                if (Directories[i].TryGetPath(directory, out string subdirectoryPath)) {
                    path = Path.Combine(Name, subdirectoryPath);
                    return true;
                }   
            }
            path = null;
            return false;
        }
        
        public bool TryGetPath(GenericNodesProjectFile file, out string path) {
            for (int i = 0; i < Files.Count; ++i) {
                if (Files[i] == file) {
                    path = Path.Combine(Name, Files[i].Name);
                    return true;
                }
            }
            for (int i = 0; i < Directories.Count; ++i) {
                if (Directories[i].TryGetPath(file, out string subdirectoryPath)) {
                    path = Path.Combine(Name, subdirectoryPath);
                    return true;
                }   
            }
            path = null;
            return false;
        }
        
        public void ToJsonObject(Hashtable ht) {
            ht[Keys.DIRECTORY_NAME] = Name;
            ht[Keys.IS_OPEN] = IsOpen;
            ht[Keys.DIRECTORIES] = Directories;
            ht[Keys.FILES] = Files;
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Name = ht.GetStringSafe(Keys.DIRECTORY_NAME, Name);
            IsOpen = ht.GetBool(Keys.IS_OPEN, IsOpen);
            Directories = ht.GetList(Keys.DIRECTORIES, Directories);
            Files = ht.GetList(Keys.FILES, Files);
        }

        private class Keys {
            public const string DIRECTORY_NAME = "DirectoryName";
            public const string IS_OPEN = "IsOpen";
            public const string DIRECTORIES = "Directories";
            public const string FILES = "Files";
        }
    }
}