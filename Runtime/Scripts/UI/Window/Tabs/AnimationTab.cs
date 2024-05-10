using System;
using System.Collections.Generic;
using System.Linq;
using DredPack.UI.Animations;
using UnityEngine;

namespace DredPack.UI.Tabs
{
    [Serializable]
    public class AnimationTab : WindowTab
    {
        public override int InspectorDrawSort => 200;


        public string CurrentAnimationName
        {
            get
            {
                if (string.IsNullOrEmpty(_currentAnimationName))
                    _currentAnimationName = "Fade";
                return _currentAnimationName;
            }
            set => _currentAnimationName = value;
        }

        private string _currentAnimationName;
        public WindowAnimationModule CurrentAnimation
        {
            get
            {
                _currentAnimation = GetAnimation(CurrentAnimationName);
                return _currentAnimation;
            }
        }

        [SerializeReference][SerializeField] public WindowAnimationModule _currentAnimation;
        [SerializeReference] private Dictionary<string, WindowAnimationModule> createdAnimations = new Dictionary<string, WindowAnimationModule>();






        [SerializeReference]
        public static List<WindowAnimationModule> RegisteredAnimations = new List<WindowAnimationModule>();
        public static List<string> RegisteredAnimationsNames = new List<string>();

        public static void RegisterAnimation(Type anim)
        {
            if (RegisteredAnimations == null)
                RegisteredAnimations = new List<WindowAnimationModule>(); 
            
            var inst = (WindowAnimationModule)Activator.CreateInstance(anim);
            RegisteredAnimations.Add(inst);
             
            RegisteredAnimations = RegisteredAnimations.OrderBy(_ => _.SortIndex).ToList();
            RegisteredAnimationsNames = (from _ in RegisteredAnimations where _.Name != "" select _.Name).ToList();
        }
        
        
        


        public WindowAnimationModule GetAnimation(string name)
        {
            if (createdAnimations == null)
                createdAnimations = new Dictionary<string, WindowAnimationModule>();
            if (createdAnimations.TryGetValue(name, out var value))
                return value;
            else
            {
                var anim = RegisteredAnimations.Find(_ => _.Name == name);
                if (anim != null)
                {
                    createdAnimations.Add(name, (WindowAnimationModule)Activator.CreateInstance(anim.GetType()));
                    return createdAnimations[name];
                }
                else
                {
                    Debug.LogError($"Cant find animation with name: <{name}>. Will be used Fade animation");
                    if (name == "Fade")
                        return null;
                    return GetAnimation("Fade");
                }
            }
        }

    }
}