using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DredPack.Audio;
using DredPack.UI.WindowAnimations;
using UnityEditor;
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
        [SerializeReference] public WindowClasses.AnimationTab Animation = new WindowClasses.AnimationTab();

        public WindowClasses.Components Components = new WindowClasses.Components();

        private HashSet<WindowClasses.IWindowCallback> callbacks = new HashSet<WindowClasses.IWindowCallback>();

        

        private Coroutine switchCor;

        private void Awake()
        {
            General.Init(this);
            Events.Init(this);
            Audio.Init(this);
            RegisterCallback(new WindowClasses.IWindowCallback[]{General,Events,Audio});
        }

        
        public void Open() => OpenCor();
        public Coroutine OpenCor()
        {
            if(switchCor != null)
                StopCoroutine(switchCor);
            switchCor = StartCoroutine(OpenIE());
            return switchCor;
        }

        
        public void Close() => CloseCor();
        public Coroutine CloseCor()
        {
            if (switchCor != null)
                StopCoroutine(switchCor);
            switchCor = StartCoroutine(CloseIE());
            return switchCor;
        }

        
        public void Switch() => SwitchCor();
        public Coroutine SwitchCor()
        {
            if(General.CurrentState == WindowClasses.StatesRead.Closed || General.CurrentState == WindowClasses.StatesRead.Closing)
                return OpenCor();
            return CloseCor();
        }

        
        public void Switch(bool state) => SwitchCor(state);
        public Coroutine SwitchCor(bool state)
        {
            if(state) return OpenCor();
            return CloseCor();
        }

        

        private IEnumerator OpenIE()
        {
            Animation.currentAnimation.Init(this);
            Animation.currentAnimation?.StopAllCoroutines();
            ChangeState(WindowClasses.StatesRead.Opening);
            OnStartOpen();
            OnStartSwitch(true);
            
            yield return Animation.currentAnimation.UpdateOpen();
            
            ChangeState(WindowClasses.StatesRead.Opened);
            OnEndOpen();
            OnEndSwitch(true);
        }
        private IEnumerator CloseIE()
        {
            Animation.currentAnimation.Init(this);
            Animation.currentAnimation?.StopAllCoroutines();
            ChangeState(WindowClasses.StatesRead.Closing);
            OnStartClose();
            OnStartSwitch(false);
            
            yield return Animation.currentAnimation.UpdateClose();
            
            OnEndClose();
            ChangeState(WindowClasses.StatesRead.Closed);
            OnEndSwitch(false);
        }

        private void ChangeState(WindowClasses.StatesRead state)
        {
            General.CurrentState = state;
            OnStateChanged(General.CurrentState);
        }


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


        
        
        #if UNITY_EDITOR
        private void Reset() => InitComponents();
        private void OnValidate() => InitComponents();


        private void InitComponents()
        {
            if(Components.DisableableObject == null)
                Components.DisableableObject = gameObject;

            if(Components.Canvas == null)
            {
                Components.Canvas = GetComponent<Canvas>();
                if (!Components.Canvas)
                {
                    var canvases = GetComponentsInParent<Canvas>();
                    Components.Canvas = canvases.Length > 0 ? canvases.First() : null;
                }
            }

            if(Components.Raycaster == null)
            {
                Components.Raycaster = GetComponent<GraphicRaycaster>();
                General.EnableableRaycaster = Components.Raycaster;
                if (!Components.Canvas)
                {
                    var canvases = GetComponentsInParent<GraphicRaycaster>();
                    Components.Raycaster = canvases.Length > 0 ? canvases.First() : null;
                }
            }

            if(Components.BackgroundImage == null)
                Components.BackgroundImage = GetComponent<Image>();
            if(Components.CanvasGroup == null)
                Components.CanvasGroup = GetComponent<CanvasGroup>();
            EditorUtility.SetDirty(this);
        }
        #endif
    }

}
