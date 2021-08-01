using System.Collections;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class IntDataField : DataField {
        
        public override bool IsOptionAllowed { get; set; } = true;
        public override DataType Type => DataType.Int;
        public int Value { get; private set; }
        
        public IntDataField() {}
        public IntDataField(string name, int defaultValue = 0) : base(name) {
            Value = defaultValue;
        }

        public void SetValue(int value) {
            Value = value;
        }
        
        public override DataField Construct(Hashtable ht) {
            Value = ht.GetInt32("Value", Value);
            return base.Construct(ht);
        }
        
        public override void FromJson(Hashtable ht, bool isAddition = false) {
            Value = ht.GetInt32(Name);
        }

        public override void ToJsonObject(Hashtable ht) {
            ht[Name] = Value;
        }

        public override DataField Clone() {
            IntDataField field = new IntDataField {Value = Value};
            return CloneBaseData(field);
        }
    }
}