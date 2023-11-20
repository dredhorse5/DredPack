using System;
using DredPack.UI;
using UnityEngine;

namespace DredPack.UI
{
    [RequireComponent(typeof(Window))]
    public class WindowBehaviour : MonoBehaviour, WindowClasses.IWindow, WindowClasses.IWindowCallback
    {
        public Window Window => _window ??= GetComponent<Window>(); private Window _window;

        protected virtual void OnEnable()
        {
            Window.RegisterCallback(this);
        }

        protected virtual void OnDisable()
        {
            Window.RemoveCallback(this);
        }


        
        
        public virtual void Open() => Window.Open();
        public virtual void Close() => Window.Close();
        public virtual void Switch() => Window.Switch();
        public virtual void Switch(bool state) => Window.Switch(state);
        
        
        
        
        public virtual void OnStartOpen() { }
        public virtual void OnStartClose() { }
        public virtual void OnStartSwitch(bool state) { }
        public virtual void OnEndOpen() { }
        public virtual void OnEndClose() { }
        public virtual void OnEndSwitch(bool state) { }
        public virtual void OnStateChanged(WindowClasses.StatesRead state) { }
    }
}
