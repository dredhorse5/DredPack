using System;
using System.Collections;
using DredPack.UI.Animations;
using DredPack.UI.Animations.Modules;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DredPack.UI.Animations
{
    [Serializable]
    public class ScaledPopUp : WindowAnimation
    { 
        [Space]
        public AnimationCurve ScaleOpenCurve = new AnimationCurve(new []{new Keyframe(0,.5f),new Keyframe(0.5f,1.15f), new Keyframe(1f,1f)});
        public AnimationCurve ScaleCloseCurve = new AnimationCurve(new []{new Keyframe(0f,1f), new Keyframe(.6f,.5f)});
        public Transform ScaledObject;
        
        [Header("Alpha")]
        [CurveRange(0, 0, 1, 1, EColor.Green)]
        public AnimationCurve AlphaOpenCurve = new AnimationCurve(new []{new Keyframe(0,0), new Keyframe(.5f,1f)});
        [CurveRange(0, 0, 1, 1, EColor.Red)]
        public AnimationCurve AlphaCloseCurve = new AnimationCurve(new []{new Keyframe(0,1f), new Keyframe(.5f,0)});
        [Space]
        public AlphaTypes AlphaType = AlphaTypes.ByCanvasGroup;
        public enum AlphaTypes { ByCanvasGroup, ByImage, GraphicModule }

        [AllowNesting,ShowIf(nameof(AlphaType), AlphaTypes.ByImage)]
        public Image Image;
        [AllowNesting,ShowIf(nameof(AlphaType), AlphaTypes.GraphicModule)]
        public GraphicModule GraphicModule;

        public override void Init(Window owner)
        {
            base.Init(owner);
            GraphicModule.Init(owner);
        }

        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            yield return StartCoroutine(base.UpdateOpen(parameters));
            for (float i = 0; i < 1f; i += Time.deltaTime * Speed * parameters.CustomSpeed)
            {
                SetOpenTime(i,parameters);
                yield return null;
            }
            SetOpenTime(1f,parameters);
        }

        public override void SetOpenTime(float time, AnimationParameters parameters)
        {
            if (AlphaType != AlphaTypes.ByCanvasGroup)
                window.Components.CanvasGroup.alpha = time <= 0.001f ? 0f : 1f;
            ScaledObject.localScale = ScaleOpenCurve.Evaluate(time) * Vector3.one;
            UpdateAlpha(AlphaOpenCurve.Evaluate(time));
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
            ScaledObject.localScale = ScaleCloseCurve.Evaluate(time) * Vector3.one;
            UpdateAlpha(AlphaCloseCurve.Evaluate(time));
            
            if (AlphaType != AlphaTypes.ByCanvasGroup)
                window.Components.CanvasGroup.alpha = time >= 0.99f ? 0f : 1f;
        }

        private void UpdateAlpha(float value)
        {
            switch (AlphaType)
            {
                case AlphaTypes.ByCanvasGroup: window.Components.CanvasGroup.alpha = value; break;
                case AlphaTypes.ByImage: Image.color = new Color(Image.color.r,Image.color.g,Image.color.b, value); break;
                case AlphaTypes.GraphicModule: GraphicModule.SetOpenTime(value, null); break;
            }
        }

    }
}