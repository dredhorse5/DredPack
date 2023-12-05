using System.Collections;
using UnityEngine;

namespace DredPack.UI.WindowAnimations
{
    public class ExampleClass : WindowAnimation
    {
        public override void OnInit(Window owner)
        {
            //init
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
            //animation code
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
            //animation code
        }

    }
}