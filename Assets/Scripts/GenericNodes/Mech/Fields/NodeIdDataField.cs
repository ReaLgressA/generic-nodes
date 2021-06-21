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
        
        public override DataField Construct(Hashtable ht) {
            Value = ht.GetAs("Value", Value);
            return base.Construct(ht);
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