using System.Collections;
using GenericNodes.Mech.Data;

namespace GenericNodes.Mech.Fields {
    public class NodeIdDataField : DataField {
        public NodeId Value { get; private set; } = NodeId.None;
        
        public NodeIdDataField() {}
        public NodeIdDataField(string name, NodeId defaultValue) : base(name) {
            Value = defaultValue;
        }

        public override DataType Type => DataType.NodeId;
        
        public void SetId(NodeId id) {
            Value = id;
        }
        
        public override DataField InitializeFromHashtable(Hashtable ht) {
            Value = ht.GetAs("Value", Value);
            return base.InitializeFromHashtable(ht);
        }
        
        public override DataField Clone() {
            NodeIdDataField node = new NodeIdDataField {Value = Value};
            return node;
        }
    }
}