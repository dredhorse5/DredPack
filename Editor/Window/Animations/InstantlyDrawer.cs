using DredPack.UI.WindowAnimations;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor.Animations
{
    [CustomPropertyDrawer(typeof(Instantly))]
    public class InstantlyDrawer : WindowAnimationDrawer
    {
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            

            var rect = new Rect(position.x, position.y + 20, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(rect,"Nothing to show", EditorStyles.boldLabel);
            
            EditorGUI.EndProperty();
            
            //EditorGUILayout.LabelField("Nothing to show");
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Calculate the height based on the number of properties
            return 20 * 2;
        }
        
    }
}