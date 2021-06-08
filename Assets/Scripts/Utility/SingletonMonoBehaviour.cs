using UnityEngine;
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;
    
    public static T Instance {
        get {
            if (m_ShuttingDown) {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                 "' already destroyed. Returning null.");
                return null;
            }
            lock(m_Lock) {
                if (m_Instance == null || m_Instance.gameObject == null) {
                    m_Instance = (T) FindObjectOfType(typeof(T));
                    if (m_Instance == null) {
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T) + " (Singleton)";
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return m_Instance;
            }
        }
    }

    private void OnApplicationQuit() {
        m_ShuttingDown = true;
    }
}