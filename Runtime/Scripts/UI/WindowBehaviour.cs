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


        
        
        public virtual Coroutine OpenCor() => Window.OpenCor();
        public virtual Coroutine CloseCor() => Window.CloseCor();
        public virtual Coroutine SwitchCor() => Window.SwitchCor();
        public virtual Coroutine SwitchCor(bool state) => Window.SwitchCor(state);
        
        public void Open() => OpenCor();
        public void Close() => CloseCor();
        public void Switch() => SwitchCor();
        public void Switch(bool state) => SwitchCor(state);


        public virtual void OnStartOpen() { }
        public virtual void OnStartClose() { }
        public virtual void OnStartSwitch(bool state) { }
        public virtual void OnEndOpen() { }
        public virtual void OnEndClose() { }
        public virtual void OnEndSwitch(bool state) { }
        public virtual void OnStateChanged(WindowClasses.StatesRead state) { }
    }
}
