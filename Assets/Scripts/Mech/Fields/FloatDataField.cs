using System.Collections;

namespace Mech.Fields {
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
        
        public override DataField InitializeFromHashtable(Hashtable ht) {
            Value = ht.GetFloat("Value", Value);
            return base.InitializeFromHashtable(ht);
        }
        
        public override DataField Clone() {
            var field = new FloatDataField();
            field.Value = Value;
            return CloneBaseData(field);
        }
    }
}