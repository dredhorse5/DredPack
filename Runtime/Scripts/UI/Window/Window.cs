using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DredPack.UI.Some;
using DredPack.UI.Tabs;
using DredPack.UI.Animations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DredPack.UI
{
    
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class Window : MonoBehaviour, IWindow, IWindowCallback
    {
        [SerializeReference] public List<WindowTab> AllTabs = new List<WindowTab>();
        public GeneralTab General => GetTab<GeneralTab>();
        public AnimationTab Animation => GetTab<AnimationTab>();

        public Components Components = new Components();

        private HashSet<IWindowCallback> callbacks = new HashSet<IWindowCallback>();

        

        private Coroutine switchCor;
        [NonSerialized]
        private bool switchedOnce = false;
        
        private void Awake()
        {
            FindAllTabs();
            
            AllTabs.ForEach(_ => _.Init(this));
            
            foreach (var tab in AllTabs)
                if (tab is IWindowCallback callback)
                    RegisterCallback(callback);
            
            
            if(General.StateOnAwakeMethod == StatesAwakeMethod.Awake && !switchedOnce)
            {
                Switch(General.StateOnAwake != StatesAwake.Open, "Instantly");
                Switch(General.StateOnAwake == StatesAwake.Open, General.AnimationOnAwake);
            }
        }

        public void FindAllTabs()
        { 
            if(AllTabs != null)
                AllTabs = AllTabs.FindAll(_ => _ != null);
            var enumerable = typeof(WindowTab).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(WindowTab))).ToList();
            foreach (var ie in enumerable)
                if (AllTabs.Find(_ => _.GetType() == ie) == null)
                    AllTabs.Add((WindowTab)Activator.CreateInstance(ie));
            AllTabs = AllTabs.OrderBy(_ => _.InspectorDrawSort).ToList();
        }

        public T GetTab<T>() where T : WindowTab
        {
            return AllTabs.Find(_ => _.GetType() == typeof(T)) as T;
        }

        private void Start()
        {
            if(General.StateOnAwakeMethod == StatesAwakeMethod.Start && !switchedOnce)
            {
                Switch(General.StateOnAwake != StatesAwake.Open, "Instantly");
                Switch(General.StateOnAwake == StatesAwake.Open, General.AnimationOnAwake);
            }
        }

        private void OnEnable()
        {
            if(General.StateOnAwakeMethod == StatesAwakeMethod.OnEnable && !switchedOnce)
            {
                Switch(General.StateOnAwake != StatesAwake.Open, "Instantly");
                Switch(General.StateOnAwake == StatesAwake.Open, General.AnimationOnAwake);
            }
            EventsTab.StartSwitchStatic.AddListener(OnStartSwitchStatic);
            EventsTab.EndSwitchStatic.AddListener(OnEndSwitchStatic);
        }

        private void OnDisable()
        {
            EventsTab.StartSwitchStatic.RemoveListener(OnStartSwitchStatic);
            EventsTab.EndSwitchStatic.RemoveListener(OnEndSwitchStatic);
        }

        private void Update()
        {
            General.CheckOutsideClick();
        }

        private void ChangeState(StatesRead state)
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
            if (General.CurrentState == StatesRead.Opened ||
                General.CurrentState == StatesRead.Opening)
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
            if (General.CurrentState == StatesRead.Closed ||
                General.CurrentState == StatesRead.Closing)
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
            if (General.CurrentState == StatesRead.Closed ||
                General.CurrentState == StatesRead.Closing)
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
            var curAnim = Animation.GetOpenAnimation(string.IsNullOrEmpty(animName) ? Animation.CurrentOpenAnimationName : animName);
            curAnim.Init(this);
            curAnim.StopAllCoroutines();
            
            ChangeState(StatesRead.Opening);
            OnStartOpen();
            OnStartSwitch(true);
            Animation.LastPlayedAnimation?.SetOpenTime(1f);
            curAnim?.SetCloseTime(1f);
            Animation.LastPlayedAnimation = curAnim;
            
            yield return StartCoroutine(curAnim.UpdateOpen(parameters));
             
            ChangeState(StatesRead.Opened);
            OnEndOpen();
            OnEndSwitch(true);
        }
        private IEnumerator CloseIE(string animName, AnimationParameters parameters)
        {
            switchedOnce = true;
            WindowAnimationModule curAnim = null;
            if(Animation.DualMode)
            {
                
                curAnim = Animation.GetCloseAnimation(string.IsNullOrEmpty(animName)
                    ? Animation.CurrentCloseAnimationName
                    : animName);
            }
            else
                curAnim = Animation.GetOpenAnimation(string.IsNullOrEmpty(animName) ? Animation.CurrentOpenAnimationName : animName);
            curAnim.Init(this);
            curAnim.StopAllCoroutines();
            
            ChangeState(StatesRead.Closing);
            OnStartClose();
            OnStartSwitch(false);
            Animation.LastPlayedAnimation?.SetOpenTime(1f);
            curAnim?.SetOpenTime(1f);
            Animation.LastPlayedAnimation = curAnim;
            
            yield return StartCoroutine(curAnim.UpdateClose(parameters));
            
            OnEndClose();
            ChangeState(StatesRead.Closed);
            OnEndSwitch(false);
        }

        #endregion
        

        #region Callbacks


        public void RegisterCallback(IWindowCallback[] _callbacks)
        {
            foreach (var cl in _callbacks)
                if(cl != null)
                    RegisterCallback(cl);
        }
        public void RegisterCallback(IWindowCallback callback) => callbacks.Add(callback);
        
        public void RemoveCallback(IWindowCallback[] _callbacks)
        {
            foreach (var cl in _callbacks)
                if(cl != null)
                    RemoveCallback(cl);
        }
        public void RemoveCallback(IWindowCallback callback) => callbacks.Remove(callback);
        
        

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
            EventsTab.StartSwitchStatic?.Invoke(this, state);
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
            EventsTab.EndSwitchStatic?.Invoke(this, state);
        }

        public void OnStateChanged(StatesRead state)
        {
            foreach (var cl in callbacks)
                cl?.OnStateChanged(state);
        }


        private void OnStartSwitchStatic(Window window, bool state)
        {
            if (General.CloseIfAnyWindowOpen && state && window != this)
            {
                if(General.CloseIfAnyWindowOpenType == GeneralTab.CloseIfAnyWindowOpenTypes.OnStart)
                    Close();
            }
        }

        private void OnEndSwitchStatic(Window window, bool state)
        {
            if (General.CloseIfAnyWindowOpen && state && window != this)
            {
                if(General.CloseIfAnyWindowOpenType == GeneralTab.CloseIfAnyWindowOpenTypes.OnEnd)
                    Close();
            }
        }
        
        
        #endregion
        

        #region Editor

        

        #if UNITY_EDITOR
        private void Reset() => InitComponents();
        private void OnValidate() => InitComponents();


        private void InitComponents()
        {
            FindAllTabs(); 
            if(Components.DisableableObject == null)
                Components.DisableableObject = gameObject;
            if(Components.SelectableOnOpen == null)
                Components.SelectableOnOpen = GetComponentInChildren<Selectable>();

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
            
            //if(Components.Canvas.worldCamera)
            
            EditorUtility.SetDirty(this);
        }
        #endif
        
        #endregion
    }

}
