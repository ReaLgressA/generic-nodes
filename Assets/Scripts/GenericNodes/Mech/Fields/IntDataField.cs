using System.Collections;

namespace GenericNodes.Mech.Fields {
    public class IntDataField : DataField {

        public int Value { get; private set; }
        
        public IntDataField() {}
        public IntDataField(string name, int defaultValue = 0) : base(name) {
            Value = defaultValue;
        }

        public override DataType Type => DataType.Int;
        
        public void SetValue(int value) {
            Value = value;
        }
        
        public override DataField InitializeFromHashtable(Hashtable ht) {
            Value = ht.GetInt32("Value", Value);
            return base.InitializeFromHashtable(ht);
        }
        
        public override DataField Clone() {
            var field = new IntDataField();
            field.Value = Value;
            return CloneBaseData(field);
        }
    }
}