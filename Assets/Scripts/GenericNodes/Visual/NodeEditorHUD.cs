using GenericNodes.Mech.Data;
using GenericNodes.Visual.PopupMenus;
using UnityEngine;

namespace GenericNodes.Visual
{
    public class NodeEditorHUD : MonoBehaviour {
        [SerializeField] private WorkspaceArea workspaceArea;
        [SerializeField] private NodeEditorInfoPanel infoPanel;
        [SerializeField] private PopupMenu popupMenu;
    
        public void OpenGraph(GraphData data) {
            UnsubscribeFromEvents();
            if (data != null) {
                SubscribeForEvents();
                infoPanel.SetupData(data);
                popupMenu.SetupScheme(data.Scheme);
            } else {
                infoPanel.SetupData(null);
            }
        }

        private void SubscribeForEvents() {
            workspaceArea.OnAreaRmbClick += OpenNodeCreatePopupMenu;
            workspaceArea.OnInterruptRmbClick += popupMenu.Hide;
        }
        
        private void UnsubscribeFromEvents() {
            workspaceArea.OnAreaRmbClick -= OpenNodeCreatePopupMenu;
            workspaceArea.OnInterruptRmbClick -= popupMenu.Hide;
        }

        private void OpenNodeCreatePopupMenu(Vector2 position) {
            popupMenu.Show("Create Node", position);
        }
    }
}
