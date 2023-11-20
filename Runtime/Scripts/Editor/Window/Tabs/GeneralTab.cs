using System.Linq;
using DredPack.UI;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor
{
    public class GeneralTab : Tab
    {
        public GeneralTab(WindowEditor window, string tabName) : base(window, tabName) { }

        public override void Draw()
        {
            base.Draw();
            window.DrawLabel(" States");
            EditorGUILayout.LabelField("Current",
                tabProperty.FindPropertyRelative("CurrentState").enumDisplayNames.First());
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("StateOnAwakeMethod"),
                new GUIContent("Awake Method"), true);
            if (window.T.General.StateOnAwakeMethod != WindowClasses.StatesAwakeMethod.Nothing)
            {
                string propName = "OnAwake";
                switch (window.T.General.StateOnAwakeMethod)
                {
                    case WindowClasses.StatesAwakeMethod.Awake:
                        propName = "On Awake";
                        break;
                    case WindowClasses.StatesAwakeMethod.Start:
                        propName = "On Start";
                        break;
                    case WindowClasses.StatesAwakeMethod.OnEnable:
                        propName = "OnEnable";
                        break;
                }

                EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("StateOnAwake"),
                    new GUIContent("Set State " + propName), true);
            }


            if (GUILayout.Button("Switch State (alt + shift + Q)"))
                Debug.Log("Switch state!"); //window.T.SwitchState();
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Switch with animation"))
                Debug.Log("Switch with animation!");
            GUI.enabled = true;



            window.DrawLabel(" Buttons");
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("CloseButton"), true);
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("OpenButton"), true);
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("SwitchButton"), true);


            window.DrawLabel(" Some");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("Disableable"), true);
            if (window.T.General.Disableable)
                EditorGUILayout.PropertyField(componentsProperty.FindPropertyRelative("DisableableObject"),
                    GUIContent.none);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("EnableableCanvas"),
                new GUIContent("Enableable Canvas"));
            if (window.T.General.EnableableCanvas)
                EditorGUILayout.PropertyField(componentsProperty.FindPropertyRelative("Canvas"), GUIContent.none);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("EnableableRaycaster"),
                new GUIContent("Enableable Raycaster"));
            if (window.T.General.EnableableRaycaster)
                EditorGUILayout.PropertyField(componentsProperty.FindPropertyRelative("Raycaster"), GUIContent.none);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("CloseIfAnyWindowOpen"), true);
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("CloseOnOutsideClick"), true);
        }
    }
}