using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace DredPack.UI.WindowAnimations
{
    [Serializable]
    public class SideAppear : WindowAnimation
    {
        [Header("Curves")]
        public CurveTypes CurveType;
        [AllowNesting,ShowIf(nameof(CurveType), CurveTypes.Curve)]
        public AnimationCurve PanelsCurve = new AnimationCurve(new[] {new Keyframe(0, 0,4.57445812f,4.57445812f), new Keyframe(.6f, 1.1f,0f,0f), new Keyframe(1, 1,0f,0f)});
        
        [AllowNesting,ShowIf(nameof(CurveType), CurveTypes.ConstantBounce)]
        public float BounceValue = 20f;
        [Range(0,1f), AllowNesting,ShowIf(nameof(CurveType), CurveTypes.ConstantBounce)]
        public float BounceTime = .6f;
        
        [Header("Panels")]
        public RectTransform Up;
        public RectTransform Right;
        public RectTransform Down;
        public RectTransform Left;
        
        [Header("Graphics")]
        public Graphic[] Graphics;
        public CanvasGroup[] CanvasGroups;

        
        public enum CurveTypes { Curve, ConstantBounce }
        private float[] imagesAlphas;
        private float[] canvasGroupsAlphas;
    }
}