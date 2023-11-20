using System;
using DredPack.Audio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DredPack.UI
{
    
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class Window : MonoBehaviour
    {
        public WindowClasses.EventsTab Events;
        public WindowClasses.AudioTab Audio;


        
        public static int currentWindowTab;
    }

}
