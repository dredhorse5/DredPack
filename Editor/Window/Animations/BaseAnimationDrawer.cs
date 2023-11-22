using System.Reflection;
using DredPack.UI.WindowAnimations;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor.Animations
{
    [CustomPropertyDrawer(typeof(WindowAnimation))]
    public class BaseAnimationDrawer : PropertyDrawer
    {

        protected void DrawLabel(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = property.FindPropertyRelative("Speed");
            if(attr != null)
                EditorGUILayout.PropertyField(attr);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var name = property.type.Replace("managedReference<", "");
            name = name.Replace(">", "");
            property.isExpanded = true;
            EditorGUILayout.PropertyField(property, new GUIContent(name),true);
        }

    }
}