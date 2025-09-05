using UnityEngine;

public class PirateSingleton<T> : PirateMonoBehaviour where T : PirateMonoBehaviour
{
    public static bool isShuttingDown = false;
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (isShuttingDown)
            {
                //Debug.LogWarning("[Singleton] Instance already destroyed on application quit. Returning null.");
                return null;
            }

            if (_instance == null)
            {
                Debug.LogError($"[Singleton] Instance of {typeof(T)} has not been created yet!");
            }
            return _instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        LoadInstance();
    }

    protected virtual void LoadInstance()
    {
        if (_instance == null)
        {
            _instance = this as T;
            
            if (transform.parent == null)
                DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogError($"[Singleton] Another instance of {typeof(T)} already exists! Destroying this one.");
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        isShuttingDown = true;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
