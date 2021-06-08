namespace Mech.Fields {
    public class StringDataField : DataField {
        public string Value { get; private set; }
        
        public override DataType Type => DataType.String;
        
        public StringDataField(string name, string defaultValue = "") : base(name) {
            Value = defaultValue;
        }

        public void SetValue(string value) {
            Value = value;
        }
    }
}