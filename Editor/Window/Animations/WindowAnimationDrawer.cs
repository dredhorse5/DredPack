using System.Reflection;
using DredPack.UI.WindowAnimations;
using NaughtyAttributes.Test;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor.Animations
{
    [CustomPropertyDrawer(typeof(WindowAnimation))]
    public class WindowAnimationDrawer : PropertyDrawer
    {
        
        protected void DrawDefaultFields(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Speed"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("SwitchDelay"));
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.Space(-23);
            EditorGUI.indentLevel--;
            property.isExpanded = true;
            EditorGUILayout.PropertyField(property,GUIContent.none,true);
            EditorGUI.indentLevel++;
        }

    }
}