using System;
using System.Collections;

namespace DredPack.UI.WindowAnimations
{
    [Serializable]
    public class Instantly : WindowAnimation
    {
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