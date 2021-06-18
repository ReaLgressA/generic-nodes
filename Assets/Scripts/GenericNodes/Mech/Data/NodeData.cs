using System.Collections;
using System.Collections.Generic;
using GenericNodes.Mech.Fields;
using UnityEditor.SearchService;
using UnityEngine;

namespace GenericNodes.Mech.Data {
    public class NodeData : IJsonInterface {
        public Vector2 Position { get; set; }
        public List<DataField> Fields { get; private set; } = new List<DataField>();
        public string NodeType { get; private set; }
        
        private GraphScheme Scheme { get; set; }
        
        public NodeData(GraphScheme scheme, string nodeType, DataField[] fields) {
            Scheme = scheme;
            NodeType = nodeType;
            for (int i = 0; i < fields.Length; ++i) {
                Fields.Add(fields[i]);
            }
        }

        public void ToJsonObject(Hashtable ht) {
            ht[Keys.TYPE] = NodeType;
            ht[Keys.POSITION] = Position;
            for (int i = 0; i < Fields.Count; ++i) {
                Fields[i].ToJsonObject(ht);
            }
            
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            NodeType = ht.GetString(Keys.TYPE);
            Position = ht.GetVector2(Keys.POSITION);
        }
        
        private class Keys {
            public const string POSITION = "Position";
            public const string TYPE = "Type";
        }
    }
}