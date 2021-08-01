using System;
using System.Collections;
using System.Linq;
using GenericNodes.Mech.Data;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class EnumDataField : DataField {
        public override DataType Type => DataType.Enum;
        public override bool IsOptionAllowed { get; set; } = false;

        public string EnumSet { get; private set; }
        public EnumDescription EnumDescription => 
            Scheme.Enums.First(enumDescription => string.Compare(enumDescription.Type, 
                                                                 EnumSet, StringComparison.Ordinal) == 0);
        
        public int SelectedIndex => EnumDescription.GetIndex(Value);
        
        public string Value { get; private set; }
        
        private GraphScheme Scheme { get; set; }

        public EnumDataField(GraphScheme scheme) {
            Scheme = scheme;
        }

        public EnumDataField(GraphScheme scheme, string name, string defaultValue = null) : base(name) {
            Scheme = scheme;
            if (defaultValue != null) {
                Value = defaultValue;
            }
        }

        public void SetValue(int selectedIndex) {
            Value = EnumDescription.Enumeration[selectedIndex];
        }
        
        public override DataField Construct(Hashtable ht) {
            EnumSet = ht.GetStringSafe("EnumType", EnumSet);
            Value = ht.GetStringSafe("Value", Value);
            return base.Construct(ht);
        }
        
        public override void FromJson(Hashtable ht, bool isAddition = false) {
            IsOptionAllowed = ht.ContainsKey(Name);
            Value = ht.GetStringSafe(Name, Value);
        }
        
        public override void ToJsonObject(Hashtable ht) {
            if (!IsOptional || IsOptionAllowed) {
                ht[Name] = Value;
            }
        }
        
        public override DataField Clone() {
            EnumDataField field = new EnumDataField(Scheme);
            field.EnumSet = EnumSet;
            field.Value = Value;
            return CloneBaseData(field);
        }
    }
}