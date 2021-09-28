using System.Collections;
using JsonParser;

namespace L10n {
    public class LanguageDescription : IJsonInterface {
        public string Id { get; protected set; }
        public string Name { get; protected set; }
        public string FontPath { get; protected set; }
        
        public void ToJsonObject(Hashtable ht) {
            ht[Keys.ID] = Id;
            ht[Keys.NAME] = Name;
            ht[Keys.FONT] = FontPath;
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Id = ht.GetStringSafe(Keys.ID);
            Name = ht.GetStringSafe(Keys.NAME);
            FontPath = ht.GetStringSafe(Keys.FONT);
        }
        
        private static class Keys {
            public const string ID = "Id";
            public const string NAME = "Name";
            public const string FONT = "Font";
        }
    }
}