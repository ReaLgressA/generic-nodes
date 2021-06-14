using UnityEngine.EventSystems;

namespace GenericNodes.Utility {
    public static class KeyboardInputManager {

        public static bool IsAnyGameObjectSelected => EventSystem.current.currentSelectedGameObject != null;

    }
}