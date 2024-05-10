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
    public class Instantly : WindowAnimation
    {
        static Instantly() => AnimationTab.RegisterAnimation(typeof(Instantly));
        public override float SortIndex => 1;
        //public override string Name => "Instantly";
        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            SetOpenTime(1f, parameters);
            yield break;
        }

        public override void SetOpenTime(float time, AnimationParameters parameters)
        {
            window.Components.CanvasGroup.alpha = 1f;
        }


        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            SetCloseTime(0f, parameters);
            yield break;
        }
        
        public override void SetCloseTime(float time, AnimationParameters parameters)
        {
            window.Components.CanvasGroup.alpha = 0f;
        }
        
    }
}