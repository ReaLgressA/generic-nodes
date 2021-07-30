using GenericNodes.Mech.Data;

namespace GenericNodes.Visual.Interfaces {
    public interface INodeIdSocketContainer : IGenericFieldParent {
        void SetLinkedNodeId(NodeId nodeId);
    }
}