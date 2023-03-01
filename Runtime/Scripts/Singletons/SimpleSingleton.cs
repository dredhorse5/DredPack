using UnityEngine;

namespace DredPack
{
    public class SimpleSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_Instance;

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = (T) FindObjectOfType(typeof(T));
                    if (m_Instance == null)
                        Debug.LogError($"Simple singleton of class : {typeof(T)} does not exists");
                }

                return m_Instance;
            }
        }
    }
}