using System;
using DredPack.Audio;

namespace DredPack.UIWindow.Tabs
{
    
    [Serializable]
    public class SCPAudio : StateChangedProperty
    {
        public AudioField Audio;
        protected override void Execute() => Audio.Play();
    }
}