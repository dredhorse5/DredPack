using System;
using DredPack.Audio;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DredPack.UI.Some
{


    [Serializable]
    public class Components
    {
        public GameObject DisableableObject;
        [Header("asd")]
        public Canvas Canvas;
        public GraphicRaycaster Raycaster;
        public Graphic BackgroundImage;
        public CanvasGroup CanvasGroup;
        public Selectable SelectableOnOpen;
        public UnityEngine.Camera CanvasCamera => Canvas.worldCamera;
    }


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

        protected virtual void Execute()
        {
        }
    }

    [Serializable]
    public class SCPAudio : StateChangedProperty
    {
        public AudioField Audio;
        protected override void Execute() => Audio.Play();
    }



    #endregion

    //---возможность выбирать, когда вызывать StatesAwake
    //---вызывать ли его вообще
    //вызывать ли ивенты при старте
    //---задержки анимаций
    //---возможность вызвать определенную анимацию
    //возможность задать направление анимации
    //новая анимация - список трансформов, которые можно как угодно двигать, скейлить и вращать
    //загрузка профилей
    //запуск совершенно разных анимаций через передачу их как параметр
    //возможность у каждой анимации задавать открытое / закрытое состояние моментально

}