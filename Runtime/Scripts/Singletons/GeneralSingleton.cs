using UnityEngine;
using System;

namespace DredPack
{
    public class GeneralSingleton<T> : MonoBehaviour where T : Component
    {
        public static T c_Instance { get; private set; }

        public static T Instance
        {
            get
            {
                if (!c_Instance)
                {
                    c_Instance = FindObjectOfType<T>(true);
                    if (!c_Instance)
                    {
                        GameObject obj = new GameObject();
                        c_Instance = obj.AddComponent<T>();
                        obj.name = c_Instance.GetType().Name;
                    }
                }

                return c_Instance;
            }
        }

        protected virtual void Awake()
        {
            if (!Application.isPlaying)
                return;

            T[] check = FindObjectsOfType<T>(true);
            if (check.Length > 0)
            {
                foreach (T searched in check)
                {
                    if (searched != this)
                    {
                        Destroy(this.gameObject);
                        return;
                    }
                }
            }

            DontDestroyOnLoad(this.gameObject);

            if (!c_Instance)
                c_Instance = this as T;
        }
    }


}