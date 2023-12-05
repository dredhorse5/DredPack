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
        private bool hasSpeedParameter;
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
            foreach (var par in animator.parameters)
            {
                if (par.name == "Speed")
                {
                    hasSpeedParameter = true;
                    break;
                }
            }
        }

        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            yield return StartCoroutine(base.UpdateOpen(parameters));
            animator.Play(OpenAnimationName);
            if(hasSpeedParameter)
                animator.SetFloat("Speed",Speed);
            yield return new WaitForSeconds(openClip.length);
        }
        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            yield return StartCoroutine(base.UpdateClose(parameters));
            animator.Play(CloseAnimationName);
            if(hasSpeedParameter)
                animator.SetFloat("Speed",Speed);
            yield return new WaitForSeconds(closeClip.length);
        }
    }
}