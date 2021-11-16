using System;
using System.Collections;
using System.Collections.Generic;
using GenericNodes.Mech.Data;
using JsonParser;
using UnityEngine;

namespace GenericNodes.Mech.Fields {
    public abstract class AbstractArrayDataField : DataField {
        public override DataType Type => DataType.GenericArray;
        public override bool IsOptionAllowed { get; set; } = true;
        public int MaxCapacity { get; private set; } = 0;
        public int MinCapacity { get; private set; } = 0;
        public List<CustomObjectDataField> Elements { get; protected set; }
        public bool CanAddElement => Elements.Count < MaxCapacity || MaxCapacity == 0;
        public bool CanRemoveElement => Elements.Count > MinCapacity;
        public abstract string DefaultElementType { get; }
        public abstract string[] AllowedElementTypes { get; }
        protected GraphScheme Scheme { get; }
        
        public event Action ElementsUpdated;

        public AbstractArrayDataField(GraphScheme scheme) {
            Scheme = scheme;
        }
        
        public AbstractArrayDataField(GraphScheme scheme, string name, 
                                      int minCapacity = 0, int maxCapacity = 0) : base(name) {
            Scheme = scheme;
            MaxCapacity = maxCapacity;
            MinCapacity = minCapacity;
            Elements = new List<CustomObjectDataField>();
            while (Elements.Count < MinCapacity) {
                AddElement(DefaultElementType);
            }
        }

        public void AddElement() {
            AddElement(DefaultElementType);
        }
        
        public void AddElement(string arrayElementType) {
            if (CanAddElement) {
                Elements.Add(new CustomObjectDataField(Scheme, arrayElementType, AllowedElementTypes));
                ElementsUpdated?.Invoke();
            }
            while (Elements.Count < MinCapacity) {
                AddElement(DefaultElementType);    
            }
        }

        public void RemoveElementAt(int index) {
            if (CanRemoveElement && index >= 0 && index < Elements.Count) {
                Elements[index].ProcessDestruction();
                Elements.RemoveAt(index);
                ElementsUpdated?.Invoke();
            }
        }
        
        public override DataField Construct(Hashtable ht) {
            MaxCapacity = ht.GetInt32(Keys.MAX_CAPACITY, MaxCapacity);
            if (MaxCapacity < 0) {
                MaxCapacity = 0;
            }
            MinCapacity = ht.GetInt32(Keys.MIN_CAPACITY, MinCapacity);
            MinCapacity = Mathf.Clamp(MinCapacity, 0, MaxCapacity > 0 ? 0 : MaxCapacity);
            
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
                        CustomObjectDataField item = new CustomObjectDataField(Scheme, DefaultElementType, AllowedElementTypes);
                        if (array[i] != null) {
                            Hashtable htDataField = (Hashtable)array[i];
                            if (htDataField == null) {
                                item.IsOptionAllowed = !item.IsOptional;    
                            } else {
                                item.FromJson(htDataField);   
                            }
                        } else {
                            item.IsOptionAllowed = !item.IsOptional;
                        }
                        Elements.Add(item);
                    }
                }
            }
            ElementsUpdated?.Invoke();
        }

        private static class Keys {
            public const string MAX_CAPACITY = "MaxCapacity";
            public const string MIN_CAPACITY = "MinCapacity";
        }
    }
}