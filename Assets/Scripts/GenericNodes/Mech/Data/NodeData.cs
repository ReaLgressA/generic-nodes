using System.Collections;
using System.Collections.Generic;
using GenericNodes.Mech.Fields;
using JsonParser;
using UnityEngine;

namespace GenericNodes.Mech.Data {
    public class NodeData : IJsonInterface {
        public Vector2 Position { get; set; }
        public List<DataField> Fields { get; private set; } = new List<DataField>();
        public NodeId NodeId { get; private set; } 
        public string NodeType { get; private set; }
        
        private GraphScheme Scheme { get; set; }

        public NodeData() {}

        public NodeData(GraphScheme scheme, string nodeType, NodeId nodeId, DataField[] fields) {
            Scheme = scheme;
            NodeType = nodeType;
            NodeId = nodeId;
            for (int i = 0; i < fields.Length; ++i) {
                Fields.Add(fields[i]);
            }
        }

        public void ToJsonObject(Hashtable ht) {
            ht[Keys.TYPE] = NodeType;
            ht[Keys.POSITION] = new SerializedVector2(Position);
            if (NodeId != NodeId.None) {
                var htNodeId = new Hashtable();
                NodeId.ToJsonObject(htNodeId);
                ht[Keys.NODE_ID] = htNodeId;
            }
            for (int i = 0; i < Fields.Count; ++i) {
                if (!Fields[i].IsOptional || Fields[i].IsOptionAllowed) {
                    Fields[i].ToJsonObject(ht);
                }
            }
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            NodeType = ht.GetString(Keys.TYPE);
            Position = ht.GetVector2(Keys.POSITION);

            if (ht.ContainsKey(Keys.NODE_ID)) {
                NodeId = new NodeId();
                NodeId.FromJson(ht[Keys.NODE_ID] as Hashtable);
            } else {
                NodeId = NodeId.None;
            }
            
            for (int i = 0; i < Fields.Count; ++i) {
                Fields[i].FromJson(ht);
            }
        }

        public void SetScheme(GraphScheme scheme, DataField[] fields) {
            Scheme = scheme;
            for (int i = 0; i < fields.Length; ++i) {
                Fields.Add(fields[i]);
            }
        }
        
        private class Keys {
            public const string POSITION = "Position";
            public const string TYPE = "Type";
            public const string NODE_ID = "NodeId";
        }
    }
}