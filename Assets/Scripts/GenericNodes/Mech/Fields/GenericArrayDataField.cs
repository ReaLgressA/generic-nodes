using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.Extensions;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class GenericArrayDataField : DataField {
        private class Keys {
            public const string ARRAY_TYPE = "ArrayType";
            public const string MAX_CAPACITY = "MaxCapacity";
        }
        
        public override DataType Type => DataType.GenericArray;
        public string ArrayType { get; private set; }
        public int MaxCapacity { get; private set; } = 0;
        public List<CustomObjectDataField> Elements { get; private set; }
        public bool CanAddElement => Elements.Count < MaxCapacity || MaxCapacity == 0;
        private GraphScheme Scheme { get; }
        
        public event Action ElementsUpdated;

        public NodeDescription CustomDataType => 
            Scheme.CustomDataTypes.First(dataType => string.Equals(dataType.Type, ArrayType,
                                                                   StringComparison.Ordinal));
        
        public GenericArrayDataField(GraphScheme scheme) {
            Scheme = scheme;
        }
        
        public GenericArrayDataField(GraphScheme scheme, string name, string arrayType, int maxCapacity = 0) : base(name) {
            Scheme = scheme;
            ArrayType = arrayType;
            MaxCapacity = maxCapacity;
            Elements = new List<CustomObjectDataField>();
        }

        public void AddElement() {
            if (CanAddElement) {
                Elements.Add(new CustomObjectDataField(Scheme, ArrayType));
                ElementsUpdated?.Invoke();
            }
        }

        public void RemoveElementAt(int index) {
            if (index >= 0 && index < Elements.Count) {
                Elements[index].ProcessDestruction();
                Elements.RemoveAt(index);
                ElementsUpdated?.Invoke();
            }
        }
        
        public override DataField Construct(Hashtable ht) {
            ArrayType = ht.GetString(Keys.ARRAY_TYPE);
            MaxCapacity = ht.GetInt32(Keys.MAX_CAPACITY, MaxCapacity);
            Elements = new List<CustomObjectDataField>();
            return base.Construct(ht);
        }

        public override void ProcessDestruction() {
            for (int i = 0; i < Elements.Count; i++) {
                Elements[i].ProcessDestruction();
            }
            base.ProcessDestruction();
        }

        public override void FromJson(Hashtable ht, bool isAddition = false) {
            if (ht.Contains(Name) && ht[Name] != null) {
                if (ht[Name] is ArrayList array) {
                    Elements = new List<CustomObjectDataField>(array.Count);
                    for (int i = 0; i < array.Count; i++) {
                        CustomObjectDataField item = new CustomObjectDataField(Scheme);
                        if (array[i] != null) {
                            Hashtable htDataField = (Hashtable)array[i];
                            item.FromJson(htDataField);
                        }
                        Elements.Add(item);
                    }
                }
            }
            ElementsUpdated?.Invoke();
        }
        
        public override void ToJsonObject(Hashtable ht) {
            ht[Name] = Elements;
        }
        
        public override DataField Clone() {
            GenericArrayDataField field = new GenericArrayDataField(Scheme, Name, ArrayType) {
                Elements = Elements.CloneElements()
            };
            return CloneBaseData(field);
        }
    }
}