using GenericNodes.Mech.Data;
using GenericNodes.Visual;
using UnityEngine;

namespace GenericNodes.Mech
{
    public class NodeEditorController : MonoBehaviour {
        [SerializeField] private NodeEditorHUD hud; 
    
        public GraphData Data { get; private set; }
    
        public GraphScheme Scheme { get; private set; }
        
        void Start() {
            hud.OpenGraph(null, null);
        }

        public void OpenGraph(GraphData graphData, GenericNodesProjectInfo projectInfo) {
            if (graphData == null) {
                Data = null;
                Scheme = null;
                hud.OpenGraph(null, projectInfo);
                return;
            }
            Data = graphData;
            Scheme = graphData.Scheme;
            hud.OpenGraph(graphData, projectInfo);
        }
    }
}
