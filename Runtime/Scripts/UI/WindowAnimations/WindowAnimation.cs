using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using DredPack.DredpackEditor;
using DredPack.WindowEditor;
using UnityEditor;
using UnityEditor.UIElements;
#endif

namespace DredPack.UI.WindowAnimations
{
    [Serializable]
    public class WindowAnimation
    {
        public virtual string Name => this.GetType().Name;
        public float Speed = 1f;
        protected Window window;
        private List<Coroutine> launchedCoroutines = new List<Coroutine>();

        public virtual void Init(Window owner)
        {
            window = owner;
        }

        public virtual IEnumerator UpdateOpen()
        {
            yield return null;
        }
        
        public virtual IEnumerator UpdateClose()
        {
            yield return null;
        }

        public void StopAllCoroutines()
        {
            foreach (var cor in launchedCoroutines)
            {
                if(cor != null)
                    window.StopCoroutine(cor);
            }
            launchedCoroutines.Clear();
        }

        protected void StartCoroutine(IEnumerator coroutine)
        {
            if (launchedCoroutines == null || launchedCoroutines.Count == 0)
                launchedCoroutines = new List<Coroutine>();

            var cor = window.StartCoroutine(coroutine);
            launchedCoroutines.Add(cor);
        }
        
        #region EDITOR
#if UNITY_EDITOR
        public virtual bool useCustomInspector => false;

        public void DrawInspector(SerializedObject obj,SerializedProperty animationProperty)
        {
            if(useCustomInspector) DrawCustomInspector(obj,animationProperty);
            else DrawDefaultInspector(obj,animationProperty);
        }

        private void DrawDefaultInspector(SerializedObject obj,SerializedProperty animationProperty) => EditorGUILayout.PropertyField(animationProperty);
        public virtual void DrawCustomInspector(SerializedObject obj,SerializedProperty animationProperty)
        {
            DredInspectorEditorTemplates.DrawLabel(Name);
            if(animationProperty.FindPropertyRelative("Speed") != null)
                EditorGUILayout.PropertyField(animationProperty.FindPropertyRelative("Speed"));
        }
#endif
        #endregion
    }
}