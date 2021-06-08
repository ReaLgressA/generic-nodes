namespace Mech.Fields {
    public abstract class DataField {
        public string Name { get; private set; }

        public abstract DataType Type { get; } 
        
        protected DataField(string name) {
            Name = name;
        }
    }
}