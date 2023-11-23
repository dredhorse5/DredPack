﻿using System.Reflection;
using DredPack.UI.WindowAnimations;
using NaughtyAttributes.Test;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor.Animations
{
    [CustomPropertyDrawer(typeof(WindowAnimation))]
    public class BaseAnimationDrawer : PropertyDrawer
    {
        
        protected void DrawDefaultFields(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = property.FindPropertyRelative("Speed");
            if(attr != null)
                EditorGUILayout.PropertyField(attr);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.Space(-23);
            EditorGUI.indentLevel--;
            property.isExpanded = true;
            EditorGUILayout.PropertyField(property,true);
            EditorGUI.indentLevel++;
        }

    }
}