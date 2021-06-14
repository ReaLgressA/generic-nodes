using System;
using System.Collections;

namespace GenericNodes.Utility.JsonSerialization
{
    public static partial class StringExtensions {
    
        public static Hashtable ReadJson(string pathJson) {
            try {
                string json = StreamingAssetReader.ReadAsString(pathJson);
                return json.HashtableFromJson();
            } catch (Exception ex) {
                UnityEngine.Debug.LogError("Failed to readJson: " + pathJson);
            }
            return null;
        }
    }
}
