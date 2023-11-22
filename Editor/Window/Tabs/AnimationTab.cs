using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor
{
    public class AnimationTab : Tab
    {
 
        int _selected = 0;

        public AnimationTab(WindowEditor window, string tabName) : base(window, tabName)
        {
            _selected = window.T.Animation.allAnimationNames.ToList().IndexOf(window.T.Animation.currentAnimationName); 
        }

        public override void Draw()
        {
            base.Draw();
            var sel = EditorGUILayout.Popup("Animation Type", _selected, window.T.Animation.allAnimationNames);
 
            if (EditorGUI.EndChangeCheck() && sel != _selected)
            {
                _selected = sel;
                window.T.Animation.currentAnimationName = window.T.Animation.allAnimationNames[_selected];
                EditorUtility.SetDirty(window.T);
            }
            DredpackEditor.DredInspectorEditorTemplates.DrawLabel(window.T.Animation.currentAnimationName);
            EditorGUILayout.Space(-20);
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("_currentAnimation"),true);
        }
    }
}