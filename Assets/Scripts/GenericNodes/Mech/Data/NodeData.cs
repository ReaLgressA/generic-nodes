using System.Collections.Generic;
using GenericNodes.Mech.Fields;

namespace GenericNodes.Mech.Data {
    public class NodeData {
        public List<DataField> Fields { get; private set; } = new List<DataField>();
        public string NodeType { get; }
        
        public NodeData(string nodeType, DataField[] fields) {
            NodeType = nodeType;
            for (int i = 0; i < fields.Length; ++i) {
                Fields.Add(fields[i]);
            }
        }
    }
}