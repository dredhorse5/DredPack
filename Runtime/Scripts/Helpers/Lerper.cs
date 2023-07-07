using System;
using System.Collections;
using DredPack.Help;
using UnityEngine;

namespace DredPack.Help
{
    public class Lerper
    {
        public Coroutine coroutine { get; protected set; }
        public float speed;
        public AnimationCurve curve;
        public MonoBehaviour owner;
        public Action<float> feedback;
        public float defaultValue;

        public Lerper(MonoBehaviour owner, Action<float> feedback, float defaultValue, float speed = 1f)
        {
            this.owner = owner;
            this.feedback = feedback;
            if (speed <= 0f)
                speed = 1f;
            this.speed = speed;
            this.defaultValue = defaultValue;
        }

        public void Set(float value, float startValue = float.MinValue, float speed = -1f)
        {
            if (startValue != float.MinValue)
                defaultValue = startValue;
            if (speed > 0f)
                this.speed = speed;

            LerpFloat(coroutine, owner, defaultValue, value, this.speed, curve, feedback + (_ => defaultValue = _));
        }

        public static void LerpFloat(Coroutine coroutine, MonoBehaviour owner, float startVal, float endVal,
            float speed, AnimationCurve curve, Action<float> feedback, Action endFeedback = null)
        {
            if (coroutine != null)
                owner.StopCoroutine(coroutine);
            coroutine = owner.StartCoroutine(LerpFloatIE(startVal, endVal, speed, curve, feedback, endFeedback));


        }

        public static IEnumerator LerpFloatIE(float startVal, float endVal, float speed, AnimationCurve curve, Action<float> feedback,
            Action endFeedback = null)
        {
            for (float i = 0; i < 1f; i += Time.deltaTime * speed)
            {
                feedback.Invoke(Mathf.LerpUnclamped(startVal, endVal, curve.Evaluate(i)));
                yield return null;
            }

            feedback.Invoke(endVal);
            if (endFeedback != null)
                endFeedback.Invoke();
        }


        public static void LerpVector3(ref Coroutine coroutine, MonoBehaviour owner, Vector3 startVal, Vector3 endVal,
            float speed, AnimationCurve curve, Action<Vector3> feedback, Action endFeedback = null)
        {
            if (coroutine != null)
                owner.StopCoroutine(coroutine);
            coroutine = owner.StartCoroutine(LerpVector3IE(startVal, endVal, speed, curve, feedback, endFeedback));
        }

        public static IEnumerator LerpVector3IE(Vector3 startVal, Vector3 endVal, float speed, AnimationCurve curve,
            Action<Vector3> feedback, Action endFeedback = null)
        {
            for (float i = 0; i < 1f; i += Time.deltaTime * speed)
            {
                feedback?.Invoke(Vector3.LerpUnclamped(startVal, endVal, curve.Evaluate(i)));
                yield return null;
            }

            feedback?.Invoke(endVal);
            endFeedback?.Invoke();
        }


        public static IEnumerator LerpColorIE(Color startVal, Color endVal, float speed, AnimationCurve curve,
            Action<Color> feedback, Action endFeedback = null)
        {
            for (float i = 0; i < 1f; i += Time.deltaTime * speed)
            {
                feedback.Invoke(Color.LerpUnclamped(startVal, endVal, curve.Evaluate(i)));
                yield return null;
            }

            feedback.Invoke(endVal);
            if (endFeedback != null)
                endFeedback.Invoke();
        }


    }
}