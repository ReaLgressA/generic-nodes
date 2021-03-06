using System;
using System.Collections;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class SpriteAssetDataField : DataField {
        
        public override bool IsOptionAllowed { get; set; } = true;
        public override DataType Type => DataType.SpriteAsset;
        public string Value { get; private set; }
        public string PackageLabel { get; private set; } = null;

        public event Action ValueChanged;
        
        public SpriteAssetDataField() {}
        
        public SpriteAssetDataField(string name, string packageLabel, string defaultValue = "") : base(name) {
            PackageLabel = packageLabel;
            Value = defaultValue;
        }

        public virtual void SetValue(string value) {
            Value = value;
            ValueChanged?.Invoke();
        }
        
        public override DataField Construct(Hashtable ht) {
            Value = ht.GetStringSafe("Value", Value);
            PackageLabel = ht.GetStringSafe("PackageLabel", PackageLabel);
            return base.Construct(ht);
        }

        public override void FromJson(Hashtable ht, bool isAddition = false) {
            Value = ht.GetString(Name);
        }
        
        public override void ToJsonObject(Hashtable ht) {
            ht[Name] = Value;
        }
        
        public override DataField Clone() {
            SpriteAssetDataField field = new SpriteAssetDataField {
                Value = Value, 
                PackageLabel = PackageLabel
            };
            return CloneBaseData(field);
        }
    }
}