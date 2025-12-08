using UnityEngine;

/// <summary>
/// Generic Singleton base class for MonoBehaviours.
/// Inherit from this class to create a consistent singleton pattern.
/// Example: public class MyManager : Singleton<MyManager>
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static object _lock = new object();
    private static bool _applicationIsQuitting = false;

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        _instance = null;
        _applicationIsQuitting = false;
    }
#endif

    public static T instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindFirstObjectByType(typeof(T));

                    if (FindObjectsByType(typeof(T), FindObjectsSortMode.None).Length > 1)
                    {
                        Debug.LogError($"[Singleton] Something went really wrong - there should never be more than 1 singleton of type {typeof(T)}! Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = $"(singleton) {typeof(T)}";

                        DontDestroyOnLoad(singleton);

                        Debug.Log($"[Singleton] An instance of {typeof(T)} is needed in the scene, so '{singleton}' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log($"[Singleton] Using instance already created: {_instance.gameObject.name}");
                    }
                }

                return _instance;
            }
        }
    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it has been destroyed,
    /// it will create a buggy ghost object that will stay on the Editor scene
    /// even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    protected virtual void OnDestroy()
    {
        // Only set quitting flag if the actual singleton instance is being destroyed
        // Duplicates being destroyed on scene load should not trigger this
        if (_instance == this)
        {
            _applicationIsQuitting = true;
        }
    }

    protected virtual void Awake()
    {
        // If this is the first instance, make it persistent
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        // If another instance already exists, destroy this one
        else if (_instance != this)
        {
            Debug.LogWarning($"[Singleton] Another instance of {typeof(T)} already exists. Destroying duplicate on {gameObject.name}");
            Destroy(gameObject);
        }
    }
}

/// <summary>
/// Generic Singleton base class for MonoBehaviours that should NOT persist between scenes.
/// Use this for singletons that should be destroyed when changing scenes.
/// Example: public class MySceneManager : SingletonNoPersist<MySceneManager>
/// </summary>
public class SingletonNoPersist<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T instance
    {
        get
        {
            // Check if existing instance was destroyed (scene reload)
            if (_instance == null)
            {
                _instance = (T)FindFirstObjectByType(typeof(T));

                if (_instance == null)
                {
                    Debug.LogWarning($"[Singleton] No instance of {typeof(T)} found in the scene!");
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // Simply take over as the new instance - don't destroy anything
        // Unity will clean up old scene objects naturally
        // Just clear the old reference and set ourselves as the new instance
        _instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        // Only clear if this is the current instance
        if (_instance == this)
        {
            _instance = null;
        }
    }

    /// <summary>
    /// Call this to manually clear the singleton instance (useful before scene reload)
    /// </summary>
    public static void ClearInstance()
    {
        _instance = null;
    }
}