using GenericNodes.Mech.Data;
using GenericNodes.Utility.JsonSerialization;
using GenericNodes.Visual;
using UnityEngine;

namespace GenericNodes.Mech
{
    public class NodeEditorConroller : MonoBehaviour {
        [SerializeField] private NodeEditorHUD hud; 
    
        public GraphData Data { get; private set; }
    
        public GraphScheme Scheme { get; private set; }
    
        void Awake() {

            Scheme = new GraphScheme();
            Scheme.FromJson(StringExtensions.ReadJson("scheme-game-event.json"));
        
            Data = Scheme.CreateGraph();
        }

        void Start() {
            hud.OpenGraph(Data);
        }
    }
}
