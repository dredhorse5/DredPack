using System;
using DredPack.UI;
using DredPack.UI.Animations;
using DredPack.UI.Some;
using UnityEngine;

namespace DredPack.UI
{
    [RequireComponent(typeof(Window))]
    public class WindowBehaviour : MonoBehaviour, IWindow, IWindowCallback
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


        
        
        


        
        public virtual void OnStartOpen() { }
        public virtual void OnStartClose() { }
        public virtual void OnStartSwitch(bool state) { }
        public virtual void OnEndOpen() { }
        public virtual void OnEndClose() { }
        public virtual void OnEndSwitch(bool state) { }
        public virtual void OnStateChanged(StatesRead state) { }
        
        
        
        public void Open() => Window.Open();
        public void Open(string animName) => Window.Open(animName);
        public void Open(AnimationParameters parameters) => Window.Open(parameters);
        public Coroutine OpenCor(string animName, AnimationParameters parameters) => Window.OpenCor(animName, parameters);



        public void Close() => Window.Close();
        public void Close(string animName) => Window.Close(animName);
        public void Close(AnimationParameters parameters) => Window.Close(parameters);
        public Coroutine CloseCor(string animName, AnimationParameters parameters) => Window.CloseCor(animName, parameters);


        public void Switch() => Window.Switch();
        public void Switch(string animName) => Window.Switch(animName);
        public void Switch(AnimationParameters parameters) => Window.Switch(parameters);
        public Coroutine SwitchCor(string animName, AnimationParameters parameters) => Window.SwitchCor(animName, parameters);
        

        public void Switch(bool state) => Window.Switch(state);
        public void Switch(bool state, string animName) => Window.Switch(state, animName);
        public void Switch(bool state, AnimationParameters parameters) => Window.Switch(state, parameters);
        public Coroutine SwitchCor(bool state, string animName, AnimationParameters parameters) => Window.SwitchCor(state, animName, parameters);
    }
}
