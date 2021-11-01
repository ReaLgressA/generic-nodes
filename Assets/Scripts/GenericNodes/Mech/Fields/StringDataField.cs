using System.Collections;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class StringDataField : DataField {
        
        public override DataType Type => DataType.String;
        public override bool IsOptionAllowed { get; set; } = false;
        public string Value { get; protected set; }

        public StringDataField() {}
        public StringDataField(string name, string defaultValue = "") : base(name) {
            Value = defaultValue;
        }

        public virtual void SetValue(string value) {
            Value = value;
        }
        
        public override DataField Construct(Hashtable ht) {
            Value = ht.GetStringSafe("Value", Value);
            return base.Construct(ht);
        }

        public override void FromJson(Hashtable ht, bool isAddition = false) {
            IsOptionAllowed = ht.ContainsKey(Name);
            Value = ht.GetString(Name);
        }
        
        public override void ToJsonObject(Hashtable ht) {
            if (!IsOptional || IsOptionAllowed) {
                ht[Name] = Value;
            }
        }
        
        public override DataField Clone() {
            var field = new StringDataField();
            field.Value = Value;
            return CloneBaseData(field);
        }
    }
}