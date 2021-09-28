﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnly : PropertyDrawer
{
	// Necessary since some properties tend to collapse smaller than their content
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}

	// Draw a disabled property field
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		GUI.enabled = false; // Disable fields
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = true; // Enable fields
	}
}
#endif