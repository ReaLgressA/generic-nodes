using System;
using System.Collections;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.Extensions;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class GenericMultiTypeArrayDataField : AbstractArrayDataField {

        public string[] AllowedTypes { get; private set; } = Array.Empty<string>();

        public override string DefaultElementType => AllowedTypes[0];
        
        // public NodeDescription CustomDataType => 
        //     Scheme.CustomDataTypes.First(dataType => string.Equals(dataType.Type, ArrayTypes, StringComparison.Ordinal));
        
        public GenericMultiTypeArrayDataField(GraphScheme scheme) : base(scheme) { }

        public GenericMultiTypeArrayDataField(GraphScheme scheme, string name, string[] allowedTypes,
                                              int minCapacity = 0, int maxCapacity = 0)
            : base(scheme, name, minCapacity, maxCapacity) {
            AllowedTypes = allowedTypes;
        }

        public override DataField Construct(Hashtable ht) {
            AllowedTypes = ht.GetArray(Keys.ALLOWED_TYPES, AllowedTypes);
            return base.Construct(ht);
        }

        public override void ToJsonObject(Hashtable ht) {
            ht[Name] = Elements;
        }

        public override DataField Clone() {
            GenericMultiTypeArrayDataField field = new GenericMultiTypeArrayDataField(Scheme, Name, AllowedTypes,
                MinCapacity, MaxCapacity) {
                Elements = Elements.CloneElements()
            };
            return CloneBaseData(field);
        }
        
        private static class Keys {
            public const string ALLOWED_TYPES = "AllowedTypes";
        }
    }
}
