using DredPack.UI.WindowAnimations;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor
{
    [CustomPropertyDrawer(typeof(SideAppear.SwitchCurve))]
    public class SwitchCurveDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            
            EditorGUI.BeginProperty(position, label, property);

            var typeProp = property.FindPropertyRelative("Type");
            var timeProp = property.FindPropertyRelative("Time");
            var valueProp = property.FindPropertyRelative("Value");
            var customCurveProp = property.FindPropertyRelative("CustomCurve");

            bool isCustomCurve = typeProp.enumValueIndex == 0;

            EditorGUI.PrefixLabel(position, label);
            position = new Rect(position.x + 60, position.y, position.width - 60, position.height);
            var leftRect = new Rect(position.x - 14, position.y, position.width / (3f) + 7f, position.height);
            var rightRect = new Rect(position.x - 14 + position.width/3f, position.y, (2f * position.width / (3f)) + 7f, position.height);
            
            EditorGUI.PropertyField(leftRect, typeProp,GUIContent.none);
            
            if (isCustomCurve)
            {
                EditorGUI.PropertyField(rightRect, customCurveProp, GUIContent.none);
            }
            else
            {
                float ratioValue = .7f;
                var timeRect = new Rect(rightRect.x, rightRect.y, rightRect.width * ratioValue + 10, rightRect.height);
                var valRect = new Rect(rightRect.x + (rightRect.width * ratioValue), rightRect.y, rightRect.width * (1f - ratioValue), rightRect.height);
                EditorGUI.PropertyField(timeRect, timeProp, GUIContent.none);
                EditorGUI.PropertyField(valRect, valueProp, GUIContent.none);
            }
            
            EditorGUI.EndProperty();

            
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}