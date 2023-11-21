using System;
using System.Collections;
using DredPack.DredpackEditor;

#if UNITY_EDITOR
using DredPack.DredpackEditor;
using DredPack.WindowEditor;
using UnityEditor;
using UnityEditor.UIElements;
#endif

namespace DredPack.UI.WindowAnimations
{
    [Serializable]
    public class Instantly : WindowAnimation
    {
        //public override string Name => "Instantly";
        public override IEnumerator UpdateOpen() { if(false) yield return null; }
        public override IEnumerator UpdateClose() { if(false) yield return null; }
        
        
        #region EDITOR
#if UNITY_EDITOR
        public override bool useCustomInspector => true;
        public override void DrawCustomInspector(SerializedObject obj,SerializedProperty animationProperty)
        {
            DredInspectorEditorTemplates.DrawLabel(Name);
            EditorGUILayout.LabelField("Nothing to show");
        }
#endif
        #endregion
    }
}