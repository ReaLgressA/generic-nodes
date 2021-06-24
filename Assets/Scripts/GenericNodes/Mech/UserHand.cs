using System;
using GenericNodes.Mech.Data;
using GenericNodes.Visual;
using GenericNodes.Visual.Interfaces;
using GenericNodes.Visual.Links;
using GenericNodes.Visual.Nodes;
using UnityEngine;

namespace GenericNodes.Mech {
    public class UserHand : INodeLinkSocketProvider,
                            INodeLinkSocket {
        public IHoldable Holdable { get; set; } = null;
        public NodeLink NodeLink { get; set; } = null;
        
        private NodeVisual nodeVisual;

        private Vector2 nodeShiftFromHand = Vector2.zero;
        private readonly WorkspaceArea workspaceArea;

        public bool IsFree => Holdable == null && NodeLink == null;
        public NodeId Id => NodeId.None;
        public Vector2 Position => workspaceArea.GetFixedWorldPosition(Input.mousePosition);
        public Color LinkColor => Color.white;
        public NodeSocketMode Mode => NodeSocketMode.Input;
        
        public event Action<INodeLinkSocket, NodeId> SocketLinked;
        public void LinkSocketTo(NodeId nodeId) {
            //Hand only receives the link, so no need to link anything
        }

        public UserHand(WorkspaceArea workspaceArea) {
            this.workspaceArea = workspaceArea;

            workspaceArea.OnInterruptLmbClick += ClearNodeLinkIfNeeded;
        }

        ~UserHand() {
            workspaceArea.OnInterruptLmbClick -= ClearNodeLinkIfNeeded;
        }

        public INodeLinkSocket GetLinkSocket() {
            return this;
        }

        public void SetHoldableItem(IHoldable holdable) {
            Holdable = holdable;
            SetupHoldableNode(Holdable as NodeVisual);
        }

        public void Reset() {
            Holdable = null;
            ClearNodeLinkIfNeeded();
            nodeVisual = null;
        }

        public void Update(float dTime) {
            if (Holdable != null) {
                ProcessNodeDrag(Holdable);
            }
        }

        private void SetupHoldableNode(NodeVisual node) {
            if (node == null) {
                return;
            }
            nodeVisual = node;
            nodeShiftFromHand = nodeVisual.Transform.anchoredPosition - workspaceArea.GetWorldPosition(Input.mousePosition);
        }

        private void ProcessNodeDrag(IHoldable holdable) {
            nodeVisual.SetPosition(workspaceArea.GetWorldPosition(Input.mousePosition) + nodeShiftFromHand);
        }
        
        private void ClearNodeLinkIfNeeded() {
            if (NodeLink != null) {
                NodeLink.Reset();
                NodeLink = null;
            }    
        }
    }
}