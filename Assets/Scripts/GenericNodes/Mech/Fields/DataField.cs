using System.Collections;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public abstract class DataField : IJsonInterface {
        public string Name { get; set; }

        public abstract DataType Type { get; }
        public bool IsOptional { get; private set; } = false;
        public abstract bool IsOptionAllowed { get; set; }
        
        protected DataField() {}
        
        protected DataField(string name) {
            Name = name;
        }

        public virtual DataField Construct(Hashtable ht) {
            Name = ht.GetStringSafe(Keys.NAME, Name);
            IsOptional = ht.GetBool(Keys.IS_OPTIONAL, IsOptional);
            return this;
        }

        public virtual void ProcessDestruction() {}

        public abstract void FromJson(Hashtable ht, bool isAddition = false);
        
        public abstract void ToJsonObject(Hashtable ht);

        public abstract DataField Clone();

        protected DataField CloneBaseData(DataField field) {
            field.Name = Name;
            field.IsOptional = IsOptional;
            return field;
        }

        private static class Keys {
            public const string NAME = "Name";
            public const string IS_OPTIONAL = "IsOptional";
        }
    }
}