using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DredPack.UI.WindowAnimations
{
    [Serializable]
    public class WindowAnimation : WindowAnimationModule
    {
        public float Speed = 1f;
        public SwitchDelay SwitchDelay;
        

        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            if (SwitchDelay.Open > 0.001f)
                yield return new WaitForSeconds(SwitchDelay.Open);
            yield return null;
        }

        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            if (SwitchDelay.Close > 0.001f)
                yield return new WaitForSeconds(SwitchDelay.Close);
            yield return null;
        }
    }

    [Serializable]
    public class AnimationParameters
    {
        public readonly float CustomSpeed = 1f;
    }

    [Serializable]
    public struct SwitchDelay
    {
        public float Open;
        public float Close;
    }
}