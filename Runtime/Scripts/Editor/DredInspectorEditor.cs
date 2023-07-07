using UnityEditor;
using UnityEngine;

namespace DredPack.DredpackEditor
{
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

        private W _t;
        
        public static void DrawComponentHeader(string customName = null)
        {
            GUILayout.BeginVertical();
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 25;
            myStyle.fontStyle = FontStyle.Bold;
            myStyle.normal.textColor =  Color.white;
            GUILayout.Label(customName ?? typeof(W).Name, myStyle);
            GUILayout.EndVertical();
        }
    }
}