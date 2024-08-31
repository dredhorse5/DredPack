using UnityEngine;

namespace DredPack
{
    public class SimpleSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T c_Instance;

        public static T Instance
        {
            get
            {
                if (c_Instance == null)
                    c_Instance = (T) FindObjectOfType(typeof(T), true);
                return c_Instance;
            }
        }
    }
}
