using System.Collections;
using System.Collections.Generic;
using System.IO;
using GenericNodes.Mech.Extensions;
using GenericNodes.Mech.Fields;
using JsonParser;
using UnityEngine;

namespace GenericNodes.Mech.Data {
    public class GraphData : IJsonInterface {
        public string Type { get; private set; }
        public NodeData Info { get; private set; }
        
        public GraphScheme Scheme { get; private set; }

        public List<NodeData> Nodes { get; private set; } = new List<NodeData>();
        
        public string FilePath { get; }

        public GraphData(string type, NodeData info, GraphScheme scheme, string filePath) {
            Type = type;
            Info = info;
            Scheme = scheme;
            FilePath = filePath;
        }

        public void ToJsonObject(Hashtable ht) {
            ht[Keys.TYPE] = Type;
            ht[Scheme.NodeArrayName] = Nodes;
            Info.ToJsonObject(ht);
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Type = ht.GetString(Keys.TYPE);
            Info = new NodeData();
            Info.SetScheme(Scheme, Scheme.Fields.CloneFields());
            Info.FromJson(ht);
            
            if (ht.Contains(Scheme.NodeArrayName) && ht[Scheme.NodeArrayName] != null) {
                if (ht[Scheme.NodeArrayName] is ArrayList array) {
                    Nodes = new List<NodeData>(array.Count);
                    for (int i = 0; i < array.Count; i++) {
                        NodeData item = new NodeData();
                        if (array[i] != null) {
                            Hashtable htNode = (Hashtable)array[i];
                            DataField[] dataFields = Scheme.GetFieldsForNode(htNode["Type"].ToString());
                            item.SetScheme(Scheme, dataFields);
                            item.FromJson(htNode);
                        }
                        Nodes.Add(item);
                    }
                }
            }
        }

        public void SaveToFile() {
            Debug.Log($"Saving: {FilePath}");
            string json = MiniJSON.JsonEncode(this, true);
            File.WriteAllText(FilePath, json);
        }
        
        private static class Keys {
            public const string TYPE = "Type";
        }
    }
}