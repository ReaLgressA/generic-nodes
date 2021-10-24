using System.Collections;
using System.IO;
using GenericNodes.Visual.Popups;
using JsonParser;

namespace GenericNodes.Mech.Data {
    public class GenericNodesProjectInfo : IJsonInterface {
        public string ProjectName { get; set; } = "New Project";

        public GenericNodesProjectDirectory RootDirectory { get; set; }

        public string RootPath { get; set; } = null;
        public string AbsoluteRootPath => Path.Combine(RootPath, RootDirectory.Name);
        public GraphSchemeProvider SchemeProvider { get; private set; }
        public VaultProvider VaultProvider { get; private set; }
        public LocalizationProvider LocalizationProvider { get; private set; }
        
        public GenericNodesProjectInfo() {}

        public void ToJsonObject(Hashtable ht) {
            ht[Keys.PROJECT_NAME] = ProjectName;
            ht[Keys.ROOT_DIRECTORY] = RootDirectory;
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            ProjectName = ht.GetStringSafe(Keys.PROJECT_NAME, ProjectName);
            RootDirectory = ht.GetAs(Keys.ROOT_DIRECTORY, RootDirectory);
        }

        public void BuildProviders() {
            SchemeProvider = new GraphSchemeProvider();
            SchemeProvider.Setup(this);
            VaultProvider = new VaultProvider();
            VaultProvider.Setup(this);
            LocalizationProvider = new LocalizationProvider();
            LocalizationProvider.Setup(AbsoluteRootPath);
            PopupManager.GetPopup<SelectSpriteAssetPopup>().VaultProvider = VaultProvider;
        }

        private static class Keys {
            public const string PROJECT_NAME = "ProjectName";
            public const string ROOT_DIRECTORY = "RootDirectory";
        }
    }
}