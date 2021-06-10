using System;
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

        public int GetIndex(string enumKey) {
            for (int i = 0; i < Enumeration.Length; ++i) {
                if (string.Compare(Enumeration[i], enumKey, StringComparison.Ordinal) == 0) {
                    return i;
                }   
            }
            return 0;
        }
    }
}