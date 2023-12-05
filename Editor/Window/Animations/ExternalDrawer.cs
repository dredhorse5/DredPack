using DredPack.UI.WindowAnimations;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor.Animations
{
    [CustomPropertyDrawer(typeof(External))]
    public class ExternalDrawer : WindowAnimationDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            var rect = new Rect(position.x, position.y + 20, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("Component"));
            
            EditorGUI.EndProperty();

        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 20 * 2;
        }

    }
}