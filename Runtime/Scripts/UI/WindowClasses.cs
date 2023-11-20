using System;
using System.Collections.Generic;
using DredPack.Audio;
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
        
        [Serializable]
        public class Components
        {
            public GameObject DisableableObject;
            public Canvas Canvas;
            public GraphicRaycaster Raycaster;
            public Graphic BackgroundImage;
            public Graphic CanvasGroup;
        }
        

        #region Tabs
        [Serializable]
        public class GeneralTab
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
        }

        
        
        [Serializable]
        public class EventsTab : IWindowCallback
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
        public class AudioTab : IWindowCallback
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