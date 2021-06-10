using System.Collections;
using System.Collections.Generic;
using Mech.Fields;

namespace Mech.Data {
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
            DataField[] fields = new DataField[Fields.Count];
            for (int i = 0; i < fields.Length; ++i) {
                fields[i] = Fields[i].Clone();
            }
            NodeData graphInfo = new NodeData(fields);
            return new GraphData(Type, graphInfo);
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
    }
}