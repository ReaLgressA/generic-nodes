using System.Collections;
using JsonParser;

namespace GenericNodes.Mech.Data {
    public class GenericNodesProjectFile : IJsonInterface {
        public string Name { get; set; } = "new.json";

        public void ToJsonObject(Hashtable ht) {
            ht[Keys.FILE_NAME] = Name;
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Name = ht.GetStringSafe(Keys.FILE_NAME, Name);
        }
        
        private class Keys {
            public const string FILE_NAME = "FileName";
        }
    }
}