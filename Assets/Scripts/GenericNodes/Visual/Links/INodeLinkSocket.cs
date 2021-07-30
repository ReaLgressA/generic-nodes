using System;
using GenericNodes.Mech.Data;
using GenericNodes.Visual.Nodes;
using UnityEngine;

namespace GenericNodes.Visual.Links {
    public interface INodeLinkSocket {
        NodeId Id { get; }
        Vector2 Position { get; }
        NodeSocketMode Mode { get; }

        event Action PositionChanged;

        public void SetLinkedNodeId(NodeId id);
    }
}