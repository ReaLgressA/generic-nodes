using System.Collections;
using System.IO;
using JsonParser;
using L10n;


namespace GenericNodes.Mech.Data {
    public class LocalizationProvider {
        
        public void Setup(GenericNodesProjectInfo projectInfo) {
            string l10nDirectoryPath = Path.Combine(projectInfo.AbsoluteRootPath, "Localization");
            if (!Directory.Exists(l10nDirectoryPath)) {
                Directory.CreateDirectory(l10nDirectoryPath);
            }
            string languagesConfigPath = Path.Combine(l10nDirectoryPath, "localizationSetup.json");
            LocalizationSetup l10nSetup = null;
            if (File.Exists(languagesConfigPath)) {
                string json = File.ReadAllText(languagesConfigPath);
                Hashtable ht = MiniJSON.JsonDecode(json) as Hashtable;
                
            }
            if (l10nSetup == null) {
                return;
            }

            DirectoryInfo l10nDirectory = new DirectoryInfo(l10nDirectoryPath);
            FileInfo[] files = l10nDirectory.GetFiles();
            //
            // for (int i = 0; i < files.Length; ++i) {
            //     string json = File.ReadAllText(files[i].FullName);
            //     Hashtable ht = MiniJSON.JsonDecode(json) as Hashtable;
            //     if (ht != null) {
            //         GraphScheme scheme = new GraphScheme();
            //         try {
            //             scheme.FromJson(ht);
            //             Schemes.Add(scheme);
            //         } catch (Exception ex) {
            //             Debug.LogError($"Failed to parse graph scheme by path: {files[i].FullName}. Exception: {ex.Message}\n{ex.StackTrace}");
            //         }
            //     }
            //     
            // }
        }
    }
}