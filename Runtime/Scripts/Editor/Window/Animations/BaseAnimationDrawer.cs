using DredPack.UI.WindowAnimations;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor.Animations
{
    [CustomPropertyDrawer(typeof(WindowAnimation))]
    public class BaseAnimationDrawer : PropertyDrawer
    {
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = property.FindPropertyRelative("Speed");
            if(attr != null)
                EditorGUILayout.PropertyField(attr);
        }

    }
}