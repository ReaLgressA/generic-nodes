using GenericNodes.Mech.Data;
using UnityEngine;

namespace GenericNodes.Visual.Interfaces {
    public interface IGenericFieldParent {
        NodeId NodeId { get; }
        Vector2 ParentPositionShift { get; }
        IGenericFieldParent Parent { get; }
    }
}