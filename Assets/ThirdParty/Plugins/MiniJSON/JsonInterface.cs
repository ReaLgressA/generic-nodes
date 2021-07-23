using System;
using System.Collections;

namespace MiniJSON {
    [Serializable]
    abstract public class JsonInterface {

        public JsonInterface() {
        }

        public JsonInterface(Hashtable ht) {
            if (ht != null) FromJson(ht);
        }

        public virtual string ToJsonString() {
            Hashtable ht = new Hashtable();
            ToJsonObject(ht);
            return MiniJsonExtensions.ToJson(ht);
        }

        public abstract void ToJsonObject(Hashtable json);

        public virtual void FromJson(string json) {
            Hashtable ht = MiniJsonExtensions.HashtableFromJson(json);
            FromJson(ht);
        }

        abstract public void FromJson(Hashtable json);

#if UNITY_EDITOR
        public virtual void FromXml(System.Xml.XmlNode node) {
        }
#endif
    }

}