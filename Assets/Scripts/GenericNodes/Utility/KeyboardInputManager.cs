using UnityEngine.EventSystems;

namespace Utility {
    public static class KeyboardInputManager {

        public static bool IsAnyGameObjectSelected => EventSystem.current.currentSelectedGameObject != null;

    }
}