using UnityEngine;
using System;

namespace DredPack
{
    public class GeneralSingleton<T> : MonoBehaviour where T : Component
    {
        public static T _instance { get; private set; }

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<T>(true);
                    if (!_instance)
                    {
                        GameObject obj = new GameObject();
                        //obj.hideFlags = HideFlags.HideAndDontSave;
                        _instance = obj.AddComponent<T>();
                        obj.name = _instance.GetType().Name;
                    }
                }

                return _instance;
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

            if (!_instance)
                _instance = this as T;
        }
    }


}