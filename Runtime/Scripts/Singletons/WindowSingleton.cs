using DredPack.UI;
using UnityEngine;

namespace DredPack
{
    public class WindowSingleton<T> : WindowBehaviour where T : MonoBehaviour
    {
        private static T m_Instance;

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = (T) FindObjectOfType(typeof(T), true);
                return m_Instance;
            }
        }
    }
}