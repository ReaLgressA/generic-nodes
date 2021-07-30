using UnityEngine;
using UnityEngine.UI.Extensions;

namespace GenericNodes.Visual.Links {
    
    [RequireComponent(typeof(UILineRenderer))]
    public class NodeLink : MonoBehaviour {
        [SerializeField] private UILineRenderer lineRenderer = null;
        
        public UILineRenderer LineRenderer => lineRenderer ??= GetComponent <UILineRenderer>();
        public INodeLinkSocket SourceSocket { get; private set; } = null;
        public INodeLinkSocket TargetSocket { get; private set; } = null;

        public void SetupLink(INodeLinkSocket source, INodeLinkSocket target) {
            Reset();
            SourceSocket = source;
            TargetSocket = target;
            SourceSocket.PositionChanged += RefreshLink;
            TargetSocket.PositionChanged += RefreshLink;
            gameObject.SetActive(true);
            RefreshLink();
        }

        public void Reset() {
            if (SourceSocket != null) {
                SourceSocket.PositionChanged -= RefreshLink;
            }
            if (TargetSocket != null) {
                TargetSocket.PositionChanged -= RefreshLink;
            }
            SourceSocket = null;
            TargetSocket = null;
            gameObject.SetActive(false);
        }
        
        public void RefreshLink() {
            if (LineRenderer != null) {
                if (SourceSocket != null && TargetSocket != null) {
                    lineRenderer.Points = new[] {
                        SourceSocket.Position,
                        TargetSocket.Position
                    };
                } else {
                    lineRenderer.Points = null;
                }
            }
        }
    }
}