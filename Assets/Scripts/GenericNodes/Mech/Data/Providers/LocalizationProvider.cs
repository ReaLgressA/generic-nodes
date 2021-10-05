using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JsonParser;
using L10n;
using UnityEngine;

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
                l10nSetup = new LocalizationSetup();
                l10nSetup.FromJson(ht);
            }
            if (l10nSetup == null) {
                l10nSetup = new LocalizationSetup { 
                    Languages = new List<LanguageData> { new LanguageData("en", "English") }
                };
                L10N.Initialize(l10nSetup);
                Save(projectInfo);
                return;
            }
            
            L10N.Initialize(l10nSetup);
            foreach (LanguageData language in l10nSetup.Languages) {
                TryLoadLocalizationDirectory(language, l10nDirectoryPath);
            }
        }

        public void Save(GenericNodesProjectInfo projectInfo) {
            string l10nDirectoryPath = Path.Combine(projectInfo.AbsoluteRootPath, "Localization");
            string languagesConfigPath = Path.Combine(l10nDirectoryPath, "localizationSetup.json");
            string json = MiniJSON.JsonEncode(L10N.Config);
            File.WriteAllText(languagesConfigPath, json);
            foreach (LanguageData language in L10N.Config.Languages) {
                SaveLocalizationDirectory(language, l10nDirectoryPath);
            }
        }

        private void SaveLocalizationDirectory(LanguageData language, string l10nDirectoryPath) {
            string directoryPath = Path.Combine(l10nDirectoryPath, language.Id);
            if (!Directory.Exists(directoryPath)) {
                Directory.CreateDirectory(directoryPath);
            }
            LocalizedLanguage languageData = L10N.GetLanguage(language.Id);
            List<LocalizationDataPack> categories = languageData.ListCategories();
            foreach (LocalizationDataPack categoryData in categories) {
                string categoryFilePath = Path.Combine(directoryPath, $"{language.Id}_{categoryData.Category}.json");
                string jsonContent = MiniJSON.JsonEncode(categoryData);
                File.WriteAllText(categoryFilePath, jsonContent);
            }
        }

        private void TryLoadLocalizationDirectory(LanguageData language, string l10nDirectoryPath) {
            string directoryPath = Path.Combine(l10nDirectoryPath, language.Id);
            if (Directory.Exists(directoryPath)) {
                DirectoryInfo languageDirectory = new DirectoryInfo(directoryPath);
                FileInfo[] files = languageDirectory.GetFiles();
                foreach (var file in files) {
                    string json = File.ReadAllText(file.FullName);
                    if (MiniJSON.JsonDecode(json) is Hashtable ht) {
                        LocalizationDataPack dataPack = new LocalizationDataPack();
                        try {
                            dataPack.FromJson(ht);
                            L10N.Register(dataPack);
                        } catch (Exception ex) {
                            Debug.LogError($"Failed to parse LocalizationDataPack by path: {file.FullName}. Exception: {ex.Message}\n{ex.StackTrace}");
                        }
                    }
                }
            } else {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}