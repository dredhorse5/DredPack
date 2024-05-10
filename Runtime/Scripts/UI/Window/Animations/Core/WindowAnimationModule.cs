using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DredPack.UI.Animations
{
    public class WindowAnimationModule : IWindowAnimation
    {
        protected Window window;
        private List<Coroutine> launchedCoroutines = new List<Coroutine>();

        public virtual string Name => this.GetType().Name;
        public virtual float SortIndex => 100f;
        
        public virtual void Init(Window owner)
        {
            if (!window)
            {
                window = owner;
                OnInit(owner);
            }
        }

        public virtual void OnInit(Window owner) { }

        public virtual IEnumerator UpdateOpen(AnimationParameters parameters) {yield break;}
        public virtual void SetOpenTime(float time, AnimationParameters parameters) { }
        
        public virtual IEnumerator UpdateClose(AnimationParameters parameters) {yield break;}
        public virtual void SetCloseTime(float time, AnimationParameters parameters) { }

        public virtual void StopAllCoroutines()
        {
            foreach (var cor in launchedCoroutines)
            {
                if(cor != null)
                    window.StopCoroutine(cor);
            }
            launchedCoroutines.Clear();
        }

        protected virtual Coroutine StartCoroutine(IEnumerator coroutine)
        {
            if (launchedCoroutines == null || launchedCoroutines.Count == 0)
                launchedCoroutines = new List<Coroutine>();

            var cor = window.StartCoroutine(coroutine);
            launchedCoroutines.Add(cor);
            return cor;
        }
    }
}