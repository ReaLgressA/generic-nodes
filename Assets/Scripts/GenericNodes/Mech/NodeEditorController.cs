using GenericNodes.Mech.Data;
using GenericNodes.Visual;
using UnityEngine;

namespace GenericNodes.Mech
{
    public class NodeEditorController : MonoBehaviour {
        [SerializeField] private NodeEditorHUD hud; 
    
        public GraphData Data { get; private set; }
    
        public GraphScheme Scheme { get; private set; }
    
        void Awake() {
            //Scheme = new GraphScheme();
            //Scheme.FromJson(StringExtensions.ReadJson("scheme-game-event.json"));
        
            //Data = Scheme.CreateGraph();
        }

        void Start() {
            hud.OpenGraph(null);
        }

        public void OpenGraph(GraphData graphData) {
            if (graphData == null) {
                Data = null;
                Scheme = null;
                hud.OpenGraph(null);
                return;
            }
            Data = graphData;
            Scheme = graphData.Scheme;
            hud.OpenGraph(graphData);
        }
    }
}
