using System;
using System.Collections;
using DredPack.UI.Tabs;

namespace DredPack.UI.Animations
{
#if UNITY_EDITOR
    using UnityEditor;
    [InitializeOnLoad]
#endif
    [Serializable]
    public class External : WindowAnimation
    {
        public WindowAnimationBehaviour Component;
        static External() => AnimationTab.RegisterAnimation(typeof(External));
        public override float SortIndex => 6;
        public override void Init(Window owner)
        {
            if (!window)
            {
                window = owner;
                OnInit(owner);
            }

            Component.Init(owner);
        }

        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            yield return StartCoroutine(Component.UpdateOpen(parameters));
        }

        public override void SetOpenTime(float time, AnimationParameters parameters) => Component.SetOpenTime(time, parameters);
        
        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            yield return StartCoroutine(Component.UpdateClose(parameters));
        }
        public override void SetCloseTime(float time, AnimationParameters parameters) => Component.SetCloseTime(time, parameters);

        public override void StopAllCoroutines()
        {
            Component.StopAllCoroutines();
        }
    }
}