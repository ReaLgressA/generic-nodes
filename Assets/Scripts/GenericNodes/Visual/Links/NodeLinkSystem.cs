using System.Collections.Generic;
using GenericNodes.Mech;
using GenericNodes.Mech.Data;
using GenericNodes.Utility;
using GenericNodes.Visual.Nodes;
using UnityEngine;

namespace GenericNodes.Visual.Links {
    public class NodeLinkSystem : MonoBehaviour {
        [SerializeField] private NodeLink prefabNodeLink;
        //[SerializeField] private List<NodeLink> links = new List<NodeLink>();
        [SerializeField] private WorkspaceArea workspace;

        private RectTransform rectTransform = null;

        //public List<NodeLink> nodeLinks = new List<NodeLink>();

        public PrefabPool<NodeLink> poolNodeLinks;
        
        public UserHand Hand => workspace.Hand;

        public RectTransform Transform => rectTransform ??= GetComponent<RectTransform>();

        public Dictionary<INodeLinkSocket, NodeLink> mapOutputSocketLinks = new Dictionary<INodeLinkSocket, NodeLink>();

        private void Awake() {
            poolNodeLinks = new PrefabPool<NodeLink>(prefabNodeLink, transform, 0);
        }

        public bool ProcessLinkSocketClick(INodeLinkSocket socket) {
            if (Hand.IsFree) {
                StartLinkFromSocket(socket);
                return true;
            }
            if (Hand.NodeLink != null) {
                TryLinkToSocket(Hand.NodeLink, socket);
                return true;
            }
            Hand.Reset();
            return false;
        }

        private void StartLinkFromSocket(INodeLinkSocket socket) {
            if (socket.Mode == NodeSocketMode.Output) {
                UnlinkSocket(socket);
                socket.SetLinkedNodeId(NodeId.None);
                Hand.NodeLink = GetLink();
                Hand.NodeLink.gameObject.SetActive(true);
                Hand.NodeLink.SetupLink(socket, Hand.GetLinkSocket());
            }
        }
        
        private void TryLinkToSocket(NodeLink link, INodeLinkSocket socket) {
            if (socket.Mode == NodeSocketMode.Input) {
                NodeLink nodeLink = GetLink();
                nodeLink.SetupLink(link.SourceSocket, socket);
                nodeLink.SourceSocket.SetLinkedNodeId(nodeLink.TargetSocket.Id);
                mapOutputSocketLinks[nodeLink.SourceSocket] = nodeLink;
            }
            Hand.Reset();
        }

        public NodeLink GetLink() {
            NodeLink link = poolNodeLinks.Request();
            link.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            return link;
        }

        public void ReleaseLink(NodeLink link) {
            poolNodeLinks.Release(link);
        }

        public void UnlinkSocket(INodeLinkSocket socket) {
            if (mapOutputSocketLinks.TryGetValue(socket, out NodeLink link)) {
                link.Reset();
                ReleaseLink(link);
                mapOutputSocketLinks.Remove(socket);
            }
        }

        public void LinkSocketToNode(NodeSocketVisual socket, NodeId nodeId) {
            if (nodeId != NodeId.None) {
                if (socket.Mode == NodeSocketMode.Output) {
                    INodeLinkSocket targetLinkSocket = workspace.GetNode(nodeId)?.GetLinkSocket();
                    if (targetLinkSocket == null) {
                        Debug.LogWarning($"Can't connect socket to node with id {nodeId}: node not found");
                        return;
                    }
                    NodeLink nodeLink = GetLink();
                    nodeLink.SetupLink(socket, targetLinkSocket);
                    nodeLink.SourceSocket.SetLinkedNodeId(nodeLink.TargetSocket.Id);
                    mapOutputSocketLinks[nodeLink.SourceSocket] = nodeLink;
                } else {
                    Debug.LogError("Can't connect input socket as output");
                }
            }
        }
    }
}