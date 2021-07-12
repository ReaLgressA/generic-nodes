using System.Collections.Generic;
using GenericNodes.Mech;
using GenericNodes.Mech.Data;
using GenericNodes.Visual.Nodes;
using UnityEngine;

namespace GenericNodes.Visual.Links {
    public class NodeLinkSystem : MonoBehaviour {
        [SerializeField] private List<NodeLink> links = new List<NodeLink>();
        [SerializeField] private WorkspaceArea workspace;

        private RectTransform rectTransform = null;

        public List<NodeLink> nodeLinks = new List<NodeLink>();

        public UserHand Hand => workspace.Hand;

        public RectTransform Transform => rectTransform ??= GetComponent<RectTransform>();
        
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
                Hand.NodeLink = GetLink();
                Hand.NodeLink.gameObject.SetActive(true);
                Hand.NodeLink.SetupLink(socket, Hand.GetLinkSocket());
            }
        }
        
        private void TryLinkToSocket(NodeLink link, INodeLinkSocket socket) {
            if (socket.Mode == NodeSocketMode.Input) {
                link.SetupLink(link.SourceSocket, socket);
                nodeLinks.Add(link);
                link.ConnectSockets();
                Hand.NodeLink = null;
            } else {
                Hand.Reset();
            }
        }

        private NodeLink GetLink() {
            for (int i = 0; i < links.Count; ++i) {
                if (!links[i].gameObject.activeSelf) {
                    return links[i];
                }
            }
            return CreateExtraLink();
        }

        private NodeLink CreateExtraLink() {
            NodeLink link = Instantiate(links[0], links[0].transform.parent);
            link.transform.localScale = Vector3.one;
            links.Add(link);
            return link;
        }

        public void UnlinkSocket(NodeSocketVisual socket) {
            if (socket.Mode == NodeSocketMode.Output) {
                for (int i = 0; i < nodeLinks.Count; ++i) {
                    if (ReferenceEquals(socket, nodeLinks[i].SourceSocket)) {
                        nodeLinks[i].Reset();
                        nodeLinks.RemoveAt(i);
                        return;
                    }
                }
            } else {
                Debug.LogError("Can't UnlinkOutputSocket, the socket is not in Output mode!");
            }
        }

        public void LinkSocketToNode(NodeSocketVisual socket, NodeId nodeId) {
            if (nodeId != NodeId.None) {
                NodeLink link = GetLink();
                if (socket.Mode == NodeSocketMode.Output) {
                    link.SetupLink(socket, workspace.GetNode(nodeId).GetLinkSocket());
                    nodeLinks.Add(link);
                    link.ConnectSockets();
                } else {
                    Debug.LogError("Can't connect input socket as output");
                }
            }
        }
    }
}