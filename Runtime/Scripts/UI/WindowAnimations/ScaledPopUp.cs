using System;
using System.Collections;
using DredPack.UI.WindowAnimations;
using NaughtyAttributes;
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
        [CurveRange(0, 0, 1, 1, EColor.Green)]
        public AnimationCurve AlphaOpenCurve = new AnimationCurve(new []{new Keyframe(0,0), new Keyframe(.5f,1f)});
        [CurveRange(0, 0, 1, 1, EColor.Red)]
        public AnimationCurve AlphaCloseCurve = new AnimationCurve(new []{new Keyframe(0,.5f), new Keyframe(.5f,0)});
        [Space]
        public AlphaTypes AlphaType = AlphaTypes.ByCanvasGroup;
        public enum AlphaTypes { ByCanvasGroup, ByImage_Beta/*, Custom*/ }

        [AllowNesting,ShowIf(nameof(AlphaType), AlphaTypes.ByImage_Beta)]
        public Image Image;
        /*public Image[] Images;
        public CanvasGroup[] CanvasGroups;

        private float[] imagesAplhas;
        private float[] canvasGroupsAplhas;

        public override void OnInit(Window owner)
        {
            imagesAplhas = new float[Images.Length];
            for (int i = 0; i < Images.Length; i++)
                if (Images[i])
                    imagesAplhas[i] = Images[i].color.a;
            
            canvasGroupsAplhas = new float[CanvasGroups.Length];
            for (var i = 0; i < CanvasGroups.Length; i++)
                if (CanvasGroups[i])
                    canvasGroupsAplhas[i] = CanvasGroups[i].alpha;
        }*/

        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            base.UpdateOpen(parameters);
            if (AlphaType == AlphaTypes.ByImage_Beta)
                window.Components.CanvasGroup.alpha = 1;
            for (float i = 0; i < 1f; i += Time.deltaTime * Speed * parameters.CustomSpeed)
            {
                ScaledObject.localScale = ScaleOpenCurve.Evaluate(i) * Vector3.one;
                UpdateAlpha(AlphaOpenCurve.Evaluate(i));
                yield return null;
            }
            UpdateAlpha(AlphaOpenCurve.Evaluate(1f));
        }


        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            base.UpdateClose(parameters);
            for (float i = 0; i < 1f; i += Time.deltaTime * Speed * parameters.CustomSpeed)
            {
                ScaledObject.localScale = ScaleCloseCurve.Evaluate(i) * Vector3.one;
                UpdateAlpha(AlphaCloseCurve.Evaluate(i));
                yield return null;
            }

            if (AlphaType == AlphaTypes.ByImage_Beta)
                window.Components.CanvasGroup.alpha = 0; 
            UpdateAlpha(AlphaCloseCurve.Evaluate(1f));
        }

        private void UpdateAlpha(float value)
        {
            switch (AlphaType)
            {
                case AlphaTypes.ByCanvasGroup: window.Components.CanvasGroup.alpha = value; break;
                case AlphaTypes.ByImage_Beta: Image.color = new Color(Image.color.r,Image.color.g,Image.color.b, value); break;
                /*case AlphaTypes.Custom:
                    foreach (var img in Images)
                    {
                        img.color = new Color(img.color.r, img.color.g, img.color.b, value);
                    }
                    break;*/
            }
        }

    }
}