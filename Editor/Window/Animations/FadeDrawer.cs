using DredPack.UI.WindowAnimations;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor.Animations
{
    [CustomPropertyDrawer(typeof(Fade))]
    public class FadeDrawer : BaseAnimationDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Curve"));
        }
        
    }
}