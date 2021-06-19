using System.Collections.Generic;
using GenericNodes.Visual.Links;
using UnityEngine;

namespace GenericNodes.Visual.Nodes {
    public class NodeSocketArray : MonoBehaviour {
        [SerializeField] private List<NodeSocketVisual> sockets = new List<NodeSocketVisual>();

        private NodeVisual nodeVisual;
        
        public void Initialize(NodeVisual nodeVisual) {
            this.nodeVisual = nodeVisual;
        }
        
        public NodeSocketVisual GetFreeSocket() {
            for (int i = 0; i < sockets.Count; ++i) {
                if (!sockets[i].gameObject.activeSelf) {
                    sockets[i].Initialize(nodeVisual);
                    sockets[i].gameObject.SetActive(true);
                    return sockets[i];
                }
            }
            return CreateExtraSocket();
        }

        private NodeSocketVisual CreateExtraSocket() {
            NodeSocketVisual socket = Instantiate(sockets[0], transform);
            socket.Initialize(nodeVisual);
            socket.Transform.localScale = Vector3.one;
            socket.Transform.SetAsLastSibling();
            socket.gameObject.SetActive(true);
            return socket;
        }

        public void ReleaseSocket(INodeLinkSocket linkSocket) {
            for (int i = 0; i < sockets.Count; ++i) {
                if (ReferenceEquals(linkSocket, sockets[i])) {
                    sockets[i].gameObject.SetActive(false);
                }   
            }
        }
    }
}