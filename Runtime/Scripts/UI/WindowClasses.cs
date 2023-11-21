using System;
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
            public Coroutine OpenCor();
            public Coroutine CloseCor();
            public Coroutine SwitchCor();
            public Coroutine SwitchCor(bool state);
            public void Open();
            public void Close();
            public void Switch();
            public void Switch(bool state);
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
                if(CloseButton) CloseButton.onClick.AddListener(() => window.CloseCor());
                if(OpenButton) OpenButton.onClick.AddListener(() =>window.OpenCor());
                if(SwitchButton) SwitchButton.onClick.AddListener(() =>window.SwitchCor());
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
            public string currentAnimationName = "";

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

            [SerializeReference][SerializeField] private WindowAnimation _currentAnimation;

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

            public WindowAnimation[] allAnimations = new WindowAnimation[]
            {
                new Fade(), new Instantly()
            };

            public WindowAnimation GetAnimation(string name)
            {
                for (int i = 0; i < allAnimations.Length; i++)
                {
                    if (allAnimations[i].Name == name)
                        return allAnimations[i];
                }

                //Debug.LogError($"Cant find anim with name: {name}");
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
        //возможность вызвать определенную анимацию
        //возможность задать направление анимации
        //новая анимация - список трансформов, которые можно как угодно двигать, скейлить и вращать
        //загрузка профилей
    }
}