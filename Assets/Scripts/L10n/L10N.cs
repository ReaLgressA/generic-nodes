using System;
using System.Collections.Generic;
using UnityEngine;

namespace L10n {
    public static class L10N {
        private const string DEFAULT_LANGUAGE_KEY = "EN";
        public static LocalizationSetup Config { get; private set; }
        public static string ActiveLanguage { get; private set; } = DEFAULT_LANGUAGE_KEY;

        private static readonly Dictionary<string, LocalizedLanguage> languages = new Dictionary<string, LocalizedLanguage>();
        
        public static event Action EventLanguageChanged;
        
        public static void Initialize(LocalizationSetup localizationSetup) {
            Dispose();
            Config = localizationSetup;
            for (int i = 0; i < Config.Languages.Count; ++i) {
                languages[Config.Languages[i].Id] = new LocalizedLanguage(Config.Languages[i].Id);
            }
        }

        public static void Dispose() {
            Config = null;
            languages.Clear();
        }

        public static string Translate(string key) {
            if (!string.IsNullOrWhiteSpace(key) && key[0] == Constants.PREFIX) {
                int categorySeparatorIndex = key.IndexOf(':');
                return languages[ActiveLanguage].Translate(key.Substring(1, categorySeparatorIndex - 1),
                                                           key.Substring(categorySeparatorIndex + 1));
            }
            Debug.LogError($"L10N:Translate failed: key is null or missing prefix: '{key}'");
            return $"!{key}!";
        }

        public static bool SetActiveLanguage(string languageKey) {
            if (languages.ContainsKey(languageKey)) {
                ActiveLanguage = languageKey;
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

        public static LocalizedLanguage GetLanguage(string id) {
            return languages.TryGetValue(id, out LocalizedLanguage language) ? language : null;
        }
    }
}