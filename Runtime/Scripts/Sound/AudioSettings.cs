using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class AudioSettings : ScriptableObject
{
    #region singleton
    public string Path;
    private static AudioSettings instance;
    public static AudioSettings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load("AudioSettings") as AudioSettings;
            }
            return instance;
        }
    }
    #endregion
}
