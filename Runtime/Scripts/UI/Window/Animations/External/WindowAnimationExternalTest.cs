using System.Collections;
using UnityEngine;

namespace DredPack.UI.WindowAnimations
{
    public class WindowAnimationExternalTest : WindowAnimationBehaviour
    {
        [Header("It's test fade animation")]
        public float Speed = 1;
        public AnimationCurve Curve = new AnimationCurve(new []{new Keyframe(0,0), new Keyframe(1f,1f)});

        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            yield return StartCoroutine(base.UpdateOpen(parameters));
            for (float i = 0; i < 1f; i += Time.deltaTime * Speed * parameters.CustomSpeed)
            {
                SetOpenTime(i, parameters);
                yield return null;
            }
            SetOpenTime(1f, parameters);
        }

        public override void SetOpenTime(float time, AnimationParameters parameters)
        {
            window.Components.CanvasGroup.alpha = Curve.Evaluate(time);
        }

        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            yield return StartCoroutine(base.UpdateClose(parameters));
            for (float i = 0; i < 1f; i += Time.deltaTime * Speed * parameters.CustomSpeed)
            {
                SetCloseTime(i, parameters);
                yield return null;
            }

            SetCloseTime(1f, parameters);
        }
        
        public override void SetCloseTime(float time, AnimationParameters parameters)
        {
            window.Components.CanvasGroup.alpha = Curve.Evaluate(1f - time);
        }


    }
}