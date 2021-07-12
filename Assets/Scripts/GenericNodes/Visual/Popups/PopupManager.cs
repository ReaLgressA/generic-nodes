using System;
using System.Collections.Generic;
using UnityEngine;

namespace GenericNodes.Visual.Popups {
    public class PopupManager : MonoBehaviour {
        [SerializeField] private RectTransform rtrPopupsRoot;
        private static PopupManager Instance { get; set; }

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
            }
            Instance = this;
        }

        public static T GetPopup<T>() where T : MonoBehaviour {
            string path = mapTypeToPath[typeof(T)];
            if (popupCache.TryGetValue(path, out MonoBehaviour cachedPopup)) {
                return cachedPopup as T;
            }
            T popup = Instantiate(Resources.Load<T>(path), Instance.rtrPopupsRoot);
            popupCache.Add(path, popup);
            return popup;
        }

        private static Dictionary<string, MonoBehaviour> popupCache = new Dictionary<string, MonoBehaviour>();
        
        private static Dictionary<Type, string> mapTypeToPath = new Dictionary<Type, string> {
            { typeof(SelectFilePathPopup), "Prefabs/UI/Popups/SelectFilePathPopup" }
        };
    }
}