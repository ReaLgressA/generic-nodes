using Mech.Data;
using UnityEngine;

public class NodeEditorHUD : MonoBehaviour {
    [SerializeField] private NodeEditorInfoPanel infoPanel;

    public NodeEditorInfoPanel InfoPanel => infoPanel;


    public void OpenGraph(GraphData data) {
        InfoPanel.SetupData(data.Type, data.Info);
    }
}
