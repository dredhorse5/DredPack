﻿using System;
using System.Collections.Generic;
using DredPack.Audio;
using DredPack.UI.WindowAnimations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DredPack.UI
{
    public static class WindowClasses
    {
        public interface IWindowCallback
        {
            public void OnStartOpen();
            public void OnStartClose();
            public void OnStartSwitch(bool state);

            public void OnEndOpen();
            public void OnEndClose();
            public void OnEndSwitch(bool state);

            public void OnStateChanged(StatesRead state);
        }
        
        public interface IWindow
        {
            public void Open();
            public void Open(string animName);
            public void Open(AnimationParameters parameters);
            public Coroutine OpenCor(string animName, AnimationParameters parameters);
            
            
            public void Close();
            public void Close(string animName);
            public void Close(AnimationParameters parameters);
            public Coroutine CloseCor(string animName, AnimationParameters parameters);
            
            
            public void Switch();
            public void Switch(string animName);
            public void Switch(AnimationParameters parameters);
            public Coroutine SwitchCor(string animName, AnimationParameters parameters);
            
            
            public void Switch(bool state);
            public void Switch(bool state, string animName);
            public void Switch(bool state, AnimationParameters parameters);
            public Coroutine SwitchCor(bool state,string animName, AnimationParameters parameters);
        }
        
        [Serializable]
        public struct Components
        {
            public GameObject DisableableObject;
            public Canvas Canvas;
            public GraphicRaycaster Raycaster;
            public Graphic BackgroundImage;
            public CanvasGroup CanvasGroup;
        }
        

        #region Tabs
        
        [Serializable]
        public class Tab
        {
            protected Window window;
            public virtual void Init(Window owner) => window = owner;
        }
        
        [Serializable]
        public class GeneralTab : Tab, IWindowCallback
        {
            public StatesRead CurrentState;
            public StatesAwakeMethod StateOnAwakeMethod;
            public string AnimationOnAwake = "Instantly";
            public StatesAwake StateOnAwake;

            //Buttons
            public Button CloseButton;
            public Button OpenButton;
            public Button SwitchButton;
            
            //Some
            public bool Disableable = false;
            public bool EnableableCanvas = false;
            public bool EnableableRaycaster = true;
            public bool CloseIfAnyWindowOpen;
            public bool CloseOnOutsideClick;

            public override void Init(Window owner)
            {
                base.Init(owner);
                if(CloseButton) CloseButton.onClick.AddListener(window.Close);
                if(OpenButton) OpenButton.onClick.AddListener(window.Open);
                if(SwitchButton) SwitchButton.onClick.AddListener(window.Switch);
            }

            public void OnStartOpen()
            {
                if(Disableable && window.Components.DisableableObject)
                    window.Components.DisableableObject.gameObject.SetActive(true);
                if (EnableableCanvas && window.Components.Canvas)
                    window.Components.Canvas.enabled = true;
            }

            public void OnStartClose()
            {
                if (EnableableRaycaster && window.Components.Raycaster)
                    window.Components.Raycaster.enabled = false;
                
                if (window.Components.CanvasGroup)
                {
                    window.Components.CanvasGroup.interactable = false;
                    window.Components.CanvasGroup.blocksRaycasts = false;
                }
            }

            public void OnStartSwitch(bool state) { }

            public void OnEndOpen()
            {
                if (EnableableRaycaster && window.Components.Raycaster)
                    window.Components.Raycaster.enabled = true;
                if (window.Components.CanvasGroup)
                {
                    window.Components.CanvasGroup.interactable = true;
                    window.Components.CanvasGroup.blocksRaycasts = true;
                }
            }

            public void OnEndClose()
            {
                if(Disableable && window.Components.DisableableObject)
                    window.Components.DisableableObject.gameObject.SetActive(false);
                if (EnableableCanvas && window.Components.Canvas)
                    window.Components.Canvas.enabled = false;
            }

            public void OnEndSwitch(bool state) { }

            public void OnStateChanged(StatesRead state) { }
        }

        
        
        [Serializable]
        public class EventsTab : Tab, IWindowCallback
        {
            public UnityEvent StartOpen;
            public UnityEvent StartClose;
            public UnityEvent<bool> StartSwitch;

            public UnityEvent EndOpen;
            public UnityEvent EndClose;
            public UnityEvent<bool> EndSwitch;

            public UnityEvent<StatesRead> StateChanged;

            public void OnStartOpen() => StartOpen?.Invoke();
            public void OnStartClose() => StartClose?.Invoke();
            public void OnStartSwitch(bool state) => StartSwitch?.Invoke(true);

            public void OnEndOpen() => EndOpen?.Invoke();
            public void OnEndClose() => EndClose?.Invoke();
            public void OnEndSwitch(bool state) => EndSwitch?.Invoke(state);
            public void OnStateChanged(StatesRead state) => StateChanged?.Invoke(state);
        }
        
        [Serializable]
        public class AudioTab : Tab, IWindowCallback
        {
            public SCPAudio[] List;


            public void OnStartOpen() => Play(StatesForChanged.StartOpen);
            public void OnStartClose() => Play(StatesForChanged.StartClose);
            public void OnStartSwitch(bool state) => Play(StatesForChanged.StartSwitch);

            public void OnEndOpen() => Play(StatesForChanged.EndOpen);
            public void OnEndClose() => Play(StatesForChanged.EndClose);
            public void OnEndSwitch(bool state) => Play(StatesForChanged.EndSwitch);
            public void OnStateChanged(StatesRead state) {}

            public void Play(StatesForChanged state)
            {
                for (int i = 0; i < List.Length; i++)
                    List[i]?.TryExecute(state);
            }
        }
        
        [Serializable]
        public class AnimationTab : Tab
        {
            public string currentAnimationName
            {
                get
                {
                    if (string.IsNullOrEmpty(_currentAnimationName))
                        _currentAnimationName = allAnimations[0].Name;
                    return _currentAnimationName;
                }
                set
                {
                    _currentAnimationName = value;
                    _currentAnimation = GetAnimation(_currentAnimationName);
                }
            }

            public string _currentAnimationName = "";

            public WindowAnimation currentAnimation
            {
                get
                {
                    if (string.IsNullOrEmpty(currentAnimationName))
                        currentAnimationName = allAnimations[0].Name;
                    _currentAnimation = GetAnimation(currentAnimationName);
                    return _currentAnimation;
                }
            }

            [SerializeReference][SerializeField] public WindowAnimation _currentAnimation;

            public string[] allAnimationNames
            {
                get
                {
                    string[] list = new string[allAnimations.Length];
                    for (int i = 0; i < allAnimations.Length; i++)
                        list[i] = allAnimations[i].Name;
                    return list;
                }
            }
            private WindowAnimation[] allAnimations => new WindowAnimation[]
            {
                fade, instantly, animator
            };

            [SerializeReference, SerializeField]Fade fade = new Fade();
            [SerializeReference, SerializeField] Instantly instantly = new Instantly();
            [SerializeReference, SerializeField] WindowAnimations.Animator animator = new WindowAnimations.Animator();
            
            
            public WindowAnimation GetAnimation(string name)
            {
                //finding needed animation
                for (int i = 0; i < allAnimations.Length; i++)
                {
                    if (allAnimations[i].Name == name)
                        return allAnimations[i];
                }
                
                // if nothing, finding now animation
                for (int i = 0; i < allAnimations.Length; i++)
                {
                    if (allAnimations[i].Name == currentAnimationName)
                        return allAnimations[i];
                }
                
                // or returns first animation
                return allAnimations[0];
            }
            
        }

        #endregion

        #region Enums

        

        public enum StatesRead
        {
            Opened,
            Opening,
            Closed,
            Closing
        }
        public enum StatesForChanged
        {
            StartOpen,
            EndOpen,
            StartClose,
            EndClose,
            StartSwitch,
            EndSwitch
        }
        public enum StatesAwake
        {
            Open,
            Close
        }
        public enum StatesAwakeMethod
        {
            Start,
            Awake,
            OnEnable,
            Nothing
        }

        #endregion

        #region StateChangedProperty

        


        [Serializable]
        public class StateChangedProperty
        {
            public StatesForChanged State;

            public virtual void TryExecute(StatesForChanged state)
            {
                if (State == state)
                    Execute();
            }
            protected virtual void Execute(){}
        }

        [Serializable]
        public class SCPAudio : StateChangedProperty
        {
            public AudioField Audio;
            protected override void Execute() => Audio.Play();
        }
        
        

        #endregion
        
        //возможность выбирать, когда вызывать StatesAwake
        //вызывать ли его вообще
        //вызывать ли ивенты при старте
        //задержки анимаций
        //возможность вызвать определенную анимацию
        //возможность задать направление анимации
        //новая анимация - список трансформов, которые можно как угодно двигать, скейлить и вращать
        //загрузка профилей
        //запуск совершенно разных анимаций через передачу их как параметр
    }
}