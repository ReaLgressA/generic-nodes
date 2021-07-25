using System.Collections;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class BoolDataField : DataField {
        public bool Value { get; private set; }
        
        public BoolDataField() {}
        public BoolDataField(string name, bool defaultValue = false) : base(name) {
            Value = defaultValue;
        }

        public override DataType Type => DataType.Bool;

        public void SetValue(bool value) {
            Value = value;
        }

        public override DataField Construct(Hashtable ht) {
            Value = ht.GetBool("Value", Value);
            return base.Construct(ht);
        }

        public override void FromJson(Hashtable ht, bool isAddition = false) {
            Value = ht.GetBool(Name);
        }

        public override void ToJsonObject(Hashtable ht) {
            ht[Name] = Value;
        }

        public override DataField Clone() {
            BoolDataField field = new BoolDataField { Value = Value };
            return CloneBaseData(field);
        }
    }
}