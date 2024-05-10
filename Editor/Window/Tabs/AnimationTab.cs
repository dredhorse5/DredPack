using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor
{
    public class AnimationTab : Tab
    {
 
        int _selected = 0;
        public override Type DrawerOfTab => typeof(DredPack.UI.Tabs.AnimationTab);

       

        public override void Init(WindowEditor window, SerializedProperty tabProperty)
        {
            base.Init(window, tabProperty);
            _selected = UI.Tabs.AnimationTab.RegisteredAnimationsNames.IndexOf(window.T.Animation.CurrentAnimationName);
        }

        public override void Draw()
        {
            base.Draw();
            
            var sel = EditorGUILayout.Popup("Animation Type", _selected, UI.Tabs.AnimationTab.RegisteredAnimationsNames.ToArray());
 
            if (sel != _selected)
            {
                _selected = sel;
                window.T.Animation.CurrentAnimationName = UI.Tabs.AnimationTab.RegisteredAnimationsNames[_selected];
                EditorUtility.SetDirty(window.T);
            }
            
            DredpackEditor.DredInspectorEditorTemplates.DrawLabel(window.T.Animation.CurrentAnimationName);
            var anim = window.T.Animation.CurrentAnimation;
            EditorGUILayout.Space(-20);
            SerializedProperty findPropertyRelative = tabProperty.FindPropertyRelative("_currentAnimation");
            EditorGUILayout.PropertyField(findPropertyRelative,true);
        }
    }
}