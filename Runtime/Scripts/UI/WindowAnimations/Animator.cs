using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace DredPack.UI.WindowAnimations
{
    [Serializable]
    public class Animator : WindowAnimation
    {
        public UnityEngine.Animator animator;

        //TODO: make custom drawer
        public string OpenAnimationName = "Open";
        public string CloseAnimationName = "Close";


        private AnimationClip openClip;
        private AnimationClip closeClip;
        public override void OnInit(Window owner)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == OpenAnimationName && !openClip)
                    openClip = clip;
                else if (clip.name == CloseAnimationName && !closeClip)
                    closeClip = clip;
                

                if (openClip && closeClip)
                    break;
            }

            if (!closeClip)
                Debug.LogError(
                    $"Cant find Open animation in Animator in window: <{window.name}>, with name <{OpenAnimationName}>");
            if (!openClip)
                Debug.LogError(
                    $"Cant find Close animation in Animator in window: <{window.name}>, with name <{CloseAnimationName}>");
        }

        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            animator.Play(OpenAnimationName);
            yield return new WaitForSeconds(openClip.length);
        }
        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            animator.Play(CloseAnimationName);
            yield return new WaitForSeconds(closeClip.length);
        }
    }
}