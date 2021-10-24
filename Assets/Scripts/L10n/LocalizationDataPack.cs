using System;
using System.Collections;
using System.Collections.Generic;
using JsonParser;
using UnityEngine;

namespace L10n {
    public class LocalizationDataPack : IJsonInterface {
        public string Language { get; set; }
        public string Category { get; set; }
        public Dictionary<string, string> Content { get; private set; } = new Dictionary<string, string>();

        public bool IsSamePack(LocalizationDataPack dataPack) {
            return Language.Equals(dataPack.Language) && Category.Equals(dataPack.Category);
        }
        
        public bool TryMerge(LocalizationDataPack dataPack) {
            if (IsSamePack(dataPack)) {
                foreach (var entry in dataPack.Content) {
                    if (Content.ContainsKey(entry.Key)) {
                        Debug.LogError($"Failed to merge key between dataPacks: [{Language}]'{Category}:{entry.Key}' - key already exist!");
                        continue;
                    }
                    Content.Add(entry.Key, entry.Value);
                }
                return true;
            }
            return false;
        }
        
        public string Translate(string key) {
            return Content.TryGetValue(key, out string localizedKey) ? localizedKey : null;
        }
        
        public void SetKey(string key, string translation) {
            Content[key] = translation;
        }
        
        public void ToJsonObject(Hashtable ht) {
            ht[Keys.TYPE] = "Localization";
            ht[Keys.LANGUAGE] = Language;
            ht[Keys.CATEGORY] = Category;
            LocalizedKey[] keyPairs = new LocalizedKey[Content.Count];
            int i = 0;
            
            foreach (KeyValuePair<string, string> pair in Content) {
                keyPairs[i++] = new LocalizedKey { Key = pair.Key, Value = pair.Value };
            }
            ht[Keys.CONTENT] = keyPairs;
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Language = ht.GetStringSafe(Keys.LANGUAGE, string.Empty);
            Category = ht.GetStringSafe(Keys.CATEGORY, string.Empty);
            LocalizedKey[] keyPairs = ht.GetArray(Keys.CONTENT, Array.Empty<LocalizedKey>());
            for (int i = 0; i < keyPairs.Length; ++i) {
                Content.Add(keyPairs[i].Key, keyPairs[i].Value);
            }
        }

        private static class Keys {
            public const string TYPE = "Type";
            public const string LANGUAGE = "Language";
            public const string CATEGORY = "Category";
            public const string CONTENT = "Content";
        }
    }
}