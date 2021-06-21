using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericNodes.Mech.Data;
using GenericNodes.Mech.Extensions;

namespace GenericNodes.Mech.Fields {
    public class GenericArrayDataField : DataField {
        private class Keys {
            public const string ARRAY_TYPE = "ArrayType";
            public const string MAX_CAPACITY = "MaxCapacity";
        }
        
        public override DataType Type => DataType.GenericArray;
        public string ArrayType { get; private set; }
        public int MaxCapacity { get; private set; } = 0;
        public List<GenericArrayElement> Elements { get; private set; }
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
            Elements = new List<GenericArrayElement>();
        }

        public void AddElement() {
            if (CanAddElement) {
                Elements.Add(new GenericArrayElement(this));
                ElementsUpdated?.Invoke();
            }
        }

        public void RemoveElementAt(int index) {
            if (index >= 0 && index < Elements.Count) {
                Elements.RemoveAt(index);
                ElementsUpdated?.Invoke();
            }
        }
        
        public override DataField Construct(Hashtable ht) {
            ArrayType = ht.GetString(Keys.ARRAY_TYPE);
            MaxCapacity = ht.GetInt32(Keys.MAX_CAPACITY, MaxCapacity);
            return base.Construct(ht);
        }
        
        public override void FromJson(Hashtable ht, bool isAddition = false) {
            //TODO: Generic array deserealization
        }
        
        public override void ToJsonObject(Hashtable ht) {
            //TODO: Generic array serialization
            //ht[Name] = ;
        }
        
        public override DataField Clone() {
            GenericArrayDataField field = new GenericArrayDataField(Scheme, Name, ArrayType);
            field.Elements = field.Elements.CloneElements(field);
            return CloneBaseData(field);
        }
    }
}