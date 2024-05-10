using System;
using DredPack.UI;
using DredPack.UI.Tabs;
using UnityEditor;

namespace DredPack.WindowEditor
{
    public class Tab
    {
        protected WindowEditor window;
        protected SerializedProperty tabProperty;
        protected SerializedProperty componentsProperty;
        public virtual Type DrawerOfTab => null;
        
        public virtual void Init(WindowEditor window, SerializedProperty tabProperty)
        {
            this.window = window;
            this.tabProperty = tabProperty;
            componentsProperty = this.window.serializedObject.FindProperty("Components");
        }

        
        public virtual void Draw()
        {
        }

    }
}