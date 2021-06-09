using System;
using System.Collections;
using Mech.Data;

namespace Mech.Fields {
    public class EnumDataField : DataField {
        public override DataType Type => DataType.Enum;

        public string[] EnumSet { get; private set; }
        public int SelectedIndex { get; private set; } = 0;
        public string Value => EnumSet[SelectedIndex];
        
        private GraphScheme Scheme { get; set; }

        public EnumDataField(GraphScheme scheme) {
            Scheme = scheme;
        }

        public EnumDataField(GraphScheme scheme, string name, string defaultValue = null) : base(name) {
            Scheme = scheme;
            if (defaultValue != null) {
                for (int i = 0; i < EnumSet.Length; ++i) {
                    if (string.Compare(EnumSet[i], defaultValue, StringComparison.Ordinal) == 0) {
                        SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        public void SetValue(int selectedIndex) {
            SelectedIndex = selectedIndex;
        }
        
        public override DataField InitializeFromHashtable(Hashtable ht) {
            ht.GetStringSafe("Value");
            return base.InitializeFromHashtable(ht);
        }
        
        public override DataField Clone() {
            var field = new FloatDataField();
            //field.Value = Value;
            return CloneBaseData(field);
        }
    }
}