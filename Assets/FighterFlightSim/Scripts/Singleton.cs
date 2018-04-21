using UnityEngine;

[DisallowMultipleComponent]
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    private static volatile T instance;
    // thread safety
    private static object _lock = new object();
    
    public static bool FindInactive = true;
    
    // Whether or not this object should persist when loading new scenes. Should be set in Init().
    public static bool Persist;

    // Whether or not destory other singleton instances if any. Should be set in Init().
    public static bool DestroyOthers = true;
    
    // instead of heavy comparision (instance != null)
    // http://blogs.unity3d.com/2014/05/16/custom-operator-should-we-keep-it/
    private static bool instantiated;

    private static bool applicationIsQuitting;

    public static bool Lazy;

    public static T Instance {
        get {
            if (applicationIsQuitting) {
                Debug.LogWarningFormat("[Singleton] Instance '{0}' already destroyed on application quit. Won't create again - returning null.", typeof(T));
                return null;
            }
            lock (_lock) {
                if (!instantiated) {
                    Object[] objects;
                    if (FindInactive) { objects = Resources.FindObjectsOfTypeAll(typeof(T)); }
                    else { objects = FindObjectsOfType(typeof(T)); }
                    if (objects == null || objects.Length < 1) {
                        GameObject singleton = new GameObject();
                        singleton.name = string.Format("{0} [Singleton]", typeof(T));
                        Instance = singleton.AddComponent<T>();
                        Debug.LogWarningFormat("[Singleton] An Instance of '{0}' is needed in the scene, so '{1}' was created{2}", typeof(T), singleton.name, Persist ? " with DontDestoryOnLoad." : ".");
                    }
                    else if (objects.Length >= 1) {
                        Instance = objects[0] as T;
                        if (objects.Length > 1) {
                            Debug.LogWarningFormat("[Singleton] {0} instances of '{1}'!", objects.Length, typeof(T));
                            if (DestroyOthers) {
                                for (int i = 1; i < objects.Length; i++) {
                                    Debug.LogWarningFormat("[Singleton] Deleting extra '{0}' instance attached to '{1}'", typeof(T), objects[i].name);
                                    Destroy(objects[i]);
                                }
                            }
                        }
                        return instance;
                    }
                }
                return instance;
            }
        }
        protected set {
            instance = value;
            instantiated = true;
            instance.AwakeSingleton();
            if (Persist) { DontDestroyOnLoad(instance.gameObject); }
        }
    }

    // if Lazy = false and gameObject is active this will set instance
    // unless instance was called by another Awake method
    private void Awake() {
        if (Lazy) { return; }
        lock (_lock) {
            if (!instantiated) {
                Instance = this as T;
            }
            else if (DestroyOthers && Instance.GetInstanceID() != GetInstanceID()) {
                Debug.LogWarningFormat("[Singleton] Deleting extra '{0}' instance attached to '{1}'", typeof(T), name);
                Destroy(this);
            }
        }
    }
    
    // this might be called for inactive singletons before Awake if FindInactive = true
    protected virtual void AwakeSingleton() {}

    protected virtual void OnDestroy() {
        applicationIsQuitting = true;
        instantiated = false;
    }
}