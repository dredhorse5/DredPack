using System;
using System.Collections;
using UnityEngine;

namespace DredPack.UI.WindowAnimations
{
    [Serializable]
    public class Fade : WindowAnimation
    {
        public AnimationCurve Curve = new AnimationCurve(new []{new Keyframe(0,0), new Keyframe(1f,1f)});

        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            for (float i = 0; i < 1f; i += Time.deltaTime * Speed * parameters.CustomSpeed)
            {
                window.Components.CanvasGroup.alpha = Curve.Evaluate(i);
                yield return null;
            }
            window.Components.CanvasGroup.alpha = Curve.Evaluate(1f);
        }

        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            for (float i = 0; i < 1f; i += Time.deltaTime * Speed * parameters.CustomSpeed)
            {
                window.Components.CanvasGroup.alpha = Curve.Evaluate(1f - i);
                yield return null;
            }

            window.Components.CanvasGroup.alpha = Curve.Evaluate(0);
        }
        
        
    }
}