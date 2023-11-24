using System;
using DredPack.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DredPack.WindowEditor
{
    public class EventsTab : Tab
    {
        private SerializedProperty startOpenProperty;
        private SerializedProperty startCloseProperty;
        private SerializedProperty startSwitchProperty;
        private SerializedProperty endOpenProperty;
        private SerializedProperty endCloseProperty;
        private SerializedProperty endSwitchProperty;
        private SerializedProperty stateChangedProperty;

        public EventsTab(WindowEditor window, string tabName) : base(window, tabName)
        {
            startOpenProperty = tabProperty.FindPropertyRelative("StartOpen");
            startCloseProperty = tabProperty.FindPropertyRelative("StartClose");
            startSwitchProperty = tabProperty.FindPropertyRelative("StartSwitch");
            endOpenProperty = tabProperty.FindPropertyRelative("EndOpen");
            endCloseProperty = tabProperty.FindPropertyRelative("EndClose");
            endSwitchProperty = tabProperty.FindPropertyRelative("EndSwitch");
            stateChangedProperty = tabProperty.FindPropertyRelative("StateChanged");
        }

        public override void Draw()
        {
            base.Draw();

            window.DrawLabel(" Start");
            EditorGUILayout.PropertyField(startOpenProperty, true);
            EditorGUILayout.PropertyField(startCloseProperty, true);
            EditorGUILayout.PropertyField(startSwitchProperty, true);
            window.DrawLabel(" End");
            EditorGUILayout.PropertyField(endOpenProperty, true);
            EditorGUILayout.PropertyField(endCloseProperty, true);
            EditorGUILayout.PropertyField(endSwitchProperty, true);
            window.DrawLabel(" State Changed");
            EditorGUILayout.PropertyField(stateChangedProperty, true);
        }
    }
}