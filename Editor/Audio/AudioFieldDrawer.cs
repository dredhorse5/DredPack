using DredPack.Audio;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DredPack.DredpackEditor.Audio
{
    [CustomPropertyDrawer(typeof(AudioField), true)]
    public class AudioFieldDrawer : PropertyDrawer
    {
        private SerializedProperty localVolumeProp;
        private SerializedProperty clipProp;
        private SerializedProperty typeProp;
        private SerializedProperty findAudioMethodProp;
        private SerializedProperty groupIDProp;
        private SerializedProperty advancedIDProp;
        
        
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            localVolumeProp = property.FindPropertyRelative("LocalVolume");
            clipProp = property.FindPropertyRelative("Clip");
            typeProp = property.FindPropertyRelative("Type");
            findAudioMethodProp = property.FindPropertyRelative("FindAudioMethod");
            groupIDProp = property.FindPropertyRelative("GroupID");
            advancedIDProp = property.FindPropertyRelative("Advanced");

            var rect1 = new Rect(position);
            rect1.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.DrawRect(rect1, new Color(0,0,0,.15f));
            var rect = new Rect(position);
            rect.height -= 2;
            EditorGUI.DrawRect(rect, new Color(0,0,0,.15f));
            
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.BeginProperty(position, label, property);

            DrawPrefix(position, property, label);
            if(property.isExpanded)
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
            
            
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, new GUIContent(""),true);
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

            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                float offset = 60;
                if (property.FindPropertyRelative("FindAudioMethod").enumValueIndex == 0)
                    offset = 40;
                return EditorGUI.GetPropertyHeight(property) - offset;//20 * 12;
            }
            return 18;
        }
    }
}