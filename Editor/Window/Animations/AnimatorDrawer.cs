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

            var speedRect = new Rect(position.x, position.y + 20, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(speedRect, property.FindPropertyRelative("Speed"), new GUIContent("Speed Multiplier"));
            
            var delaysRect = new Rect(speedRect.x, speedRect.y + 20, speedRect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(delaysRect, property.FindPropertyRelative("SwitchDelay"));

            var animatorRect = new Rect(delaysRect.x, delaysRect.y + 30, delaysRect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(animatorRect, property.FindPropertyRelative("animator"), true);

            var labelRect = new Rect(animatorRect.x, animatorRect.y + 20, animatorRect.width, EditorGUIUtility.singleLineHeight);
            var namesRect = EditorGUI.PrefixLabel(labelRect, new GUIContent("Animation Names"));
            var openLabelRect = new Rect(namesRect.x - 14, namesRect.y, namesRect.width / 2f + 7f, namesRect.height);
            var closeLabelRect = new Rect(namesRect.x + namesRect.width / 2f - 14, namesRect.y, namesRect.width / 2f + 14f, namesRect.height);
            EditorGUI.LabelField(openLabelRect, "Open");
            EditorGUI.LabelField(closeLabelRect, "Close");


            var openNameProp = property.FindPropertyRelative("OpenAnimationName");
            var closeNameProp = property.FindPropertyRelative("CloseAnimationName");
            var openStringRect = new Rect(openLabelRect.x + 40, openLabelRect.y, openLabelRect.width - 40, openLabelRect.height);
            var closeStringRect = new Rect(closeLabelRect.x + 40, closeLabelRect.y, closeLabelRect.width - 40, closeLabelRect.height);
            openNameProp.stringValue = EditorGUI.TextField(openStringRect, openNameProp.stringValue);
            closeNameProp.stringValue = EditorGUI.TextField(closeStringRect, closeNameProp.stringValue);

            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 20 * 5 + 10;
        
    }
}