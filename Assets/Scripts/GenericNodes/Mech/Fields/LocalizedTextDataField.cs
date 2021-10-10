namespace GenericNodes.Mech.Fields {
    public class LocalizedTextDataField : StringDataField {
        private const string DEFAULT_VALUE = "";
        public override DataType Type => DataType.LocalizedText;

        public LocalizedTextDataField() {}
        public LocalizedTextDataField(string name, string defaultValue = DEFAULT_VALUE) : base(name, defaultValue) { }
        
        public override DataField Clone() {
            var field = new LocalizedTextDataField {
                Value = Value
            };
            return CloneBaseData(field);
        }
    }
}