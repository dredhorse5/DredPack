using System;
using System.Collections;

namespace DredPack.UI.WindowAnimations
{
    [Serializable]
    public class Instantly : WindowAnimation
    {
        //public override string Name => "Instantly";
        public override IEnumerator UpdateOpen() { if(false) yield return null; }
        public override IEnumerator UpdateClose() { if(false) yield return null; }
        
        
    }
}