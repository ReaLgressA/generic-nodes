using System.Collections;

namespace GenericNodes.Mech.Fields {
    public abstract class DataField : IJsonInterface {
        public string Name { get; private set; }

        public abstract DataType Type { get; } 
        
        protected DataField() {}
        
        protected DataField(string name) {
            Name = name;
        }

        public virtual DataField InitializeFromHashtable(Hashtable ht) {
            Name = ht.GetStringSafe(Keys.NAME, Name);
            return this;
        }

        public abstract void FromJson(Hashtable ht, bool isAddition = false);
        
        public abstract void ToJsonObject(Hashtable ht);
        

        public abstract DataField Clone();

        protected DataField CloneBaseData(DataField field) {
            field.Name = Name;
            return field;
        }

        private class Keys {
            public const string NAME = "Name";
        }
    }
}