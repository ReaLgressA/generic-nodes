using GenericNodes.Mech.Data;
using GenericNodes.Visual.Nodes;
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
            workspaceArea.OnNodeRightClick += OpenNodePopupMenu;
            workspaceArea.OnAreaRmbClick += OpenNodeCreatePopupMenu;
            workspaceArea.OnInterruptRmbClick += popupMenu.Hide;
        }
        
        private void UnsubscribeFromEvents() {
            workspaceArea.OnAreaRmbClick -= OpenNodeCreatePopupMenu;
            workspaceArea.OnInterruptRmbClick -= popupMenu.Hide;
        }

        private void OpenNodePopupMenu(Vector2 position, NodeVisual nodeVisual) {
            if (infoPanel.Data != null) {
                popupMenu.SetupNode(nodeVisual);
                popupMenu.Show("Node Options", position);
            }
        }
        
        private void OpenNodeCreatePopupMenu(Vector2 position) {
            if (infoPanel.Data != null) {
                popupMenu.SetupScheme(infoPanel.Data.Scheme);
                popupMenu.Show("Create Node", position);
            }
        }
    }
}
