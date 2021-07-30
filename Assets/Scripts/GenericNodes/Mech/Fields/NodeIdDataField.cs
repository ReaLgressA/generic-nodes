using System;
using System.Collections;
using GenericNodes.Mech.Data;
using JsonParser;

namespace GenericNodes.Mech.Fields {
    public class NodeIdDataField : DataField {
        public NodeId Value { get; private set; } = NodeId.None;

        public event Action<NodeIdDataField> ValueChanged;
        
        public NodeIdDataField() {}
        public NodeIdDataField(string name, NodeId defaultValue) : base(name) {
            Value = defaultValue;
        }

        public override DataType Type => DataType.NodeId;
        
        public void SetId(NodeId id) {
            Value = id;
            ValueChanged?.Invoke(this);
        }
        
        public override DataField Construct(Hashtable ht) {
            Value = ht.GetAs("Value", Value);
            return base.Construct(ht);
        }

        public override void ProcessDestruction() {
            base.ProcessDestruction();
            if (Value != NodeId.None) {
                Value = null;
                ValueChanged?.Invoke(this);
            }
        }

        public override void FromJson(Hashtable ht, bool isAddition = false) {
            Value = new NodeId(ht.GetInt32(Name));
        }
        
        public override void ToJsonObject(Hashtable ht) {
            ht[Name] = Value.Id;
        }
        
        public override DataField Clone() {
            NodeIdDataField node = new NodeIdDataField {Value = Value};
            return CloneBaseData(node);
        }
    }
}