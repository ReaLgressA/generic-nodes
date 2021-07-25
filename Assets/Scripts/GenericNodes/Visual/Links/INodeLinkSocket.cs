using System;
using GenericNodes.Mech.Data;
using GenericNodes.Visual.Nodes;
using UnityEngine;

namespace GenericNodes.Visual.Links {
    public interface INodeLinkSocket {
        NodeId Id { get; }
        Vector2 Position { get; }
        Color LinkColor { get; }
        NodeSocketMode Mode { get; }

        //event Action<INodeLinkSocket, NodeId> SocketLinked;
        event Action PositionChanged;

        //void LinkSocketTo(NodeId nodeId);
    }
}