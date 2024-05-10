using DredPack.UI.Some;
using DredPack.UI.Tabs;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor
{
    //[CustomPropertyDrawer(typeof(WindowTab))]
    public class DefTab : PropertyDrawer
    {
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.Next(true); // get first child of your property
            do
            {
                EditorGUILayout.PropertyField(property, false); // Include children
            }
            while (property.Next(false)); // get property's next sibling
        }
 
        /*public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            property.Next(true);
            float result = 0;
            do
            {
                result += EditorGUI.GetPropertyHeight(property, false) + 2; // include children
            }
            while (property.Next(false));

            return result;
        }*/
    }
}