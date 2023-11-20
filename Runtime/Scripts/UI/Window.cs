using System;
using System.Linq;
using DredPack.Audio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DredPack.UI
{
    
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class Window : MonoBehaviour
    {
        public WindowClasses.GeneralTab General = new WindowClasses.GeneralTab();
        public WindowClasses.EventsTab Events = new WindowClasses.EventsTab();
        public WindowClasses.AudioTab Audio = new WindowClasses.AudioTab();

        public WindowClasses.Components Components = new WindowClasses.Components();

        private void Reset()
        {
            Components.DisableableObject = gameObject;
            
            Components.Canvas = GetComponent<Canvas>();
            if(!Components.Canvas)
            {
                var canvases = GetComponentsInParent<Canvas>();
                Components.Canvas = canvases.Length > 0 ? canvases.First() : null;
            }
            
            Components.Raycaster = GetComponent<GraphicRaycaster>();
            General.EnableableRaycaster = Components.Raycaster;
            if(!Components.Canvas)
            {
                var canvases = GetComponentsInParent<GraphicRaycaster>();
                Components.Raycaster = canvases.Length > 0 ? canvases.First() : null;
            }
        }
    }

}
