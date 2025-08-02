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
                Debug.LogWarning("[Singleton] Instance already destroyed on application quit. Won't create again - returning null.");
                return null;
            }

            if (_instance == null) Debug.LogError("Singleton instance has not been created yet!");
            return _instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        this.LoadInstance();
    }

    protected virtual void LoadInstance()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (transform.parent == null) DontDestroyOnLoad(gameObject);
            return;
        }

        if (_instance != this) Debug.LogError("Another instance of SingletonExample already exists!");
    }

    private void OnApplicationQuit()
    {
        isShuttingDown = true;
    }

    private void OnDestroy()
    {
        isShuttingDown = true;
    }
}
