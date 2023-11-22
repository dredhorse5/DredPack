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
            window.Components.CanvasGroup.alpha = 1f;
            yield break;
        }

        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            window.Components.CanvasGroup.alpha = 0f;
            yield break;
        }
        
        
    }
}