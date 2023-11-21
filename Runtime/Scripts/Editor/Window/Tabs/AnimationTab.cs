using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor
{
    public class AnimationTab : Tab
    {
 
        int         _selected   = 0;
        public AnimationTab(WindowEditor window, string tabName) : base(window, tabName) { }
        public override void Draw()
        {
            base.Draw();
            var sel = EditorGUILayout.Popup("Animation Type", _selected, window.T.Animation.allAnimationNames);
 
            if (EditorGUI.EndChangeCheck() && sel != _selected)
            {
                _selected = sel;
                window.T.Animation.currentAnimationName = window.T.Animation.allAnimationNames[_selected];
                //Debug.Log(window.T.Animation.allAnimationNames[_selected]);
            }
            EditorGUILayout.Space();
            window.T.Animation.currentAnimation.DrawInspector(window.serializedObject,tabProperty.FindPropertyRelative("_currentAnimation"));
        }
    }
}