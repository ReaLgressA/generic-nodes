namespace GenericNodes.Mech.Fields {
    public class TextDataField : StringDataField {
        private const string DefaultValue = "";
        public override DataType Type => DataType.Text;

        public TextDataField() {}
        public TextDataField(string name, string defaultValue = DefaultValue) : base(name, defaultValue) { }
        
        public override DataField Clone() {
            var field = new TextDataField();
            field.Value = Value;
            return CloneBaseData(field);
        }
    }
}