using System.Linq;
using DredPack.UI;
using UnityEditor;
using UnityEngine;

namespace DredPack.WindowEditor
{
    public class GeneralTab : Tab
    {
        private SerializedProperty currentStateProperty;
        private SerializedProperty stateOnAwakeMethodProperty;
        private SerializedProperty stateOnAwakeProperty;
        private SerializedProperty animationOnAwakeProperty;
        private SerializedProperty closeButtonProperty;
        private SerializedProperty openButtonProperty;
        private SerializedProperty switchButtonProperty;
        private SerializedProperty disableableProperty;
        private SerializedProperty disableableObjectProperty;
        private SerializedProperty enableableCanvasProperty;
        private SerializedProperty canvasProperty;
        private SerializedProperty enableableRaycasterProperty;
        private SerializedProperty enableableCanvasGroupInteractable;
        private SerializedProperty enableableCanvasGroupRaycasts;
        private SerializedProperty raycasterProperty;
        private SerializedProperty closeIfAnyWindowOpenProperty;
        private SerializedProperty closeIfAnyWindowOpenTypeProperty;
        private SerializedProperty closeOnOutsideClickProperty; 
        private SerializedProperty autoCloseProperty; 
        private SerializedProperty autoCloseDelayProperty; 

        public GeneralTab(WindowEditor window, string tabName) : base(window, tabName)
        {
            currentStateProperty = tabProperty.FindPropertyRelative("CurrentState");
            stateOnAwakeMethodProperty = tabProperty.FindPropertyRelative("StateOnAwakeMethod");
            stateOnAwakeProperty = tabProperty.FindPropertyRelative("StateOnAwake");
            animationOnAwakeProperty = tabProperty.FindPropertyRelative("AnimationOnAwake");
            closeButtonProperty = tabProperty.FindPropertyRelative("CloseButton");
            openButtonProperty = tabProperty.FindPropertyRelative("OpenButton");
            switchButtonProperty = tabProperty.FindPropertyRelative("SwitchButton");
            disableableProperty = tabProperty.FindPropertyRelative("Disableable");
            disableableObjectProperty = componentsProperty.FindPropertyRelative("DisableableObject");
            enableableCanvasProperty = tabProperty.FindPropertyRelative("EnableableCanvas");
            canvasProperty = componentsProperty.FindPropertyRelative("Canvas");
            enableableRaycasterProperty = tabProperty.FindPropertyRelative("EnableableRaycaster");
            enableableCanvasGroupInteractable = tabProperty.FindPropertyRelative("EnableableCanvasGroupInteractable");
            enableableCanvasGroupRaycasts = tabProperty.FindPropertyRelative("EnableableCanvasGroupRaycasts");
            raycasterProperty = componentsProperty.FindPropertyRelative("Raycaster");
            closeIfAnyWindowOpenProperty = tabProperty.FindPropertyRelative("CloseIfAnyWindowOpen");
            closeIfAnyWindowOpenTypeProperty = tabProperty.FindPropertyRelative("CloseIfAnyWindowOpenType");
            closeOnOutsideClickProperty = tabProperty.FindPropertyRelative("CloseOnOutsideClick");
            autoCloseProperty = tabProperty.FindPropertyRelative("AutoClose");
            autoCloseDelayProperty = tabProperty.FindPropertyRelative("AutoCloseDelay");
        }

        public override void Draw()
        {
            base.Draw();

            window.DrawLabel(" States");

            EditorGUILayout.LabelField("Current", currentStateProperty.enumDisplayNames[currentStateProperty.enumValueIndex]);

            EditorGUILayout.PropertyField(stateOnAwakeMethodProperty, new GUIContent("Awake Method"), true);

            if (window.T.General.StateOnAwakeMethod != WindowClasses.StatesAwakeMethod.Nothing)
            {
                EditorGUI.indentLevel++;
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
                EditorGUILayout.PropertyField(stateOnAwakeProperty, new GUIContent("Set State " + propName), true);

                var nowVal = window.T.Animation.allAnimationNames.ToList().IndexOf(animationOnAwakeProperty.stringValue);
                var nextVal = EditorGUILayout.Popup("Animation", nowVal, window.T.Animation.allAnimationNames);

                if (nowVal != nextVal)
                {
                    animationOnAwakeProperty.stringValue = window.T.Animation.allAnimationNames[nextVal];
                    EditorUtility.SetDirty(window.T);
                }
                EditorGUI.indentLevel--;
            }
            if (GUILayout.Button("Switch State (alt + shift + Q)"))
                window.T.Switch("Instantly");
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Switch with animation"))
                window.T.Switch();
            GUI.enabled = true;


            window.DrawLabel(" Buttons");
            EditorGUILayout.PropertyField(closeButtonProperty, true);
            EditorGUILayout.PropertyField(openButtonProperty, true);
            EditorGUILayout.PropertyField(switchButtonProperty, true);

            
            window.DrawLabel(" Some");
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(autoCloseProperty);
            if (window.T.General.AutoClose)
            {
                EditorGUILayout.LabelField("After Open Delay", GUILayout.MaxWidth(120));
                EditorGUILayout.PropertyField(autoCloseDelayProperty, GUIContent.none);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(disableableProperty, true);
            if (window.T.General.Disableable) EditorGUILayout.PropertyField(disableableObjectProperty, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(enableableCanvasProperty, new GUIContent("Enableable Canvas"));
            if (window.T.General.EnableableCanvas) EditorGUILayout.PropertyField(canvasProperty, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(enableableRaycasterProperty, new GUIContent("Enableable Raycaster"));
            if (window.T.General.EnableableRaycaster) EditorGUILayout.PropertyField(raycasterProperty, GUIContent.none);
            EditorGUILayout.EndHorizontal();
            
            
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("CanvasGroup Control:");
            var rect = GUILayoutUtility.GetLastRect();
            var dataRect = new Rect(rect.x + 150, rect.y, rect.width - 150, rect.height);
            var leftRect = new Rect(dataRect.x - 10, dataRect.y, 100, dataRect.height);
            var rightRect = new Rect(dataRect.x + 90, dataRect.y, 100, dataRect.height);
            GUI.Box(dataRect,GUIContent.none,GUI.skin.box);
            GUI.Box(dataRect,GUIContent.none,GUI.skin.box);
            EditorGUI.LabelField(leftRect,"Interactable", EditorStyles.boldLabel);
            EditorGUI.LabelField(rightRect,"Raycasts", EditorStyles.boldLabel);
            var toggle1Rect = new Rect(leftRect.x + 76, leftRect.y, leftRect.height, leftRect.height);
            var toggle2Rect = new Rect(rightRect.x + 60, rightRect.y, rightRect.height, rightRect.height);
            EditorGUI.PropertyField(toggle1Rect, enableableCanvasGroupInteractable, GUIContent.none);
            EditorGUI.PropertyField(toggle2Rect, enableableCanvasGroupRaycasts, GUIContent.none);
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(closeIfAnyWindowOpenProperty, new GUIContent("Close If Any Window Open"), true);
            if (window.T.General.CloseIfAnyWindowOpen) EditorGUILayout.PropertyField(closeIfAnyWindowOpenTypeProperty, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            bool canCloseOnOutsideClick = window.T.Components.Canvas &&
                                          window.T.Components.Canvas.renderMode == RenderMode.ScreenSpaceCamera &&
                                          window.T.Components.Canvas.worldCamera;
            GUI.enabled = canCloseOnOutsideClick;
            EditorGUILayout.PropertyField(closeOnOutsideClickProperty, true);
            if (!canCloseOnOutsideClick)
                EditorGUILayout.LabelField("Do not activate because Canvas.renderMode must be Screen Space - Camera, and Camera must be attached", EditorStyles.helpBox);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
    }
}
