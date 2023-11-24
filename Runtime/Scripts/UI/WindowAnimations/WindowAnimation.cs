using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DredPack.UI.WindowAnimations
{
    [Serializable]
    public class WindowAnimation
    {
        public float Speed = 1f;
        public SwitchDelay SwitchDelay;
        
        
        
        protected Window window;
        private List<Coroutine> launchedCoroutines = new List<Coroutine>();

        public virtual string Name => this.GetType().Name;
        
        public void Init(Window owner)
        {
            if (!window)
            {
                window = owner;
                OnInit(owner);
            }
        }

        public virtual void OnInit(Window owner) { }

        public virtual IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            if (SwitchDelay.Open > 0.001f)
                yield return new WaitForSeconds(SwitchDelay.Open);
            yield return null;
        }
        
        public virtual IEnumerator UpdateClose(AnimationParameters parameters)
        {
            if (SwitchDelay.Close > 0.001f)
                yield return new WaitForSeconds(SwitchDelay.Close);
            yield return null;
        }

        public void StopAllCoroutines()
        {
            foreach (var cor in launchedCoroutines)
            {
                if(cor != null)
                    window.StopCoroutine(cor);
            }
            launchedCoroutines.Clear();
        }

        protected void StartCoroutine(IEnumerator coroutine)
        {
            if (launchedCoroutines == null || launchedCoroutines.Count == 0)
                launchedCoroutines = new List<Coroutine>();

            var cor = window.StartCoroutine(coroutine);
            launchedCoroutines.Add(cor);
        }
    }

    [Serializable]
    public class AnimationParameters
    {
        public readonly float CustomSpeed = 1f;
    }

    [Serializable]
    public struct SwitchDelay
    {
        public float Open;
        public float Close;
    }
}