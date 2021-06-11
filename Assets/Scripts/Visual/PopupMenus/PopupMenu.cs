using System.Collections.Generic;
using Mech.Data;
using UnityEngine;

namespace Visual.PopupMenus {
    public class PopupMenu : MonoBehaviour {
        [SerializeField] private RectTransform rtrPopupRoot;
        [SerializeField] private RectTransform rtrItemsRoot;
        [SerializeField] private PopupMenuItem prefabMenuItem;
        [SerializeField] private PopupMenuCategory prefabMenuCategory;
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

        public void Show(Vector2 position) {
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