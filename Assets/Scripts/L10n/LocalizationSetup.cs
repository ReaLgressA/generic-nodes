using System.Collections;
using System.Collections.Generic;
using JsonParser;

namespace L10n {
    public class LocalizationSetup : IJsonInterface {
        public List<LanguageData> Languages { get; set; } = new List<LanguageData>();
        
        public void ToJsonObject(Hashtable ht) {
            ht[Keys.TYPE] = "Language";
            ht[Keys.LANGUAGES] = Languages;
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Languages = ht.GetList(Keys.LANGUAGES, Languages);
        }

        private static class Keys {
            public const string TYPE = "Type";
            public const string LANGUAGES = "Languages";
        }
    }
}