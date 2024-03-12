using UnityEditor;
using UnityEngine;

namespace DredPack.DredpackEditor
{
    #if UNITY_EDITOR
    public class DredInspectorEditor<W> : Editor where W : MonoBehaviour
    {
        public W T
        {
            get
            {
                if (_t == null)
                    _t = (W)target;
                return _t;
            }
        }
        
        public W[] Ts
        {
            get
            {
                W[] targs = new W[targets.Length];
                for (var i = 0; i < targets.Length; i++)
                {
                    targs[i] = (W)targets[i];
                }
                return targs;
            }
        }

        private W _t;
        
        public GUIStyle LabelStyle(int fontSize = 15)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.white;
            labelStyle.fontSize = fontSize;
            return labelStyle;
        }
        
        public void DrawComponentHeader(string customName = null)
        {
            GUILayout.BeginVertical();
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 25;
            myStyle.fontStyle = FontStyle.Bold;
            myStyle.normal.textColor =  Color.white;
            GUILayout.Label(customName ?? typeof(W).Name, myStyle);
            GUILayout.EndVertical();
        }

        public void DrawLabel(string name,int fontSize = 15)
        {
            GUILayout.Label(name,LabelStyle(fontSize));
        }
    }

    public static class DredInspectorEditorTemplates
    {
        
        public static GUIStyle LabelStyle(int fontSize = 15)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.white;
            labelStyle.fontSize = fontSize;
            return labelStyle;
        }
        
        public static void DrawComponentHeader(string customName = "")
        {
            GUILayout.BeginVertical();
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 25;
            myStyle.fontStyle = FontStyle.Bold;
            myStyle.normal.textColor =  Color.white;
            GUILayout.Label(customName, myStyle);
            GUILayout.EndVertical();
        }

        public static void DrawLabel(string name,int fontSize = 15)
        {
            GUILayout.Label(name,LabelStyle(fontSize));
        }
    }
    #endif
}