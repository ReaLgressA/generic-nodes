using Mech.Data;
using Mech.Fields;
using UnityEngine;

public class NodeEditorConroller : MonoBehaviour {
    [SerializeField] private NodeEditorHUD hud; 
    
    public GraphData Data { get; private set; }
    
    void Awake() {
        Data = new GraphData(new NodeData(new DataField[] {
            new StringDataField("Graph Name", "New Graph"),
            new StringDataField("String#1", "I am 1"),
            new StringDataField("Empty By Default"),
            new StringDataField("String#3", "I am 3")
        }));
    }

    void Start() {
        hud.OpenGraph(Data);
    }
}
