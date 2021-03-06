using System.Collections;
using System.Collections.Generic;
using GenericNodes.Mech.Fields;
using JsonParser;

namespace GenericNodes.Mech.Data {
    public class NodeDescription : IJsonInterface {
        public string Type { get; private set; } = string.Empty;
        public List<DataField> Fields { get; private set; } = new List<DataField>();

        public void ToJsonObject(Hashtable ht) {
            throw new System.NotImplementedException();
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Type = ht.GetStringSafe(Keys.TYPE, Type);
            Fields = ht.ReadAsGenericList(Keys.FIELDS, DataFieldFactory.CreateFromHashtable);
        }
        
        private static class Keys {
            public const string TYPE = "Type";
            public const string FIELDS = "Fields";
        }
    }
}