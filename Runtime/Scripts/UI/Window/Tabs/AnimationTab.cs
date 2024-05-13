using System;
using System.Collections.Generic;
using System.Linq;
using DredPack.UI.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace DredPack.UI.Tabs
{
    [Serializable]
    public class AnimationTab : WindowTab
    {
        public override int InspectorDrawSort => 200;

        public bool DualMode = false;

        public WindowAnimationModule LastPlayedAnimation;

        #region Open

        
        //Single mode
        public string CurrentOpenAnimationName
        {
            get
            {
                if (string.IsNullOrEmpty(_currentOpenAnimationName))
                    _currentOpenAnimationName = "Fade";
                return _currentOpenAnimationName;
            }
            set => _currentOpenAnimationName = value;
        }
        [SerializeField] private string _currentOpenAnimationName;
        public WindowAnimationModule CurrentOpenAnimation
        {
            get
            {
                _currentOpenAnimation = GetOpenAnimation(CurrentOpenAnimationName);
                return _currentOpenAnimation;
            }
        }
        [SerializeReference][SerializeField] public WindowAnimationModule _currentOpenAnimation;
        
        [SerializeReference][SerializeField] private List<WindowAnimationModule> createdOpenAnimations = new List<WindowAnimationModule>();

        public WindowAnimationModule GetOpenAnimation(string name)
        {
            if (createdOpenAnimations == null)
                createdOpenAnimations = new List<WindowAnimationModule>();
            var value = createdOpenAnimations.Find(_ => _.Name == name);
            if (value != null)
                return value;
            else
            {
                var anim = RegisteredAnimations.Find(_ => _.Name == name);
                if (anim != null)
                {
                    createdOpenAnimations.Add((WindowAnimationModule)Activator.CreateInstance(anim.GetType()));
                    return createdOpenAnimations.Last();
                }
                else
                {
                    Debug.LogError($"Cant find animation with name: <{name}>. Will be used Fade animation");
                    if (name == "Fade")
                        return null;
                    return GetOpenAnimation("Fade");
                }
            }
        }

        #endregion

        
        #region Close
        
        //Dual Mode
        public string CurrentCloseAnimationName
        {
            get
            {
                if (string.IsNullOrEmpty(_currentCloseAnimationName))
                    _currentCloseAnimationName = "Fade";
                return _currentCloseAnimationName;
            }
            set => _currentCloseAnimationName = value;
        }
        [SerializeField] private string _currentCloseAnimationName;
        public WindowAnimationModule CurrentCloseAnimation
        {
            get
            {
                _currentCloseAnimation = GetCloseAnimation(CurrentCloseAnimationName);
                return _currentCloseAnimation;
            }
        }
        [SerializeReference][SerializeField] public WindowAnimationModule _currentCloseAnimation;

        [SerializeReference][SerializeField] private List<WindowAnimationModule> createdCloseAnimations = new List<WindowAnimationModule>();

        public WindowAnimationModule GetCloseAnimation(string name)
        {
            if (createdCloseAnimations == null)
                createdCloseAnimations = new List<WindowAnimationModule>();
            var value = createdCloseAnimations.Find(_ => _.Name == name);
            if (value != null)
                return value;
            else
            {
                var anim = RegisteredAnimations.Find(_ => _.Name == name);
                if (anim != null)
                {
                    createdCloseAnimations.Add((WindowAnimationModule)Activator.CreateInstance(anim.GetType()));
                    return createdCloseAnimations.Last();
                }
                else
                {
                    Debug.LogError($"Cant find animation with name: <{name}>. Will be used Fade animation");
                    if (name == "Fade")
                        return null;
                    return GetCloseAnimation("Fade");
                }
            }
        }

        #endregion

        
        
        #region Static Members

        

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
        
        
        #endregion
    }
}