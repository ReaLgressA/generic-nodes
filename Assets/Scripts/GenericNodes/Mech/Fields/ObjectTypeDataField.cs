using System;
using System.Collections;
using System.Linq;
using GenericNodes.Mech.Data;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class ObjectTypeDataField : DataField {
        public override DataType Type => DataType.ObjectType;
        public override bool IsOptionAllowed { get; set; } = false;
        
        public string[] AllowedTypes { get; set; }

        public int SelectedIndex => AllowedTypes.Contains(Value) ? Array.IndexOf(AllowedTypes, Value) : 0;
        
        public string Value { get; private set; }
        
        private GraphScheme Scheme { get; set; }

        public event Action<string> EventObjectTypeChanged;

        public ObjectTypeDataField(GraphScheme scheme) {
            Scheme = scheme;
        }

        public ObjectTypeDataField(GraphScheme scheme, string name, string defaultValue, string[] allowedTypes) : base(name) {
            Scheme = scheme;
            AllowedTypes = allowedTypes;
            if (defaultValue != null) {
                Value = defaultValue;
            }
        }

        public void SetValue(int selectedIndex) {
            if (Value.Equals(AllowedTypes[selectedIndex], StringComparison.Ordinal)) {
                return;
            }
            Value = AllowedTypes[selectedIndex];
            EventObjectTypeChanged?.Invoke(Value);
        }
        
        public override DataField Construct(Hashtable ht) {
            //This may not be even used for it
            Value = ht.GetStringSafe("Type", Value);
            return base.Construct(ht);
        }
        
        public override void FromJson(Hashtable ht, bool isAddition = false) {
            Value = ht.GetStringSafe(Name, Value);
        }
        
        public override void ToJsonObject(Hashtable ht) {
            if (!IsOptional || IsOptionAllowed) {
                ht[Name] = Value;
            }
        }
        
        public override DataField Clone() {
            ObjectTypeDataField field = new ObjectTypeDataField(Scheme);
            field.AllowedTypes = AllowedTypes;
            field.Value = Value;
            return CloneBaseData(field);
        }
    }
}