using System;
using DredPack.UI;
using DredPack.UI.Some;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DredPack.WindowEditor
{
    public class AudioTab : Tab
    {
        
        GUIContent addButonContent;
        GUIContent[] eventTypes;
        GUIContent iconToolbarMinus;
        SerializedProperty eventsProperty;
        GUIContent eventIDName;
        public override Type DrawerOfTab => typeof(DredPack.UI.Tabs.AudioTab); 

        
        public override void Init(WindowEditor window, SerializedProperty tabProperty)
        {
            base.Init(window, tabProperty);
            addButonContent = EditorGUIUtility.TrTextContent("Add New Audio Type");
            
            eventsProperty = tabProperty.FindPropertyRelative("List");
            
            eventIDName = new GUIContent("");
            
            iconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
            iconToolbarMinus.tooltip = "Remove audio in this list.";
            
            string[] eventNames = Enum.GetNames(typeof(StatesForChanged));
            eventTypes = new GUIContent[eventNames.Length];
            for (int i = 0; i < eventNames.Length; ++i)
                eventTypes[i] = new GUIContent(eventNames[i]);
        }

        public override void Draw() 
        {
            base.Draw();
            
            int toBeRemovedEntry = -1;

            EditorGUILayout.Space(0);

            Vector2 removeButtonSize = GUIStyle.none.CalcSize(iconToolbarMinus);

            for (int i = 0; i < eventsProperty.arraySize; ++i)
            {
                SerializedProperty element = eventsProperty.GetArrayElementAtIndex(i);
                SerializedProperty state = element.FindPropertyRelative("State");
                SerializedProperty audio = element.FindPropertyRelative("Audio");
                eventIDName.text = state.enumDisplayNames[state.enumValueIndex];

                Rect callbackRect = GUILayoutUtility.GetLastRect();

                Rect removeButtonPos = new Rect(callbackRect.xMax - removeButtonSize.x, callbackRect.y +1, removeButtonSize.x, removeButtonSize.y);
                if (GUI.Button(removeButtonPos, iconToolbarMinus, GUIStyle.none))
                    toBeRemovedEntry = i;
                callbackRect.width -= 20; 
                EditorGUI.PropertyField(callbackRect, audio, eventIDName, true);
                //EditorGUILayout.PropertyField(audio, eventIDName);

                EditorGUILayout.Space(EditorGUI.GetPropertyHeight(audio));
                EditorGUILayout.Space(0);
            }

            if (toBeRemovedEntry > -1)
                eventsProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
            

            
            
            Rect btPosition = GUILayoutUtility.GetRect(addButonContent, GUI.skin.button);
            const float addButonWidth = 200f;
            btPosition.x = btPosition.x + (btPosition.width - addButonWidth) / 2;
            btPosition.width = addButonWidth;
            if (GUI.Button(btPosition, addButonContent))
                ShowAddTriggerMenu();
        }


        private void ShowAddTriggerMenu()
        {
            // Now create the menu, add items and show it
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < eventTypes.Length; ++i)
            {
                bool active = true;

                // Check if we already have a Entry for the current eventType, if so, disable it
                for (int p = 0; p < eventsProperty.arraySize; ++p)
                {
                    SerializedProperty delegateEntry = eventsProperty.GetArrayElementAtIndex(p);
                    SerializedProperty eventProperty = delegateEntry.FindPropertyRelative("State");
                    active = eventProperty.enumValueIndex != i;
                    if (!active) 
                        break;
                }
                if (active)
                    menu.AddItem(eventTypes[i], false, OnAddNewSelected, i);
                else
                    menu.AddDisabledItem(eventTypes[i]);
            }
            menu.ShowAsContext();
            Event.current.Use();
        }
        
        
        private void OnAddNewSelected(object index)
        {
            int selected = (int)index;

            eventsProperty.arraySize += 1;
            SerializedProperty entry = eventsProperty.GetArrayElementAtIndex(eventsProperty.arraySize - 1);
            SerializedProperty eventProperty = entry.FindPropertyRelative("State");
            eventProperty.enumValueIndex = selected;
            window.serializedObject.ApplyModifiedProperties();
        }
    }
}