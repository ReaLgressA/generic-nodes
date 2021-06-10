using System.Collections.Generic;
using UnityEngine;

namespace Visual.PopupMenus {
    public class PopupMenu : MonoBehaviour {
        public List<PopupMenuItem> Items { get; private set; } = new List<PopupMenuItem>();


        public void Reset() {
            for (int i = 0; i < Items.Count; ++i) {
                Destroy(Items[i].gameObject);
            }
            Items.Clear();
        }
    }
}