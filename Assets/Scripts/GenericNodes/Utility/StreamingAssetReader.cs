using System.IO;
using UnityEngine;

namespace GenericNodes.Utility
{
    public static class StreamingAssetReader {
        public static string ReadAsString(string filepath) {
            string path = Path.Combine(Application.streamingAssetsPath, filepath);
            if (File.Exists(path)) {
            
                return File.ReadAllText(path);
            }        
            Debug.LogError(string.Format("StreamingAssetReader:ReadAsString file not found: {0}", path));
            return string.Empty;
        }
    }
}
