using System.Collections;

namespace Mech.Data {
    public class EnumDescription : IJsonInterface {
        private class Keys {
            public const string TYPE = "Type";
            public const string ENUMERATION = "Enumeration";
        }

        public string Type { get; private set; } = string.Empty;
        public string[] Enumeration { get; private set; } = new string[0];
        
        public void ToJsonObject(Hashtable ht) {
            throw new System.NotImplementedException();
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Type = ht.GetStringSafe(Keys.TYPE, Type);
            Enumeration = ht.GetArray(Keys.ENUMERATION, Enumeration);
        }
    }
}