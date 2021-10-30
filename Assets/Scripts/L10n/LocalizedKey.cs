using System.Collections;
using JsonParser;

namespace L10n {
    public class LocalizedKey : IJsonInterface {
        public string Key { get; set; }
        public string Value { get; set; }

        public void ToJsonObject(Hashtable ht) {
            ht.Add(Key, Value);
        }
        
        public void FromJson(Hashtable ht, bool isAddition = false) {
            IDictionaryEnumerator current = ht.GetEnumerator();
            if (current.MoveNext()) {
                Key = current.Key as string;
                Value = current.Value as string;
            }
        }
    }
}