using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DredPack.UI.WindowAnimations.Modules
{
    [Serializable]
    public class GraphicModule : WindowAnimationModule
    {
        public AnimationCurve Curve = new AnimationCurve(new []{new Keyframe(0,0), new Keyframe(1f,1f)});
        public Graphic[] Graphics;
        public CanvasGroup[] CanvasGroups;

        
        private float[] graphicAlphas;
        private float[] canvasGroupsAlphas;

        
        public override void OnInit(Window owner)
        {
            base.OnInit(owner);
            
            graphicAlphas = new float[Graphics.Length];
            for (int i = 0; i < Graphics.Length; i++)
                if (Graphics[i])
                    graphicAlphas[i] = Graphics[i].color.a;
            
            canvasGroupsAlphas = new float[CanvasGroups.Length];
            for (var i = 0; i < CanvasGroups.Length; i++)
                if (CanvasGroups[i])
                    canvasGroupsAlphas[i] = CanvasGroups[i].alpha;
        }
        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            for (float t = 0; t < 1f; t += Time.deltaTime * parameters.CustomSpeed)
            {
                SetOpenTime(t, parameters);
                yield return null;
            }
        }

        public override void SetOpenTime(float time, AnimationParameters parameters)
        {
            for (int i = 0; i < Graphics.Length; i++)
            {
                var graphic = Graphics[i];
                if (graphic)
                {
                    var c = graphic.color;
                    graphic.color = new Color(c.r, c.g, c.b, Mathf.Lerp(0f, graphicAlphas[i], Curve.Evaluate(time)));
                }
            }

            for (int i = 0; i < CanvasGroups.Length; i++)
            {
                var canvasGroup = CanvasGroups[i];
                if (canvasGroup)
                    canvasGroup.alpha = Mathf.Lerp(0f, canvasGroupsAlphas[i], Curve.Evaluate(time));
            }
        }
        
        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            for (float t = 0; t < 1f; t += Time.deltaTime * parameters.CustomSpeed)
            {
                SetCloseTime(t, parameters);
                yield return null;
            }
        }
        
        public override void SetCloseTime(float time, AnimationParameters parameters)
        {
            SetOpenTime(1f - time, parameters);
        }
    }
}