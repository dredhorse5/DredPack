using System;
using DredPack.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DredPack.WindowEditor
{
    public class EventsTab : Tab
    {
        public EventsTab(WindowEditor window, string tabName) : base(window, tabName) { }
        
        
        public override void Draw()
        {
            base.Draw();
            window.DrawLabel(" Start");
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("StartOpen"), true);
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("StartClose"), true);
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("StartSwitch"), true);
            window.DrawLabel(" End");
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("EndOpen"), true);
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("EndClose"), true);
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("EndSwitch"), true);
            window.DrawLabel(" State Changed");
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("StateChanged"), true);

        }

    }
}