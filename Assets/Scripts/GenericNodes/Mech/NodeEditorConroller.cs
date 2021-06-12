using Mech.Data;
using UnityEngine;

public class NodeEditorConroller : MonoBehaviour {
    [SerializeField] private NodeEditorHUD hud; 
    
    public GraphData Data { get; private set; }
    
    public GraphScheme Scheme { get; private set; }
    
    void Awake() {

        Scheme = new GraphScheme();
        Scheme.FromJson(StringExtensions.ReadJson("scheme-game-event.json"));
        
        Data = Scheme.CreateGraph();

        // Data = new GraphData(new NodeData(new DataField[] {
        //     new StringDataField("Graph Name", "New Graph"),
        //     new StringDataField("String#1", "I am 1"),
        //     new StringDataField("Empty By Default"),
        //     new StringDataField("String#3", "I am 3"),
        //     new TextDataField("Multiline Text"),
        //     new IntDataField("Integer", 123),
        //     new FloatDataField("Float", 4.20f),
        //     new BoolDataField("Bool", true)
        // }));
    }

    void Start() {
        hud.OpenGraph(Data);
    }
}
