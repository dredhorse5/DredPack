using DredPack.UI.Animations;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor
{
    [CustomPropertyDrawer(typeof(SwitchDelay))]
    public class SwitchDelayDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var openProp = property.FindPropertyRelative("Open");
            var closeProp = property.FindPropertyRelative("Close");

            position = EditorGUI.PrefixLabel(position, label);
            var openRect = new Rect(position.x, position.y, position.width / 2f - 7f, position.height);
            var closeRect = new Rect(position.x + position.width / 2f, position.y, position.width / 2f, position.height);
            EditorGUI.LabelField(openRect, "Open");
            EditorGUI.LabelField(closeRect, "Close");
            
            var openFloatRect = new Rect(openRect.x + 40, openRect.y, openRect.width - 40, openRect.height);
            var closeFloatRect = new Rect(closeRect.x + 40, closeRect.y, closeRect.width - 40, closeRect.height);
            openProp.floatValue = EditorGUI.FloatField(openFloatRect, openProp.floatValue);
            closeProp.floatValue = EditorGUI.FloatField(closeFloatRect, closeProp.floatValue);

            EditorGUI.EndProperty();

            
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}