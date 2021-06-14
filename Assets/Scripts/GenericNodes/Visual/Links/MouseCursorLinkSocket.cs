using GenericNodes.Mech.Data;
using UnityEngine;

namespace GenericNodes.Visual.Links {
    public class MouseCursorLinkSocket : INodeLinkSocket {
        private static readonly Color MouseLinkColor = Color.white;
        private WorkspaceArea workspaceArea = null;
        
        public MouseCursorLinkSocket Instance { get; } = new MouseCursorLinkSocket();


        public NodeId Id => NodeId.None;
        public Vector2 Position => workspaceArea.GetWorldPosition(Input.mousePosition);
        public Color LinkColor => MouseLinkColor;

        public void Initialize(WorkspaceArea workspaceArea) {
            this.workspaceArea = workspaceArea;
        }

    }
}