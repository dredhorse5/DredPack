using DredPack.UI.Animations;
using UnityEngine;

namespace DredPack.UI.Some
{
    
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
}