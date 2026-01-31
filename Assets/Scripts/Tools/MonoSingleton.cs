using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;

    /// <summary>
    /// Access singleton instance through this property.
    /// </summary>
    public static T MonoInstance
    {
        get
        {
            if (m_Instance is null || m_Instance.gameObject is null)
            {
                SetInstance();
            }
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance is not null && m_Instance.gameObject is not null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            OnAwake();
        }
    }

    public virtual void OnAwake() { }

    private static void SetInstance()
    {
        if (m_Instance is not null && m_Instance.gameObject is not null) return;

        // Search for all existing instances (active and inactive).
        var m_Instances = FindObjectsByType<T>(FindObjectsSortMode.None);
        if (m_Instances.Length > 0)
        {
            m_Instance = m_Instances[0];
            if (m_Instances.Length > 1)
            {
                Debug.LogWarning($"Found {m_Instances.Length} instances of {typeof(T)}. Keeping the first, destroying others.");
                for (int i = 1; i < m_Instances.Length; i++)
                {
                    Destroy(m_Instances[i].gameObject);
                }
            }
        }

        if (m_Instance is null)
        {
            // No objects found in the scene -> Create a new one.
            var singletonObject = new GameObject(typeof(T).Name + " (MonoSingleton)");
            m_Instance = singletonObject.AddComponent<T>();
        }

        if (m_Instance is not null)
        {
            // Make the instance persistent (optional).
            DontDestroyOnLoad(m_Instance);
        }
    }
}

