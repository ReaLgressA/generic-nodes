using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JsonParser;

namespace L10n {
    public class LocalizationConfig : IJsonInterface {
        public string Language { get; set; }
        public string Category { get; set; }
        public Dictionary<string, string> Content { get; private set; } = new Dictionary<string, string>();
        
        public void ToJsonObject(Hashtable ht) {
            ht[Keys.TYPE] = "Localization";
            ht[Keys.LANGUAGE] = Language;
            ht[Keys.CATEGORY] = Category;
            LocalizedKey[] keyPairs = new LocalizedKey[Content.Count];
            int i = 0;
            foreach (KeyValuePair<string, string> pair in Content) {
                keyPairs[i++] = new LocalizedKey { Key = pair.Key, Value = pair.Value };
            }
            ht[Keys.CONTENT] = Content.ToArray();
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Language = ht.GetStringSafe(Keys.LANGUAGE, string.Empty);
            Category = ht.GetStringSafe(Keys.CATEGORY, string.Empty);
            LocalizedKey[] keyPairs = ht.GetArray(Keys.CONTENT, Array.Empty<LocalizedKey>());
            for (int i = 0; i < keyPairs.Length; ++i) {
                Content.Add($"{Category}:{keyPairs[i].Key}", keyPairs[i].Value);
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