using System;
using System.Linq;
using DredPack.DredpackEditor;
using DredPack.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DredPack.WindowEditor
{

    [CustomEditor(typeof(Window)), CanEditMultipleObjects]
    public class WindowEditor : DredInspectorEditor<Window>
    {
        private GeneralTab generalTab;
        private EventsTab eventsTab;
        private AudioTab auidoTab;
        private AnimationTab animationTab;

        private static int currentWindowTab;
        private void OnEnable()
        {
            generalTab = new GeneralTab(this, nameof(T.General));
            eventsTab = new EventsTab(this, nameof(T.Events));
            auidoTab = new AudioTab(this, nameof(T.Audio));
            animationTab = new AnimationTab(this,nameof(T.Animation));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawComponentHeader("Window - v3");
            Tabs();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            switch (currentWindowTab)
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

        
        private void Tabs()
        {
            GUIContent[] toolbarTabs = new GUIContent[4];
            toolbarTabs[0] = new GUIContent("General");
            toolbarTabs[1] = new GUIContent("Events");
            toolbarTabs[2] = new GUIContent("Audio");
            toolbarTabs[3] = new GUIContent("Animation");
            currentWindowTab = GUILayout.Toolbar(currentWindowTab, toolbarTabs);
        }

    }
}