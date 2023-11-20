using System;
using DredPack.DredpackEditor;
using DredPack.UI;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor
{

    [CustomEditor(typeof(UI.Window)), CanEditMultipleObjects]
    public class WindowEditor : DredInspectorEditor<UI.Window>
    {
        private Tab generalTab;
        private EventsTab eventsTab;
        private AudioTab auidoTab;
        private Tab animationTab;

        private void OnEnable()
        {
            generalTab = new Tab(this, "");
            eventsTab = new EventsTab(this, nameof(T.Events));
            auidoTab = new AudioTab(this, nameof(T.Audio));
            animationTab = new Tab(this,"");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawComponentHeader("Window - v3");
            Tabs();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            switch (UI.Window.currentWindowTab)
            {
                case 0:
                    generalTab.Draw();
                    generalTab.EndDraw();
                    break;
                case 1:
                    eventsTab.Draw();
                    eventsTab.EndDraw();
                    break;
                case 2:
                    auidoTab.Draw();
                    auidoTab.EndDraw();
                    break;
                case 3:
                    animationTab.Draw();
                    animationTab.EndDraw();
                    break;
            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        private void Reset()
        {

        }

        private void Tabs()
        {
            GUIContent[] toolbarTabs = new GUIContent[4];
            toolbarTabs[0] = new GUIContent("General");
            toolbarTabs[1] = new GUIContent("Events");
            toolbarTabs[2] = new GUIContent("Audio");
            toolbarTabs[3] = new GUIContent("Animation");
            UI.Window.currentWindowTab = GUILayout.Toolbar(UI.Window.currentWindowTab, toolbarTabs);
        }

    }
}