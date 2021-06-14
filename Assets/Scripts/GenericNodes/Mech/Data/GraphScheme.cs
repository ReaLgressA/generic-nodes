using System;
using System.Collections;
using System.Collections.Generic;
using GenericNodes.Mech.Extensions;
using GenericNodes.Mech.Fields;

namespace GenericNodes.Mech.Data {
    public class GraphScheme : IJsonInterface {
        private class Keys {
            public const string TYPE = "Type";
            public const string FIELDS = "Fields";
            public const string NODE_ARRAY = "NodeArray";
            public const string NODES = "Nodes";
            public const string ENUMS = "Enums";
        }

        public string Type { get; private set; } = string.Empty;
        public List<DataField> Fields { get; private set; } = new List<DataField>();
        public EnumDescription[] Enums { get; private set; } = new EnumDescription[0];
        public NodeDescription[] Nodes { get; private set; } = new NodeDescription[0];
        public string NodeArrayName { get; private set; } = null;

        public GraphData CreateGraph() {
            NodeData graphInfo = new NodeData(Fields.CloneFields());
            return new GraphData(Type, graphInfo, this);
        }

        public void ToJsonObject(Hashtable ht) {
            throw new System.NotImplementedException();
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Type = ht.GetStringSafe(Keys.TYPE, Type);
            Enums = ht.GetArray(Keys.ENUMS, Enums);
            DataFieldFactory.CurrentGraphScheme = this;
            Fields = ht.ReadAsGenericList(Keys.FIELDS, DataFieldFactory.CreateFromHashtable);
            NodeArrayName = ht.GetStringSafe(Keys.NODE_ARRAY, NodeArrayName);
            Nodes = ht.GetArray(Keys.NODES, Nodes);
        }

        public NodeData CreateNodeData(string nodeType) {
            for (int i = 0; i < Nodes.Length; ++i) {
                if (string.Compare(Nodes[i].Type, nodeType, StringComparison.Ordinal) == 0) {
                    return new NodeData(Nodes[i].Fields.CloneFields());
                }
            }
            return null;
        }
        
    }
}