using System;
using System.Collections;
using DredPack.UI.WindowAnimations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DredPack.UI.WindowAnimations
{
    [Serializable]
    public class ScaledPopUp : WindowAnimation
    { 
        [Space]
        public AnimationCurve ScaleOpenCurve = new AnimationCurve(new []{new Keyframe(0,.5f),new Keyframe(0.5f,1.15f), new Keyframe(1f,1f)});
        public AnimationCurve ScaleCloseCurve = new AnimationCurve(new []{new Keyframe(0f,1f), new Keyframe(.6f,.5f)});
        public Transform ScaledObject;
        
        [Header("Alpha")]
        public AlphaTypes AlphaType = AlphaTypes.ByCanvasGroup;
        public enum AlphaTypes { ByCanvasGroup, ByImage, Custom }
        public AnimationCurve AlphaCurve = new AnimationCurve(new []{new Keyframe(0,0), new Keyframe(.5f,1f)});
        public Image Image;
        public Image[] Images;
        public Image[] CanvasGroups;
        
        
        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            base.UpdateOpen(parameters);
            if (AlphaType == AlphaTypes.ByImage)
                window.Components.CanvasGroup.alpha = 1;
            for (float i = 0; i < 1f; i += Time.deltaTime * Speed * parameters.CustomSpeed)
            {
                ScaledObject.localScale = ScaleOpenCurve.Evaluate(i) * Vector3.one;
                UpdateAlpha(i);
                yield return null;
            }
            UpdateAlpha(1f);
        }


        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            base.UpdateClose(parameters);
            for (float i = 0; i < 1f; i += Time.deltaTime * Speed * parameters.CustomSpeed)
            {
                ScaledObject.localScale = ScaleCloseCurve.Evaluate(i) * Vector3.one;
                UpdateAlpha(1f - i);
                yield return null;
            }

            if (AlphaType == AlphaTypes.ByImage)
                window.Components.CanvasGroup.alpha = 0; 
            UpdateAlpha(0);
        }

        private void UpdateAlpha(float value)
        {
            switch (AlphaType)
            {
                case AlphaTypes.ByCanvasGroup: window.Components.CanvasGroup.alpha = AlphaCurve.Evaluate(value); break;
                case AlphaTypes.ByImage: Image.color = new Color(Image.color.r,Image.color.g,Image.color.b, AlphaCurve.Evaluate(value)); break;
            }
        }

    }
}