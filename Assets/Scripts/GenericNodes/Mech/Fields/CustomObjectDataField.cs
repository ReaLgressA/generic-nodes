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
            ObjectType = ht.GetString(Keys.OBJECT_TYPE, ObjectType);
            for (int i = 0; i < Fields.Length; ++i) {
                Fields[i].FromJson(ht);
            }
        }

        public override void ToJsonObject(Hashtable ht) {
            ht[Keys.OBJECT_TYPE] = ObjectType;
            for (int i = 0; i < Fields.Length; ++i) {
                Fields[i].ToJsonObject(ht);
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