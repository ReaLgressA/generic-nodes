using System.Collections;

namespace GenericNodes.Mech.Fields {
    public abstract class DataField {
        public string Name { get; private set; }

        public abstract DataType Type { get; } 
        
        protected DataField() {}
        
        protected DataField(string name) {
            Name = name;
        }

        public virtual DataField InitializeFromHashtable(Hashtable ht) {
            Name = ht.GetStringSafe("Name", Name);
            return this;
        }

        public abstract DataField Clone();

        protected DataField CloneBaseData(DataField field) {
            field.Name = Name;
            return field;
        }
    }
}