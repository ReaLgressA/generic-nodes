namespace Mech.Fields {
    public class FloatDataField : DataField {

        public float Value { get; private set; }
        
        public FloatDataField(string name, float defaultValue = 0f) : base(name) {
            Value = defaultValue;
        }

        public override DataType Type => DataType.Float;
        
        public void SetValue(float value) {
            Value = value;
        }
    }
}