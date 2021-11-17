using GenericNodes.Mech.Data;
using UnityEngine;

namespace GenericNodes.Visual.Interfaces {
    public interface IGenericFieldParent {
        NodeId NodeId { get; }
        Vector2 ParentPositionShift { get; }
        IGenericFieldParent Parent { get; }
    }
    
    public static class IGenericFieldParentExtensions {
        public static int CountParentLevel(this IGenericFieldParent parent) {
            return parent.Parent?.CountParentLevel() ?? 0 + 1;
        } 
    }
}