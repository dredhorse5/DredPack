using DredPack.UI;
using UnityEditor;

namespace DredPack.WindowEditor
{
    public class Tab
    {
        protected WindowEditor window;
        protected SerializedProperty tabProperty;
        protected SerializedProperty componentsProperty;
        public Tab(WindowEditor window, string tabName)
        {
            this.window = window;
            tabProperty = this.window.serializedObject.FindProperty(tabName);
            componentsProperty = this.window.serializedObject.FindProperty("Components");
        }

        
        public virtual void Draw()
        {
            EditorGUI.indentLevel++;
        }

        public virtual void EndDraw()
        {
            EditorGUI.indentLevel--;
        }
    }
}