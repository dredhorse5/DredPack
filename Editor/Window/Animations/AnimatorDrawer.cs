using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor.Animations
{
    [CustomPropertyDrawer(typeof(DredPack.UI.WindowAnimations.Animator))]
    public class AnimatorDrawer : BaseAnimationDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);


            var animatorRect = new Rect(position.x, position.y + 20, position.width, position.height);
            EditorGUI.PropertyField(animatorRect, property.FindPropertyRelative("animator"), true);

            var labelRect = new Rect(animatorRect.x, animatorRect.y + 20, animatorRect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelRect,"Animation Names", EditorStyles.boldLabel);

            var openRect = new Rect(labelRect.x, labelRect.y + 20, labelRect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(openRect, property.FindPropertyRelative("OpenAnimationName"));

            var closeRect = new Rect(openRect.x, openRect.y + 20, openRect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(closeRect, property.FindPropertyRelative("CloseAnimationName"));

            
            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 20 * 5;
        
    }
}