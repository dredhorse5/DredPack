using System.Reflection;
using DredPack.UI.Animations;
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
            var lastRect = GUILayoutUtility.GetLastRect();
            lastRect.position = new Vector2(lastRect.position.x - 30, lastRect.position.y) + new Vector2(3, -21);
            lastRect.width = -lastRect.height;
            lastRect.width += 53;
            lastRect.height += 110;
            EditorGUILayout.PropertyField(property, GUIContent.none, true);
            var guiStyle = new GUIStyle();
            if (EditorGUIUtility.isProSkin)
                guiStyle.normal.textColor = new Color(56f / 256f, 56f / 256f, 56f / 256f);
            else
                guiStyle.normal.textColor = new Color(200f / 256f, 200f / 256f, 200f / 256f);
            GUI.Box(lastRect, "◆",
                guiStyle); //super mega crutch. it is needed to hide the Arrow from FoldOut. fierce shit XD
            EditorGUI.indentLevel++;
        }
    }
}