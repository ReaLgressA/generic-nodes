using System;
using System.Collections;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.Extensions;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class CustomObjectDataField : DataField {

        private DataField[] fields = null;
        
        public DataField[] Fields => fields ??= CreateFieldsForCurrentObjectType();

        public override DataType Type => DataType.CustomObject;
        
        public string ObjectType { get; private set; }
        public string[] AllowedObjectTypes { get; set; }
        public override bool IsOptionAllowed { get; set; } = false;
        private GraphScheme Scheme { get; }
        private ObjectTypeDataField ObjectTypeField { get; set; } = null;

        public event Action EventFieldsUpdated;

        public CustomObjectDataField(GraphScheme scheme, string objectType = null, string[] allowedObjectTypes = null) {
            Scheme = scheme;
            ObjectType = objectType;
            AllowedObjectTypes = allowedObjectTypes;
        }

        public override DataField Construct(Hashtable ht) {
            if (ObjectType == null) {
                ObjectType = ht.GetString(Keys.OBJECT_TYPE, ObjectType);
            }
            if (AllowedObjectTypes == null) {
                AllowedObjectTypes = new[] { ObjectType };
            }
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
                if (ObjectType == null) {
                    ObjectType = ht.GetString(Keys.OBJECT_TYPE, ObjectType);
                }
                if (AllowedObjectTypes == null) {
                    
                    AllowedObjectTypes = new[] { ObjectType };
                }
                for (int i = 0; i < Fields.Length; ++i) {
                    Fields[i].FromJson(ht);
                }
            } else {
                if (ht.ContainsKey(Name)) {
                    Hashtable htObject = ht[Name] as Hashtable;

                    if (ObjectType == null) {
                        ObjectType = htObject.GetString(Keys.OBJECT_TYPE, ObjectType);
                    }
                    if (AllowedObjectTypes == null) {
                        AllowedObjectTypes = new[] { ObjectType };
                    }
                    
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
                // ht[Keys.OBJECT_TYPE] = ObjectType;
                for (int i = 0; i < Fields.Length; ++i) {
                    Fields[i].ToJsonObject(ht);
                }
            }
            if (IsOptionAllowed) {
                Hashtable htObject = new Hashtable();
                // htObject[Keys.OBJECT_TYPE] = ObjectType;
                for (int i = 0; i < Fields.Length; ++i) {
                    Fields[i].ToJsonObject(htObject);
                }
                ht[Name] = htObject;
            }
        }

        public override DataField Clone() {
            CustomObjectDataField field = new CustomObjectDataField(Scheme, ObjectType, AllowedObjectTypes) {
                ObjectType = ObjectType,
                fields = Fields.CloneFields()
            };
            return CloneBaseData(field);
        }
        
        private DataField[] CreateFieldsForCurrentObjectType() {
            DataField[] objectFields = Scheme.GetCustomDataTypeFields(ObjectType);

            if (AllowedObjectTypes.Length > 1) {
                if (ObjectTypeField != null) {
                    ObjectTypeField.EventObjectTypeChanged -= UpdateObjectType;
                    ObjectTypeField = null;
                    for (int i = 0; i < Fields.Length; ++i) {
                        Fields[i].ProcessDestruction();
                    }
                }
                fields = new DataField[objectFields.Length + 1];
                ObjectTypeField = new ObjectTypeDataField(Scheme, "Type", ObjectType);
                ObjectTypeField.EventObjectTypeChanged += UpdateObjectType;
                fields[0] = ObjectTypeField;
                for (int i = 0; i < objectFields.Length; ++i) {
                    fields[i + 1] = objectFields[i];
                }
                return fields;
            }
            return objectFields;
        }

        private void UpdateObjectType(string objectType) {
            ObjectType = objectType;
            
            fields = CreateFieldsForCurrentObjectType();
            
            EventFieldsUpdated?.Invoke();
        }

        private static class Keys {
            public const string OBJECT_TYPE = "ObjectType";
        }
    }
}