using System;
using System.Collections.Generic;
using System.Linq;
using DredPack.Audio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DredPack.UI
{
    
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class Window : MonoBehaviour, WindowClasses.IWindow, WindowClasses.IWindowCallback
    {
        public WindowClasses.GeneralTab General = new WindowClasses.GeneralTab();
        public WindowClasses.EventsTab Events = new WindowClasses.EventsTab();
        public WindowClasses.AudioTab Audio = new WindowClasses.AudioTab();

        public WindowClasses.Components Components = new WindowClasses.Components();

        private HashSet<WindowClasses.IWindowCallback> callbacks;

        private void Awake()
        {
            General.Init(this);
            Events.Init(this);
            Audio.Init(this);
            RegisterCallback(new WindowClasses.IWindowCallback[]{General,Events,Audio});
        }

        public void Open() { }

        public void Close() { }

        public void Switch() { }

        public void Switch(bool state) { }


        #region Callbacks


        public void RegisterCallback(WindowClasses.IWindowCallback[] _callbacks)
        {
            foreach (var cl in _callbacks)
                if(cl != null)
                    RegisterCallback(cl);
        }
        public void RegisterCallback(WindowClasses.IWindowCallback callback) => callbacks.Add(callback);
        
        public void RemoveCallback(WindowClasses.IWindowCallback[] _callbacks)
        {
            foreach (var cl in _callbacks)
                if(cl != null)
                    RemoveCallback(cl);
        }
        public void RemoveCallback(WindowClasses.IWindowCallback callback) => callbacks.Remove(callback);
        
        

        public void OnStartOpen()
        {
            foreach (var cl in callbacks)
                cl?.OnStartOpen();
        }

        public void OnStartClose()
        {
            foreach (var cl in callbacks)
                cl?.OnStartClose();
        }

        public void OnStartSwitch(bool state)
        {
            foreach (var cl in callbacks)
                cl?.OnStartSwitch(state);
        }

        public void OnEndOpen()
        {
            foreach (var cl in callbacks)
                cl?.OnEndOpen();
        }

        public void OnEndClose()
        {
            foreach (var cl in callbacks)
                cl?.OnEndClose();
        }

        public void OnEndSwitch(bool state)
        {
            foreach (var cl in callbacks)
                cl?.OnEndSwitch(state);
        }

        public void OnStateChanged(WindowClasses.StatesRead state)
        {
            foreach (var cl in callbacks)
                cl?.OnStateChanged(state);
        }
        
        
        
        #endregion


        private void Reset()
        {
            Components.DisableableObject = gameObject;

            Components.Canvas = GetComponent<Canvas>();
            if (!Components.Canvas)
            {
                var canvases = GetComponentsInParent<Canvas>();
                Components.Canvas = canvases.Length > 0 ? canvases.First() : null;
            }

            Components.Raycaster = GetComponent<GraphicRaycaster>();
            General.EnableableRaycaster = Components.Raycaster;
            if (!Components.Canvas)
            {
                var canvases = GetComponentsInParent<GraphicRaycaster>();
                Components.Raycaster = canvases.Length > 0 ? canvases.First() : null;
            }

            Components.BackgroundImage = GetComponent<Image>();
            Components.CanvasGroup = GetComponent<CanvasGroup>();
        }
    }

}
