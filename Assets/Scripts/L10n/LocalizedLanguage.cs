using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace L10n {
    public class LocalizedLanguage {
        private readonly Dictionary<string, LocalizationDataPack> categories = new Dictionary<string, LocalizationDataPack>();
        
        public string LanguageKey { get; }

        public LocalizedLanguage(string languageKey) {
            LanguageKey = languageKey;
        }
        
        public void Register(LocalizationDataPack dataPack) {
            if (!LanguageKey.Equals(dataPack.Language, StringComparison.Ordinal)) {
                Debug.LogError($"Failed to register localization data pack: languages are not equal {LanguageKey} != {dataPack.Language}");
                return;
            }
            if (categories.TryGetValue(dataPack.Category, out LocalizationDataPack localPack)) {
                localPack.TryMerge(dataPack);
            } else {
                categories.Add(dataPack.Category, dataPack);
            }  
        }
        
        public List<LocalizationDataPack> ListCategories() {
            return categories.Values.ToList();
        }

        public void SetKeyTranslation(string category, string key, string translation) {
            if (categories.TryGetValue(category, out LocalizationDataPack localPack)) {
                localPack.SetKey(key, translation);
            } else {
                var newDataPack = new LocalizationDataPack {Language = LanguageKey, Category = category};
                newDataPack.SetKey(key, translation);
                categories.Add(category, newDataPack);
            }
        }
        
        public string Translate(string category, string key) {
            categories.TryGetValue(category, out LocalizationDataPack dataPack);
            string localizedKey = dataPack?.Translate(key);
            Debug.Log($"Attempt to translate: '{category}:{key} = {localizedKey}'");
            return localizedKey ?? $"%{category}:{key}%";
        }

        public bool Translate(string category, string key, out string result) {
            categories.TryGetValue(category, out LocalizationDataPack dataPack);
            result = dataPack?.Translate(key);
            return result != null;
        }

        public bool DoesKeyExist(string category, string key) {
            return Translate(category, key, out _);
        }

        public void RegisterKey(string category, string key) {
            SetKeyTranslation(category, key, string.Empty);
        }
    }
}