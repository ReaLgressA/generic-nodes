using System;
using System.Collections;
using System.Collections.Generic;
using GenericNodes.Mech.Extensions;
using GenericNodes.Mech.Fields;
using JsonParser;
using UnityEngine;

namespace GenericNodes.Mech.Data {
    public class GraphScheme : IJsonInterface {
        public string Type { get; private set; } = string.Empty;
        public List<DataField> Fields { get; private set; } = new List<DataField>();
        public EnumDescription[] Enums { get; private set; } = Array.Empty<EnumDescription>();
        public NodeDescription[] Nodes { get; private set; } = Array.Empty<NodeDescription>();
        public NodeDescription[] CustomDataTypes { get; private set; } = Array.Empty<NodeDescription>();
        
        public string NodeArrayName { get; private set; } = null;

        public GraphData CreateGraph(string filePath) {
            NodeData graphInfo = new NodeData(this, Type, NodeId.None, Fields.CloneFields());
            return new GraphData(Type, graphInfo, this, filePath);
        }

        public void ToJsonObject(Hashtable ht) {
            
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Type = ht.GetStringSafe(Keys.TYPE, Type);
            Enums = ht.GetArray(Keys.ENUMS, Enums);
            DataFieldFactory.CurrentGraphScheme = this;
            CustomDataTypes = ht.GetArray(Keys.CUSTOM_DATA_TYPES, CustomDataTypes);
            Fields = ht.ReadAsGenericList(Keys.FIELDS, DataFieldFactory.CreateFromHashtable);
            NodeArrayName = ht.GetStringSafe(Keys.NODE_ARRAY, NodeArrayName);
            Nodes = ht.GetArray(Keys.NODES, Nodes);
        }

        public NodeData CreateNodeData(string nodeType, NodeId nodeId) {
            for (int i = 0; i < Nodes.Length; ++i) {
                if (string.Equals(Nodes[i].Type, nodeType, StringComparison.Ordinal)) {
                    return new NodeData(this, Nodes[i].Type, nodeId, Nodes[i].Fields.CloneFields());
                }
            }
            return null;
        }

        public DataField[] GetFieldsForNode(string nodeType) {
            for (int i = 0; i < Nodes.Length; ++i) {
                if (string.Equals(Nodes[i].Type, nodeType, StringComparison.Ordinal)) {
                    return Nodes[i].Fields.CloneFields();
                }
            }
            return Array.Empty<DataField>();
        }

        public DataField[] GetCustomDataTypeFields(string objectType) {
            for (int i = 0; i < CustomDataTypes.Length; ++i) {
                if (CustomDataTypes[i].Type.Equals(objectType, StringComparison.Ordinal)) {
                    return CustomDataTypes[i].Fields.CloneFields();
                }               
            }
            Debug.LogError($"GetCustomDataTypeFields failed! No such custom data type '{objectType}'");
            return Array.Empty<DataField>();
        }
        
        private static class Keys {
            public const string TYPE = "Type";
            public const string FIELDS = "Fields";
            public const string NODE_ARRAY = "NodeArray";
            public const string NODES = "Nodes";
            public const string ENUMS = "Enums";
            public const string CUSTOM_DATA_TYPES = "CustomDataTypes";
        }
    }
}