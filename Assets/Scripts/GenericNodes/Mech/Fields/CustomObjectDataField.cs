using System.Collections;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.Extensions;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class CustomObjectDataField : DataField {

        private DataField[] fields = null;
        
        public DataField[] Fields => fields ??= Scheme.GetCustomDataTypeFields(ObjectType);
        public override DataType Type => DataType.CustomObject;
        
        public string ObjectType { get; private set; }
        public override bool IsOptionAllowed { get; set; } = false;
        private GraphScheme Scheme { get; }

        public CustomObjectDataField(GraphScheme scheme) {
            Scheme = scheme;
        }
        
        public CustomObjectDataField(GraphScheme scheme, string objectType) {
            Scheme = scheme;
            ObjectType = objectType;
        }

        public override DataField Construct(Hashtable ht) {
            ObjectType = ht.GetString(Keys.OBJECT_TYPE, ObjectType);
            return base.Construct(ht);
        }

        public override void ProcessDestruction() {
            for (int i = 0; i < Fields.Length; ++i) {
                Fields[i].ProcessDestruction();
            }
            base.ProcessDestruction();
        }

        public override void FromJson(Hashtable ht, bool isAddition = false) {
            if (Name == null) {
                ObjectType = ht.GetString(Keys.OBJECT_TYPE, ObjectType);
                for (int i = 0; i < Fields.Length; ++i) {
                    Fields[i].FromJson(ht);
                }
            } else {
                if (ht.ContainsKey(Name)) {
                    Hashtable htObject = ht[Name] as Hashtable;
                    ObjectType = htObject.GetString(Keys.OBJECT_TYPE, ObjectType);
                    for (int i = 0; i < Fields.Length; ++i) {
                        Fields[i].FromJson(htObject);
                    }
                    IsOptionAllowed = true;
                } else {
                    IsOptionAllowed = !IsOptional;
                }
            }
        }

        public override void ToJsonObject(Hashtable ht) {
            if (string.IsNullOrWhiteSpace(Name) || Name.Contains("#")) {
                ht[Keys.OBJECT_TYPE] = ObjectType;
                for (int i = 0; i < Fields.Length; ++i) {
                    Fields[i].ToJsonObject(ht);
                }
            }
            if (IsOptionAllowed) {
                Hashtable htObject = new Hashtable();
                htObject[Keys.OBJECT_TYPE] = ObjectType;
                for (int i = 0; i < Fields.Length; ++i) {
                    Fields[i].ToJsonObject(htObject);
                }
                ht[Name] = htObject;
            }
        }

        public override DataField Clone() {
            CustomObjectDataField field = new CustomObjectDataField(Scheme) {
                ObjectType = ObjectType,
                fields = Fields.CloneFields()
            };
            return CloneBaseData(field);
        }

        private static class Keys {
            public const string OBJECT_TYPE = "ObjectType";
        }
    }
}