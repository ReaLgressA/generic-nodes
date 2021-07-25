using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JsonParser;
using UnityEngine;

namespace GenericNodes.Mech.Data {
    public class GraphSchemeProvider {
        public List<GraphScheme> Schemes { get; private set; } = new List<GraphScheme>();

        public void Setup(GenericNodesProjectInfo projectInfo) {
            string schemesDirectoryPath = Path.Combine(projectInfo.AbsoluteRootPath, "Schemes");
            if (!Directory.Exists(schemesDirectoryPath)) {
                Directory.CreateDirectory(schemesDirectoryPath);
            }
            DirectoryInfo schemesDirectory = new DirectoryInfo(schemesDirectoryPath);
            FileInfo[] files = schemesDirectory.GetFiles();
            
            for (int i = 0; i < files.Length; ++i) {
                string json = File.ReadAllText(files[i].FullName);
                Hashtable ht = MiniJSON.JsonDecode(json) as Hashtable;
                if (ht != null) {
                    GraphScheme scheme = new GraphScheme();
                    try {
                        scheme.FromJson(ht);
                        Schemes.Add(scheme);
                    } catch (Exception ex) {
                        Debug.LogError($"Failed to parse graph scheme by path: {files[i].FullName}. Exception: {ex.Message}\n{ex.StackTrace}");
                    }
                }
                
            }
        }
    }
}