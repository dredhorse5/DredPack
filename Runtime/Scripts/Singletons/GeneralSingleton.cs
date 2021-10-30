using UnityEngine;
using System;

/// <summary>
/// Persistent humble singleton, basically a classic singleton but will destroy any other older components of the same type it finds on awake
/// </summary>
public class GeneralSingleton<T> : MonoBehaviour where T : Component
{
	protected static T _instance;
	public float InitializationTime;

	/// <summary>
	/// Singleton design pattern
	/// </summary>
	/// <value>The instance.</value>
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<T>();
				if (_instance == null)
				{
					GameObject obj = new GameObject();
					obj.hideFlags = HideFlags.HideAndDontSave;
					_instance = obj.AddComponent<T>();
				}
			}
			return _instance;
		}
	}

	/// <summary>
	/// On awake, we check if there's already a copy of the object in the scene. If there's one, we destroy it.
	/// </summary>
	/*protected virtual void Awake()
	{
		if (!Application.isPlaying)
		{
			return;
		}

		InitializationTime = Time.time;

		DontDestroyOnLoad(this.gameObject);
		// we check for existing objects of the same type
		T[] check = FindObjectsOfType<T>();
		foreach (T searched in check)
		{
			if (searched != this)
			{
				// if we find another object of the same type (not this), and if it's older than our current object, we destroy it.
				if (searched.GetComponent<Singleton<T>>().InitializationTime < InitializationTime)
				{
					Destroy(searched.gameObject);
				}
			}
		}

		if (_instance == null)
		{
			_instance = this as T;
		}
	}*/

	/// <summary>
	/// new awake
	/// On awake we check if there's Exists another object. if it is, we destroy this object. 
	/// </summary>
	protected virtual void Awake()
    {
		if (!Application.isPlaying)
		{
			return;
		}

		// we check for existing objects of the same type
		T[] check = FindObjectsOfType<T>();
		if(check.Length > 0)
        {
			//if we find another object of the same type (not this), we destroy this object.
			foreach (T searched in check)
            {
				if (searched != this)
				{
					Destroy(this.gameObject);
					return;
				}
			}
        }

		//if objects isn't exists in the scene, we initialization this object

		DontDestroyOnLoad(this.gameObject);
		

		if (_instance == null)
		{
			_instance = this as T;
		}

		InitializationTime = Time.time;
	}
}

