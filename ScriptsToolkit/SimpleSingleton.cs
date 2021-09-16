using UnityEngine;
// ReSharper disable StaticMemberInGenericType
// ReSharper disable InconsistentNaming

public class SimpleSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance
    {
        get
        {

            if (m_Instance == null)
            {
                // Search for existing instance.
                m_Instance = (T)FindObjectOfType(typeof(T));

                // Create new instance if one doesn't already exist.
                if (m_Instance == null)
                {
                    // Need to create a new GameObject to attach the singleton to.
                    var singletonObject = new GameObject();
                    m_Instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T) + " (SimpleSingleton)";
                }
            }

            return m_Instance;
        }
    }


}
