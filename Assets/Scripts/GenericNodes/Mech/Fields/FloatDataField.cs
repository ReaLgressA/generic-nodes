using System.Collections;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class FloatDataField : DataField {
        public float Value { get; private set; }

        public FloatDataField() {}
        public FloatDataField(string name, float defaultValue = 0f) : base(name) {
            Value = defaultValue;
        }

        public override DataType Type => DataType.Float;
        
        public void SetValue(float value) {
            Value = value;
        }
        
        public override DataField Construct(Hashtable ht) {
            Value = ht.GetFloat("Value", Value);
            return base.Construct(ht);
        }
        
        public override void FromJson(Hashtable ht, bool isAddition = false) {
            Value = ht.GetFloat(Name);
        }
        
        public override void ToJsonObject(Hashtable ht) {
            ht[Name] = Value;
        }
        
        public override DataField Clone() {
            FloatDataField field = new FloatDataField();
            field.Value = Value;
            return CloneBaseData(field);
        }
    }
}