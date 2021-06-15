using System.Collections.Generic;
using GenericNodes.Mech.Data;
using GenericNodes.Visual.Nodes;
using TMPro;
using UnityEngine;

namespace GenericNodes.Visual.PopupMenus {
    public class PopupMenu : MonoBehaviour {
        [SerializeField] private WorkspaceArea workspaceArea;
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private RectTransform rtrPopupRoot;
        [SerializeField] private RectTransform rtrItemsRoot;
        [SerializeField] private PopupMenuItem prefabMenuItem;
        [SerializeField] private PopupMenuCategory prefabMenuCategory;
        [SerializeField] private NodeVisual prefabGenericNode;

        public List<PopupMenuItem> Items { get; private set; } = new List<PopupMenuItem>();
        
        private GraphScheme scheme;

        public void Reset() {
            for (int i = 0; i < Items.Count; ++i) {
                Destroy(Items[i].gameObject);
            }
            Items.Clear();
        }

        public void SetupScheme(GraphScheme scheme) {
            this.scheme = scheme;
            for (int i = 0; i < scheme.Nodes.Length; ++i) {
                AddPopupMenuItem(scheme.Nodes[i]);
            }
        }

        public void Show(string title, Vector2 position) {
            textTitle.text = title;
            rtrPopupRoot.anchoredPosition = position;
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        private void AddPopupMenuItem(NodeDescription node) {
            var item = InstantiateItem();
            item.Initilize(node.Type, CreateNodeOfType);
            Items.Add(item);
        }

        private void CreateNodeOfType(string nodeType) {
            Debug.Log($"Create node of type: {nodeType}");
            NodeVisual node = Instantiate(prefabGenericNode, workspaceArea.NodesRoot.parent);
            RectTransform rtr = node.GetComponent<RectTransform>();
            rtr.localScale = Vector3.one;
            rtr.anchoredPosition = (rtrPopupRoot.anchoredPosition - workspaceArea.NodesRoot.sizeDelta / 2)
                                   / workspaceArea.CanvasScale;
            rtr.SetParent(workspaceArea.NodesRoot);
            rtr.SetAsLastSibling();
            node.SetupData(nodeType, scheme.CreateNodeData(nodeType));
            workspaceArea.RegisterNode(node);
            Hide();
        }

        private PopupMenuItem InstantiateItem() {
            PopupMenuItem item = Instantiate(prefabMenuItem, rtrItemsRoot);
            RectTransform rtr = item.GetComponent<RectTransform>();
            rtr.localScale = Vector3.one;
            rtr.SetAsLastSibling();
            return item;
        }
    }
}