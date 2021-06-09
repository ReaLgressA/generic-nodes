namespace Mech.Fields {
    public class BoolDataField : DataField {
        
        public bool Value { get; private set; }
        
        public BoolDataField(string name, bool defaultValue = false) : base(name) {
            Value = defaultValue;
        }

        public override DataType Type => DataType.Bool;

        public void SetValue(bool value) {
            Value = value;
        }
    }
}