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
            
            
            var curStateProp = tabProperty.FindPropertyRelative("CurrentState");
            EditorGUILayout.LabelField("Current", curStateProp.enumDisplayNames[curStateProp.enumValueIndex]);
            
            
            EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("StateOnAwakeMethod"), new GUIContent("Awake Method"), true);
            
            
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


                var nowVal = window.T.Animation.allAnimationNames.ToList().IndexOf(window.T.General.AnimationOnAwake);
                var nextVal = EditorGUILayout.Popup("Animation", nowVal, window.T.Animation.allAnimationNames);
 
                if (EditorGUI.EndChangeCheck() && nowVal != nextVal)
                {
                    window.T.General.AnimationOnAwake = window.T.Animation.allAnimationNames[nextVal];
                    EditorUtility.SetDirty(window.T);
                }
                //EditorGUILayout.PropertyField(tabProperty.FindPropertyRelative("AnimationOnAwake"),true);
            }


            if (GUILayout.Button("Switch State (alt + shift + Q)"))
                window.T.Switch("Instantly");
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Switch with animation"))
                window.T.Switch();
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