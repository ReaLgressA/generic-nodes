using System;
using System.Collections.Generic;
using UnityEngine;

namespace L10n {
    public static class L10N {
        private const string DEFAULT_LANGUAGE_KEY = "en";
        public static LocalizationSetup Config { get; private set; }
        public static string ActiveLanguageId { get; private set; } = DEFAULT_LANGUAGE_KEY;
        public static LocalizedLanguage ActiveLanguage => 
            languages.TryGetValue(ActiveLanguageId, out LocalizedLanguage language) ? language : null;
        private static readonly Dictionary<string, LocalizedLanguage> languages = new Dictionary<string, LocalizedLanguage>();

        public delegate void KeyTranslationChanged(string localizationKey, string activeLanguageTranslation);
        
        public static event Action EventLanguageChanged;
        public static event KeyTranslationChanged EventKeyTranslationChanged;
        
        public static void Initialize(LocalizationSetup localizationSetup) {
            Dispose();
            Config = localizationSetup;
            for (int i = 0; i < Config.Languages.Count; ++i) {
                languages[Config.Languages[i].Id] = new LocalizedLanguage(Config.Languages[i].Id);
            }
            SetActiveLanguage(Config.Languages[0].Id);
        }

        public static void Dispose() {
            Config = null;
            languages.Clear();
        }

        public static string Translate(string localizationKey) {
            if (ParseLocalizationKey(localizationKey, out string category, out string key)) {
                return languages[ActiveLanguageId].Translate(category, key);
            }
            Debug.LogError($"L10N:Translate failed: key is null or missing prefix: '{localizationKey}'");
            return $"!{localizationKey}!";
        }

        public static bool SetActiveLanguage(string languageKey) {
            if (languages.ContainsKey(languageKey)) {
                ActiveLanguageId = languageKey;
                EventLanguageChanged?.Invoke();
                return true;
            }
            Debug.LogError($"Failed to set active language '{languageKey}' - key doesn't exits.");
            return false;
        }

        public static void Register(LocalizationDataPack dataPack) {
            if (languages.TryGetValue(dataPack.Language, out var language)) {
                language.Register(dataPack);
            } else {
                Debug.LogError($"Failed to register l10n data pack: language {dataPack.Language} doesn't exist!");
            }
        }

        public static bool DoesKeyExist(string localizationKey) {
            if (ParseLocalizationKey(localizationKey, out string category, out string key)) {
                return DoesKeyExist(category, key);
            }
            return false;
        }
        public static bool DoesKeyExist(string category, string key) {
            foreach(var language in languages) {
                if (language.Value.DoesKeyExist(category, key)) {
                    return true;
                }
            }
            return false;
        }

        public static void RegisterKey(string category, string key) {
            foreach(var language in languages) {
                language.Value.RegisterKey(category, key);
            }
        }

        public static LocalizedLanguage GetLanguage(string id) {
            return languages.TryGetValue(id, out LocalizedLanguage language) ? language : null;
        }

        public static void BuildKeyList(ref List<LocalizedKeyDescription> keysCache) {
            keysCache.Clear();
            List<LocalizationDataPack> categories = languages[Config.Languages[0].Id].ListCategories();
            for (int i = 0; i < categories.Count; ++i) {
                foreach (var pair in categories[i].Content) {
                    keysCache.Add(new LocalizedKeyDescription(categories[i].Category, pair.Key));   
                }
            }
        }
        
        public static void SetKeyTranslation(string localizationKey, string translation) {
            if (ParseLocalizationKey(localizationKey, out string category, out string key)) {
                ActiveLanguage.SetKeyTranslation(category, key, translation);
                EventKeyTranslationChanged?.Invoke(localizationKey, Translate(localizationKey));
                return;
            }
            Debug.LogError($"Failed to set key value: '{localizationKey}' doesn't exist for language '{ActiveLanguageId}'");
        }

        private static bool ParseLocalizationKey(string localizationKey, out string category, out string key) {
            category = null;
            key = null;
            if (!string.IsNullOrWhiteSpace(localizationKey) && localizationKey[0] == Constants.PREFIX) {
                int categorySeparatorIndex = localizationKey.IndexOf(':');
                if (categorySeparatorIndex == -1) {
                    return false;
                }
                category = localizationKey.Substring(1, categorySeparatorIndex - 1);
                key = localizationKey.Substring(categorySeparatorIndex + 1);
                return !string.IsNullOrWhiteSpace(category) && !string.IsNullOrWhiteSpace(key);
            }
            return false;
        }
    }
}