using System;
using UnityEngine;

namespace DredPack.Audio.Help
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class AudioTypeAttribute : PropertyAttribute { }
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class AudioGroupAttribute : PropertyAttribute { }
}