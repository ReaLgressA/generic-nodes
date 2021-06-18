namespace GenericNodes.Mech.Fields {
    public class TextDataField : StringDataField {
        private const string DEFAULT_VALUE = "";
        public override DataType Type => DataType.Text;

        public TextDataField() {}
        public TextDataField(string name, string defaultValue = DEFAULT_VALUE) : base(name, defaultValue) { }
        
        public override DataField Clone() {
            var field = new TextDataField();
            field.Value = Value;
            return CloneBaseData(field);
        }
    }
}