using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor.Animations
{
    [CustomPropertyDrawer(typeof(DredPack.UI.WindowAnimations.Animator))]
    public class AnimatorDrawer : BaseAnimationDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("animator"), new GUIContent("Animator"));
            EditorGUILayout.LabelField("Animation Names", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("OpenAnimationName"), new GUIContent("Open"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("CloseAnimationName"), new GUIContent("Close"));
        }
    }
}