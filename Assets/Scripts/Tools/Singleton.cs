/// <summary>
/// Inherit from this base class to create a singleton
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Singleton<T> where T : class, new()
{
    private static T _instance;
    private static readonly object syslock = new object();

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (syslock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Destroys the singleton instance.
    /// </summary>
    public static void Destroy()
    {
        lock (syslock)
        {
            _instance = null;
        }
    }

}

