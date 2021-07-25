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
            SourceSocket = source;
            TargetSocket = target;
            SourceSocket.PositionChanged += RefreshLink;
            TargetSocket.PositionChanged += RefreshLink;
            RefreshLink();
        }

        public void Reset() {
            //SourceSocket.LinkSocketTo(NodeId.None);
            //TargetSocket.LinkSocketTo(NodeId.None);
            SourceSocket = null;
            TargetSocket = null;
            gameObject.SetActive(false);
        }

        // private void Update() {
        //     if (SourceSocket != null && TargetSocket != null) {
        //         RefreshLink();
        //     }
        // }

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

        // public void ConnectSockets() {
        //     //SourceSocket.LinkSocketTo(TargetSocket.Id);
        // }
    }
}