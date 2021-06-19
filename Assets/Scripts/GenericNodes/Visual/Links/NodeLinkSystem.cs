using System.Collections.Generic;
using GenericNodes.Mech;
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

        public NodeLink GetLink() {
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
    }
}