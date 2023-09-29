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
            Window.OpenEvent.AddListener(OnOpen);
            Window.EndOpenEvent.AddListener(OnOpened);
            Window.CloseEvent.AddListener(OnClose);
            Window.EndCloseEvent.AddListener(OnClosed);
            Window.SwitchEvent.AddListener(OnSwitch);
        }

        protected virtual void OnDisable()
        {
            Window.OpenEvent.RemoveListener(OnOpen);
            Window.EndOpenEvent.RemoveListener(OnOpened);
            Window.CloseEvent.RemoveListener(OnClose);
            Window.EndCloseEvent.RemoveListener(OnClosed);
            Window.SwitchEvent.RemoveListener(OnSwitch);
        }

        protected virtual void OnOpen() { }
        protected virtual void OnOpened() { }
        protected virtual void OnClose() { }
        protected virtual void OnClosed(){}
        protected virtual void OnSwitch(bool currentState) { }

        
        public virtual void Open() => Window.Open();
        public virtual void Close() => Window.Close();
        public virtual void Switch() => Window.Switch();
        public virtual void Switch(bool state) => Window.Switch(state);
    }
}
