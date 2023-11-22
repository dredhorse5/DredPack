using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DredPack.UI.WindowAnimations
{
    [Serializable]
    public class WindowAnimation
    {
        public virtual string Name => this.GetType().Name;
        public float Speed = 1f;
        protected Window window;
        private List<Coroutine> launchedCoroutines = new List<Coroutine>();

        public virtual void Init(Window owner)
        {
            window = owner;
        }

        public virtual IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            yield return null;
        }
        
        public virtual IEnumerator UpdateClose(AnimationParameters parameters)
        {
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

    public class AnimationParameters
    {
        public readonly float CustomSpeed = 1f;
    }
}