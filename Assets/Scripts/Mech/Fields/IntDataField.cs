namespace Mech.Fields {
    public class IntDataField : DataField {

        public int Value { get; private set; }
        
        public IntDataField(string name, int defaultValue = 0) : base(name) {
            Value = defaultValue;
        }

        public override DataType Type => DataType.Int;
        
        public void SetValue(int value) {
            Value = value;
        }
    }
}