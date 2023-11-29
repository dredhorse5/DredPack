using DredPack.Audio;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DredPack.DredpackEditor.Audio
{
    [CustomPropertyDrawer(typeof(AudioField))]
    public class AudioFieldDrawer : PropertyDrawer
    {
        private SerializedProperty localVolumeProp;
        private SerializedProperty clipProp;
        private SerializedProperty typeProp;
        private SerializedProperty findAudioMethodProp;
        private SerializedProperty groupIDProp;
        private SerializedProperty advancedIDProp;
        
        private bool expanded;
        
        private float savedY;
        private float height;
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            localVolumeProp = property.FindPropertyRelative("LocalVolume");
            clipProp = property.FindPropertyRelative("Clip");
            typeProp = property.FindPropertyRelative("Type");
            findAudioMethodProp = property.FindPropertyRelative("FindAudioMethod");
            groupIDProp = property.FindPropertyRelative("GroupID");
            advancedIDProp = property.FindPropertyRelative("Advanced");

            savedY = position.y;
            EditorGUI.DrawRect(position, new Color(0,0,0,.15f));
            
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.BeginProperty(position, label, property);

            DrawPrefix(position, property, label);
            if(expanded)
                DrawExpand(position, property, label);

            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }

        private void DrawPrefix(Rect position, SerializedProperty property, GUIContent label)
        {
            var prefixRect = EditorGUI.PrefixLabel(position, label);
            
            prefixRect.width /= 2f;
            prefixRect.width -= 5;
            EditorGUI.PropertyField(prefixRect, localVolumeProp, GUIContent.none);

            prefixRect.x += prefixRect.width + 5;
            prefixRect.width += 5;
            if(findAudioMethodProp.enumValueIndex == 0)
            {
                if (clipProp.arraySize < 1)
                    clipProp.arraySize = 1;
                var audioProp = GetFirstAudioProperty(clipProp);
                EditorGUI.ObjectField(prefixRect, audioProp, new GUIContent(""));
            } 
            else
            {
                EditorGUI.PropertyField(prefixRect, groupIDProp, GUIContent.none);
            }
            
            
            expanded = EditorGUI.Foldout(position, expanded, new GUIContent(""),true);
        }

        SerializedProperty GetFirstAudioProperty(SerializedProperty array)
        {
            for (int i = 0; i < array.arraySize; i++)
            {
                var el = array.GetArrayElementAtIndex(i);
                if (el != null && el.objectReferenceValue != null)
                    return el;
            }

            return array.GetArrayElementAtIndex(0);
        }

        private void DrawExpand(Rect position, SerializedProperty property, GUIContent label)
        {
            position.y += 20;
            EditorGUI.indentLevel++;
            //TODO: draw Type from string array
            EditorGUI.PropertyField(position, typeProp);
            
            position.y += 20;
            EditorGUI.PropertyField(position, findAudioMethodProp);
            
            if(findAudioMethodProp.enumValueIndex == 0)
            {
                position.y += 20;
                EditorGUI.PropertyField(position, clipProp);
                position.y += EditorGUI.GetPropertyHeight(clipProp,true) ;
            }
            else
                position.y += 20;
            EditorGUI.PropertyField(position, advancedIDProp,true);
            position.y += EditorGUI.GetPropertyHeight(advancedIDProp,true);

            height = position.y - savedY;
            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (expanded)
                return height + 10;//20 * 12;
            return 18;
        }
    }
}