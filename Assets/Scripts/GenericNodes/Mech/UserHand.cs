using GenericNodes.Visual;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Nodes;
using UnityEngine;

namespace GenericNodes.Mech {
    public class UserHand {
        public IHoldable Holdable { get; set; } = null;

        private NodeVisual nodeVisual;

        private Vector2 nodeShiftFromHand = Vector2.zero;
        private readonly WorkspaceArea workspaceArea;

        public UserHand(WorkspaceArea workspaceArea) {
            this.workspaceArea = workspaceArea;
        }

        public void SetHoldableItem(IHoldable holdable) {
            Holdable = holdable;
        }

        public void Reset() {
            if (nodeVisual != null) {
                nodeVisual.OnActionDrag -= ProcessNodeDrag;
            }
            Holdable = null;
            nodeVisual = null;
            SetupHoldableNode(Holdable as NodeVisual);
        }

        private void SetupHoldableNode(NodeVisual node) {
            if (node == null) {
                return;
            }
            nodeVisual = node;
            
            nodeShiftFromHand = nodeVisual.Transform.anchoredPosition - workspaceArea.GetWorldPosition(Input.mousePosition); 
            nodeVisual.OnActionDrag += ProcessNodeDrag;
        }

        private void ProcessNodeDrag(IHoldable holdable) {
            
            nodeVisual.SetPosition(workspaceArea.GetWorldPosition(Input.mousePosition) + nodeShiftFromHand);
        }
    }
}