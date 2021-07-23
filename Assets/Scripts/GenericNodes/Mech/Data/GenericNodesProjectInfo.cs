using System.Collections;
using System.IO;
using MiniJSON;

namespace GenericNodes.Mech.Data {
    public class GenericNodesProjectInfo : IJsonInterface {
        public string ProjectName { get; set; } = "New Project";

        public GenericNodesProjectDirectory RootDirectory { get; set; }

        public string RootPath { get; set; } = null;
        
        public void ToJsonObject(Hashtable ht) {
            ht[Keys.PROJECT_NAME] = ProjectName;
            ht[Keys.ROOT_DIRECTORY] = RootDirectory;
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            ProjectName = ht.GetStringSafe(Keys.PROJECT_NAME, ProjectName);
            RootDirectory = ht.GetAs(Keys.ROOT_DIRECTORY, RootDirectory);
        }

        public bool TryGetPath(GenericNodesProjectDirectory directory, out string path) {
            if (RootDirectory.TryGetPath(directory, out path)) {
                path = Path.Combine(RootPath, path);
                return true;
            }
            path = null;
            return false;
        }
        
        public bool TryGetPath(GenericNodesProjectFile file, out string path) {
            if (RootDirectory.TryGetPath(file, out path)) {
                path = Path.Combine(RootPath, path);
                return true;
            }
            path = null;
            return false;
        }

        private class Keys {
            public const string PROJECT_NAME = "ProjectName";
            public const string ROOT_DIRECTORY = "RootDirectory";
        }
    }
}