using System;

namespace GenericNodes.Visual.PopupMenus {
    public class PopupMenuCategory : PopupMenuItem {
        
        public override void Initilize(string actionName, Action<string> action) {
            base.Initilize(actionName, action);
        }
    }
}