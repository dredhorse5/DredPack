using System;
using System.Collections;
using System.Collections.Generic;
using DredPack.UI.Animations.Modules;
using DredPack.UI.Tabs;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace DredPack.UI.Animations
{
#if UNITY_EDITOR
    using UnityEditor;
    [InitializeOnLoad]
#endif
    [Serializable]
    public class SideAppear : WindowAnimation
    {
        static SideAppear() => AnimationTab.RegisterAnimation(typeof(SideAppear));
        public override float SortIndex => 4;
        
        
        [Header("Curves")]
        public SwitchCurve Open = new SwitchCurve(SwitchCurve.Types.ConstantBounce,
            new AnimationCurve(new[] {new Keyframe(0, 0,4.57445812f,4.57445812f), new Keyframe(.6f, 1.1f,0f,0f), new Keyframe(1, 1,0f,0f)}), 
            .6f,
            20f);
        public SwitchCurve Close = new SwitchCurve(SwitchCurve.Types.Curve,
            new AnimationCurve(new[] {new Keyframe(0, 1f), new Keyframe(.6f, 0)}), 
            .6f,
            20f);
        
        [Header("Panels")]
        public RectTransform[] Up = new RectTransform[]{};
        public RectTransform[] Right= new RectTransform[]{};
        public RectTransform[] Down= new RectTransform[]{};
        public RectTransform[] Left= new RectTransform[]{};

        [Header("Graphics")]
        public GraphicModule Graphics = new GraphicModule();


        private float[] upMaxes;
        private float[] rightMaxes;
        private float[] downMaxes;
        private float[] leftMaxes;

        public override void Init(Window owner)
        {
            base.Init(owner);
            Graphics.Init(owner);
        }

        public override void OnInit(Window owner)
        {
            base.OnInit(owner);
            //UpdateAnchors();
            upMaxes = new float[Up.Length];
            for (int i = 0; i < Up.Length; i++)
                if (Up[i])
                    upMaxes[i] = -Up[i].anchoredPosition.y;
            
            rightMaxes = new float[Right.Length];
            for (int i = 0; i < Right.Length; i++)
                if (Right[i])
                    rightMaxes[i] = -Right[i].anchoredPosition.x;
            
            downMaxes = new float[Down.Length];
            for (int i = 0; i < Down.Length; i++)
                if (Down[i])
                    downMaxes[i] = Down[i].anchoredPosition.y;
            
            leftMaxes = new float[Left.Length];
            for (int i = 0; i < Left.Length; i++)
                if (Left[i])
                    leftMaxes[i] = Left[i].anchoredPosition.x;

        }

        public void UpdateAnchors()
        {
            for (int i = 0; i < Up.Length; i++)
            {
                if (Up[i])
                {
                    Up[i].anchorMax = new Vector2(Up[i].anchorMax.x, 1);
                    Up[i].anchorMin = new Vector2(Up[i].anchorMin.x, 1);
                }
            }
            
            for (int i = 0; i < Right.Length; i++)
            {
                if (Right[i])
                {
                    Right[i].anchorMax = new Vector2(1, Right[i].anchorMax.y);
                    Right[i].anchorMin = new Vector2(1, Right[i].anchorMin.y);
                    Right[i].ForceUpdateRectTransforms();
                }
            }
            
            for (int i = 0; i < Down.Length; i++)
            {
                if (Down[i])
                {
                    Down[i].anchorMax = new Vector2(Down[i].anchorMax.x, 0);
                    Down[i].anchorMin = new Vector2(Down[i].anchorMin.x, 0);
                    Down[i].ForceUpdateRectTransforms();
                }
            }
            
            for (int i = 0; i < Left.Length; i++)
            {
                if (Left[i])
                {
                    Left[i].anchorMax = new Vector2(0, Left[i].anchorMax.y);
                    Left[i].anchorMin = new Vector2(0, Left[i].anchorMin.y);
                    Left[i].ForceUpdateRectTransforms();
                }
            }

        }


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
            window.Components.CanvasGroup.alpha = time <= 0.001f ? 0f : 1f;
            
            
            for (int i = 0; i < Up.Length; i++)
            {
                var obj = Up[i];
                if (obj)
                {
                    var t = Open.GetCurve(upMaxes[i]).Evaluate(time);
                    var pos = Mathf.LerpUnclamped(upMaxes[i], -upMaxes[i], t);
                    obj.anchoredPosition = new Vector2(obj.anchoredPosition.x,pos);
                }
            }
            
            
            for (int i = 0; i < Right.Length; i++)
            {
                var obj = Right[i];
                if (obj)
                {
                    var t = Open.GetCurve(rightMaxes[i]).Evaluate(time);
                    var pos = Mathf.LerpUnclamped(rightMaxes[i], -rightMaxes[i], t);
                    obj.anchoredPosition = new Vector2(pos,obj.anchoredPosition.y);
                }
            }
            
            
            for (int i = 0; i < Down.Length; i++)
            {
                var obj = Down[i];
                if (obj)
                {
                    var t = Open.GetCurve(downMaxes[i]).Evaluate(time);
                    var pos = Mathf.LerpUnclamped(-downMaxes[i], downMaxes[i], t);
                    obj.anchoredPosition = new Vector2(obj.anchoredPosition.x,pos);
                }
            }
            
            
            for (int i = 0; i < Left.Length; i++)
            {
                var obj = Left[i];
                if (obj)
                {
                    var t = Open.GetCurve(leftMaxes[i]).Evaluate(time);
                    var pos = Mathf.LerpUnclamped(-leftMaxes[i], leftMaxes[i], t);
                    obj.anchoredPosition = new Vector2(pos, obj.anchoredPosition.y);
                }
            }
            
            Graphics.SetOpenTime(time,parameters);
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
            window.Components.CanvasGroup.alpha = time >= 0.99f ? 0f : 1f;
            
            
            for (int i = 0; i < Up.Length; i++)
            {
                var obj = Up[i];
                if (obj)
                {
                    var t = Close.GetCurve(upMaxes[i]).Evaluate(time);
                    var pos = Mathf.LerpUnclamped(upMaxes[i], -upMaxes[i], t);
                    obj.anchoredPosition = new Vector2(obj.anchoredPosition.x,pos);
                }
            }
            
            
            for (int i = 0; i < Right.Length; i++)
            {
                var obj = Right[i];
                if (obj)
                {
                    var t = Close.GetCurve(rightMaxes[i]).Evaluate(time);
                    var pos = Mathf.LerpUnclamped(rightMaxes[i], -rightMaxes[i], t);
                    obj.anchoredPosition = new Vector2(pos,obj.anchoredPosition.y);
                }
            }
            
            
            for (int i = 0; i < Down.Length; i++)
            {
                var obj = Down[i];
                if (obj)
                {
                    var t = Close.GetCurve(downMaxes[i]).Evaluate(time);
                    var pos = Mathf.LerpUnclamped(-downMaxes[i], downMaxes[i], t);
                    obj.anchoredPosition = new Vector2(obj.anchoredPosition.x,pos);
                }
            }
            
            
            for (int i = 0; i < Left.Length; i++)
            {
                var obj = Left[i];
                if (obj)
                {
                    var t = Close.GetCurve(leftMaxes[i]).Evaluate(time);
                    var pos = Mathf.LerpUnclamped(-leftMaxes[i], leftMaxes[i], t);
                    obj.anchoredPosition = new Vector2(pos, obj.anchoredPosition.y);
                }
            }
            
            Graphics.SetOpenTime(1f - time,parameters);
        }


        [Serializable]
        public class SwitchCurve
        {
            public enum Types { Curve, ConstantBounce }
            public Types Type;
            [Range(0,1f)]
            public float Time = .6f;
            public float Value = 20f;

            public AnimationCurve CustomCurve;

            public SwitchCurve(Types type,AnimationCurve curve, float time, float value)
            {
                Type = type;
                CustomCurve = curve;
                Time = time;
                Value = value;
            }
            public AnimationCurve GetCurve(float maxVal)
            {
                if (Type == Types.Curve)
                    return CustomCurve;
                var val = (maxVal + Value) / maxVal;
                var key = new Keyframe(Time,val);
                return new AnimationCurve(new[]
                {
                    new Keyframe(0, 0,4.57445812f,4.57445812f), 
                    key, 
                    new Keyframe(1, 1,0f,0f)
                });
            }
        }
    }
}