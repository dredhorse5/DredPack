using System;
using System.Collections;

namespace DredPack.UI.WindowAnimations
{
    [Serializable]
    public class Animator : WindowAnimation
    {
        public UnityEngine.Animator animator;
        
        public Types SwitchType = Types.Trigger;
        public enum Types { Trigger, BoolSwap, AnimationNames }
        
        //type = = Trigger
        public string OpenTrigger;
        public string CloseTrigger;
        
        //type = = BoolSwap
        public string BoolName;
        
        //type = = AnimationNames
        public string OpenAnimationName;
        public string CloseAnimationName;
        

        
        
        public override IEnumerator UpdateOpen(AnimationParameters parameters)
        {
            yield break;
            switch (SwitchType)
            {
                default: case Types.Trigger: break;
                case Types.BoolSwap: break;
                case Types.AnimationNames: break;
            }
        }

        public override IEnumerator UpdateClose(AnimationParameters parameters)
        {
            yield break;
            switch (SwitchType)
            {
                default: case Types.Trigger: break;
                case Types.BoolSwap: break;
                case Types.AnimationNames: break;
            }
        }
    }
}