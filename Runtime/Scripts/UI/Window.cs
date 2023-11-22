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
        private WindowClasses.IWindow _windowImplementation;
        [NonSerialized]
        private bool switchedOnce = false;

        private void Awake()
        {
            General.Init(this);
            Events.Init(this);
            Audio.Init(this);
            
            if(General.StateOnAwakeMethod == WindowClasses.StatesAwakeMethod.Awake && !switchedOnce)
            {
                Switch(General.StateOnAwake != WindowClasses.StatesAwake.Open, "Instantly");
                Switch(General.StateOnAwake == WindowClasses.StatesAwake.Open, General.AnimationOnAwake);
            }
            RegisterCallback(new WindowClasses.IWindowCallback[]{General,Events,Audio});
        }

        private void Start()
        {
            if(General.StateOnAwakeMethod == WindowClasses.StatesAwakeMethod.Start && !switchedOnce)
            {
                Switch(General.StateOnAwake != WindowClasses.StatesAwake.Open, "Instantly");
                Switch(General.StateOnAwake == WindowClasses.StatesAwake.Open, General.AnimationOnAwake);
            }
        }

        private void OnEnable()
        {
            if(General.StateOnAwakeMethod == WindowClasses.StatesAwakeMethod.OnEnable && !switchedOnce)
            {
                Switch(General.StateOnAwake != WindowClasses.StatesAwake.Open, "Instantly");
                Switch(General.StateOnAwake == WindowClasses.StatesAwake.Open, General.AnimationOnAwake);
            }
        }

        
        
        private void ChangeState(WindowClasses.StatesRead state)
        {
            General.CurrentState = state;
            OnStateChanged(General.CurrentState);
        }
        
        
        #region External Contol


        public void Open() => OpenCor("", new AnimationParameters());
        public void Open(string animName) => OpenCor(animName,new AnimationParameters());
        public void Open(AnimationParameters parameters) => OpenCor("", parameters);
        public Coroutine OpenCor(string animName, AnimationParameters parameters)
        {
            if (General.CurrentState == WindowClasses.StatesRead.Opened ||
                General.CurrentState == WindowClasses.StatesRead.Opening)
                return null;
            if(switchCor != null)
                StopCoroutine(switchCor);
            switchCor = StartCoroutine(OpenIE(animName, parameters));
            return switchCor;
        }

        
        public void Close() => CloseCor("", new AnimationParameters());
        public void Close(string animName) => CloseCor(animName,new AnimationParameters());
        public void Close(AnimationParameters parameters) => CloseCor("", parameters);
        public Coroutine CloseCor(string animName, AnimationParameters parameters)
        {
            if (General.CurrentState == WindowClasses.StatesRead.Closed ||
                General.CurrentState == WindowClasses.StatesRead.Closing)
                return null;
            if (switchCor != null)
                StopCoroutine(switchCor);
            switchCor = StartCoroutine(CloseIE(animName, parameters));
            return switchCor;
        }

        
        public void Switch() => SwitchCor("", new AnimationParameters());
        public void Switch(string animName) => SwitchCor(animName,new AnimationParameters());
        public void Switch(AnimationParameters parameters) => SwitchCor("", parameters);
        public Coroutine SwitchCor(string animName, AnimationParameters parameters)
        {
            if (General.CurrentState == WindowClasses.StatesRead.Closed ||
                General.CurrentState == WindowClasses.StatesRead.Closing)
                return OpenCor(animName, parameters);
            return CloseCor(animName, parameters);
        }


        public void Switch(bool state) => SwitchCor(state, "", new AnimationParameters());
        public void Switch(bool state,string animName) => SwitchCor(state, animName, new AnimationParameters());
        public void Switch(bool state, AnimationParameters parameters) => SwitchCor(state, "", parameters);
        public Coroutine SwitchCor(bool state,string animName, AnimationParameters parameters)
        {
            if(state) return OpenCor(animName, parameters);
            return CloseCor(animName, parameters);
        }

        

        private IEnumerator OpenIE(string animName, AnimationParameters parameters)
        {
            switchedOnce = true;
            var curAnim = Animation.GetAnimation(string.IsNullOrEmpty(animName) ? Animation.currentAnimationName : animName);
            curAnim.Init(this);
            curAnim.StopAllCoroutines();
            
            ChangeState(WindowClasses.StatesRead.Opening);
            OnStartOpen();
            OnStartSwitch(true);
            
            yield return StartCoroutine(curAnim.UpdateOpen(parameters));
            
            ChangeState(WindowClasses.StatesRead.Opened);
            OnEndOpen();
            OnEndSwitch(true);
        }
        private IEnumerator CloseIE(string animName, AnimationParameters parameters)
        {
            switchedOnce = true;
            var curAnim = Animation.GetAnimation(string.IsNullOrEmpty(animName) ? Animation.currentAnimationName : animName);
            curAnim.Init(this);
            curAnim.StopAllCoroutines();
            
            ChangeState(WindowClasses.StatesRead.Closing);
            OnStartClose();
            OnStartSwitch(false);
            
            yield return StartCoroutine(curAnim.UpdateClose(parameters));
            
            OnEndClose();
            ChangeState(WindowClasses.StatesRead.Closed);
            OnEndSwitch(false);
        }

        /*
        private IEnumerator Cor1()
        {
            yield return Cor2();
            yield return Cor2();
            yield return Cor2();
            yield return Cor2();
            yield return Cor2();
            yield return Cor2();
            yield return Cor2();
            yield return Cor2();
            yield return Cor2();
            yield return Cor2();
        }

        private IEnumerator Cor2()
        {
            if(false)
                yield return null;
            // не ждем ничего, сразу выходим
        }*/


        #endregion
        

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
        

        #region Editor

        

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
        
        #endregion
    }

}
