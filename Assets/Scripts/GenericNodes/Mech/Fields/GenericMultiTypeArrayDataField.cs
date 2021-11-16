using System;
using System.Collections;
using System.Collections.Generic;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.Extensions;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class GenericMultiTypeArrayDataField : AbstractArrayDataField {

        public string[] AllowedTypes { get; private set; } = Array.Empty<string>();

        public override string DefaultElementType => AllowedTypes[0];
        public override string[] AllowedElementTypes => AllowedTypes;

        public GenericMultiTypeArrayDataField(GraphScheme scheme) : base(scheme) { }

        public GenericMultiTypeArrayDataField(GraphScheme scheme, string name, string[] allowedTypes,
                                              int minCapacity = 0, int maxCapacity = 0)
            : base(scheme, name, minCapacity, maxCapacity) {
            AllowedTypes = allowedTypes;
        }

        public override DataField Construct(Hashtable ht) {
            base.Construct(ht);
            AllowedTypes = ht.GetArray(Keys.ALLOWED_TYPES, AllowedTypes);
            
            Elements = new List<CustomObjectDataField>();
            while (Elements.Count < MinCapacity) {
                AddElement(DefaultElementType);
            }

            return this;
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
