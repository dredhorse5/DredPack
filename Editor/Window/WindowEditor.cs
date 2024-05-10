using System;
using System.Collections.Generic;
using System.Linq;
using DredPack.DredpackEditor;
using DredPack.UI;
using DredPack.UI.Some;
using DredPack.UI.Tabs;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DredPack.WindowEditor
{

    [CustomEditor(typeof(Window)), CanEditMultipleObjects]
    public class WindowEditor : DredInspectorEditor<Window>
    {
        private static int currentWindowTab;
        [SerializeReference] private Tab[] tabs;
        public void InitTabsDrawers()
        {
            tabs = new Tab[T.AllTabs.Count];
            var tabDrawsTypes = typeof(Tab).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Tab))).ToList();
            
            Tab[] rawTabsDraws = new Tab[tabDrawsTypes.Count];
            for (var i = 0; i < tabDrawsTypes.Count; i++)
            {
                rawTabsDraws[i] = (Tab)Activator.CreateInstance(tabDrawsTypes[i]);
                int index = -1;
                for (var j = 0; j < T.AllTabs.Count; j++)
                {
                    if (rawTabsDraws[i].DrawerOfTab == T.AllTabs[j].GetType())
                    {
                        index = j;
                        break;
                    }
                }
                if(index > -1)
                    rawTabsDraws[i].Init(this,serializedObject.FindProperty(nameof(T.AllTabs)).GetArrayElementAtIndex(index));
            }

            
            for (int i = 0; i < T.AllTabs.Count; i++)
            {
                for (int j = 0; j < rawTabsDraws.Length; j++)
                {
                    var memberInfo = T.AllTabs[i].GetType();
                    
                    if (rawTabsDraws[j].DrawerOfTab == memberInfo)
                    {
                        tabs[i] = rawTabsDraws[j];
                        break;
                    }
                }
            }
        }
        private void OnEnable()
        { 
            T.FindAllTabs();
            InitTabsDrawers();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawComponentHeader("Window - v4");
            Tabs();
            if(T.AllTabs == null || T.AllTabs.Count == 0)
                return;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            
            if(tabs[currentWindowTab] != null)
            {
                tabs[currentWindowTab].Draw();
            }
            else
            {
                var property = serializedObject.FindProperty(nameof(T.AllTabs))
                    .GetArrayElementAtIndex(currentWindowTab);
                foreach (SerializedProperty o in property)
                    EditorGUILayout.PropertyField(o, true);
            }
            
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        
        private void Tabs()
        {
            if(T.AllTabs == null || T.AllTabs.Count == 0)
                return;
            GUIContent[] toolbarTabs = new GUIContent[T.AllTabs.Count];
            for (var i = 0; i < T.AllTabs.Count; i++)
                toolbarTabs[i] = new GUIContent(T.AllTabs[i].TabName);
            
            currentWindowTab = GUILayout.Toolbar(currentWindowTab, toolbarTabs);
        }

    }
}