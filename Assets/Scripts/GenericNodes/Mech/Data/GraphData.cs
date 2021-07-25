using System.Collections;
using System.IO;
using GenericNodes.Mech.Extensions;
using JsonParser;
using UnityEngine;

namespace GenericNodes.Mech.Data {
    public class GraphData : IJsonInterface {
        public string Type { get; private set; }
        public NodeData Info { get; private set; }
        
        public GraphScheme Scheme { get; private set; }
        
        public string FilePath { get; }

        public GraphData(string type, NodeData info, GraphScheme scheme, string filePath) {
            Type = type;
            Info = info;
            Scheme = scheme;
            FilePath = filePath;
        }

        public void ToJsonObject(Hashtable ht) {
            ht[Keys.TYPE] = Type;
            ht[Keys.INFO] = Info;
        }

        public void FromJson(Hashtable ht, bool isAddition = false) {
            Type = ht.GetString(Keys.TYPE);
            Info = new NodeData();
            Info.SetScheme(Scheme, Scheme.Fields.CloneFields());
            Info.FromJson(ht.GetHashtable(Keys.INFO));
        }

        private class Keys {
            public const string TYPE = "Type";
            public const string INFO = "Info";
        }

        public void SaveToFile() {
            Debug.Log($"Saving: {FilePath}");
            string json = MiniJSON.JsonEncode(this, true);
            File.WriteAllText(FilePath, json);
        }
    }
}