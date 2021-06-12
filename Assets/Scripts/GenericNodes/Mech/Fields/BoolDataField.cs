using System.Collections;

namespace Mech.Fields {
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

        public override DataField InitializeFromHashtable(Hashtable ht) {
            Value = ht.GetBool("Value", Value);
            return base.InitializeFromHashtable(ht);
        }

        public override DataField Clone() {
            var field = new BoolDataField();
            field.Value = Value;
            return CloneBaseData(field);
        }
    }
}