using UnityEngine;

public class LayerSingleton : MonoBehaviour
{
    public static GameObject Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = gameObject;
    }

    void OnDestroy()
    {
        if (Instance == gameObject)
            Instance = null;
    }
}
