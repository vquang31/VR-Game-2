using UnityEngine;

public abstract class Singleton<T> : NewMonoBehaviour where T : NewMonoBehaviour
{
    public static T _instance;

    public static T Instance
    {
        get 
        {
            if (_instance == null) Debug.Log("Chua cos Instance");
            return _instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        LoadInstance();
    }

    private void LoadInstance()
    {
        if (_instance == null) { 
            _instance = this as T;
            if(transform.parent == null)
                DontDestroyOnLoad(transform);
            return;
        }
        if (_instance != this)
        {
            Debug.Log("Da co Instance");
        }
    }

}
