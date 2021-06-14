using GenericNodes.Mech.Data;
using GenericNodes.Visual.GenericFields;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace GenericNodes.Visual.Links {
    
    [RequireComponent(typeof(UILineRenderer))]
    public class NodeLink : MonoBehaviour {
        [SerializeField] private UILineRenderer lineRenderer = null;
        
        public UILineRenderer LineRenderer => lineRenderer ??= GetComponent <UILineRenderer>();
        public NodeId SourceNodeId { get; private set; } = NodeId.None;
        public NodeIdGenericField SourceField { get; private set; } = null;
        public INodeLinkSocket TargetSocket { get; private set; } = null;

        
    }
}