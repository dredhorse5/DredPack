using System;
using DredPack.UI;
using UnityEngine;

namespace DredPack.UI
{
    [RequireComponent(typeof(Window))]
    public class WindowBehaviour : MonoBehaviour
    {
        public Window Window => _window ??= GetComponent<Window>(); private Window _window;

        protected virtual void OnEnable()
        {
            //TODO: uncomment
            /*Window.OpenEvent.AddListener(OnOpen);
            Window.EndOpenEvent.AddListener(OnOpened);
            Window.CloseEvent.AddListener(OnClose);
            Window.EndCloseEvent.AddListener(OnClosed);
            Window.SwitchEvent.AddListener(OnSwitch);*/
        }

        protected virtual void OnDisable()
        {
            //TODO: uncomment
            /*Window.OpenEvent.RemoveListener(OnOpen);
            Window.EndOpenEvent.RemoveListener(OnOpened);
            Window.CloseEvent.RemoveListener(OnClose);
            Window.EndCloseEvent.RemoveListener(OnClosed);
            Window.SwitchEvent.RemoveListener(OnSwitch);*/
        }

        protected virtual void OnOpen() { }
        protected virtual void OnOpened() { }
        protected virtual void OnClose() { }
        protected virtual void OnClosed(){}
        protected virtual void OnSwitch(bool currentState) { }

        
        //TODO:Window.GetType remove
        public virtual void Open() => Window.GetType();
        public virtual void Close() => Window.GetType();
        public virtual void Switch() => Window.GetType();
        public virtual void Switch(bool state) => Window.GetType();
    }
}
