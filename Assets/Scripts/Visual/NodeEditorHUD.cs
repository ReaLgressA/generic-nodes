using Mech.Data;
using UnityEngine;
using Visual;
using Visual.PopupMenus;

public class NodeEditorHUD : MonoBehaviour {
    [SerializeField] private WorkspaceArea workspaceArea;
    [SerializeField] private NodeEditorInfoPanel infoPanel;
    [SerializeField] private PopupMenu popupMenu;
    

    public void OpenGraph(GraphData data) {
        workspaceArea.OnAreaRightClicked += OpenNodeCreatePopupMenu;
        infoPanel.SetupData(data.Type, data.Info);
        popupMenu.SetupScheme(data.Scheme);
    }

    private void OpenNodeCreatePopupMenu(Vector2 position) {
        popupMenu.Show(position);
    }
}
