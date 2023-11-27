using DredPack.UI.WindowAnimations;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor.Animations
{
    [CustomPropertyDrawer(typeof(Fade))]
    public class FadeDrawer : WindowAnimationDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawDefaultFields(position, property, label);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Curve"));
        }
        
    }
}