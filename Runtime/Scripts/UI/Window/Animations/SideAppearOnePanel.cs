using System.Collections;
using DredPack.UI.WindowAnimations.Modules;
using NaughtyAttributes;
using UnityEngine;

namespace DredPack.UI.WindowAnimations
{
    public class SideAppearOnePanel : WindowAnimation
    {
        public AnimationCurve OpenCurve = new AnimationCurve(new []{new Keyframe(0,0), new Keyframe(1f,1f)});
        public AnimationCurve CloseCurve = new AnimationCurve(new []{new Keyframe(0,0), new Keyframe(1f,1f)});
        [Space] 
        public Directions OpenDirection = Directions.Right;
        public Directions CloseDirection = Directions.Right;
        public bool CustomReferenceTransform;
        [AllowNesting,ShowIf(nameof(CustomReferenceTransform))]
        public RectTransform ReferenceTransform;
        public RectTransform Panel;
        [Space] 
        public GraphicModule Graphics;


        private Offsets currentOffsets;
        public override void Init(Window owner)
        {
            base.Init(owner);
            Graphics.Init(owner);
            
        }

        public override void OnInit(Window owner)
        {
            if (!CustomReferenceTransform)
                ReferenceTransform = Panel.parent.transform.gameObject.transform as RectTransform;
            currentOffsets = GetOffsets(OpenDirection);
            
        }

        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            yield return StartCoroutine(base.UpdateOpen(parameters));
            currentOffsets = GetOffsets(OpenDirection);
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
            Panel.offsetMax = currentOffsets.Lerp(1f-OpenCurve.Evaluate(time)).Item1;
            Panel.offsetMin = currentOffsets.Lerp(1f-OpenCurve.Evaluate(time)).Item2;
            Graphics.SetOpenTime(time,parameters);
        }

        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            yield return StartCoroutine(base.UpdateClose(parameters));
            currentOffsets = GetOffsets(CloseDirection);
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
            Panel.offsetMax = currentOffsets.Lerp(CloseCurve.Evaluate(time)).Item1;
            Panel.offsetMin = currentOffsets.Lerp(CloseCurve.Evaluate(time)).Item2;
            Graphics.SetCloseTime(time,parameters);
        }

        public Offsets GetOffsets(Directions direction)
        {
            var obj = new Offsets();
            obj.OpenedMax = new Vector2(0, 0);
            obj.OpenedMin = new Vector2(0, 0);
            
            switch (direction)
            {
                case Directions.Down:
                    obj.ClosedMax = new Vector2(0, -ReferenceTransform.sizeDelta.y);
                    obj.ClosedMin = new Vector2(0, -ReferenceTransform.sizeDelta.y);
                    break;
                case Directions.Up:
                    obj.ClosedMax = new Vector2(0, ReferenceTransform.sizeDelta.y);
                    obj.ClosedMin = new Vector2(0, ReferenceTransform.sizeDelta.y);
                    break;
                case Directions.Right:
                    obj.ClosedMax = new Vector2(ReferenceTransform.sizeDelta.x, 0);
                    obj.ClosedMin = new Vector2(ReferenceTransform.sizeDelta.x, 0);
                    break;
                case Directions.Left:
                    obj.ClosedMax = new Vector2(-ReferenceTransform.sizeDelta.x, 0);
                    obj.ClosedMin = new Vector2(-ReferenceTransform.sizeDelta.x, 0);
                    break;
            }

            return obj;
        }
        
        public struct Offsets
        {
            public Vector2 OpenedMax;
            public Vector2 OpenedMin;
            
            public Vector2 ClosedMax;
            public Vector2 ClosedMin;

            public (Vector2, Vector2) Lerp(float time) => (Vector2.Lerp(OpenedMax, ClosedMax, time), Vector2.Lerp(OpenedMin, ClosedMin, time));
        }

        public enum Directions{Left,Right,Up,Down}
    }
}