using System.Collections.Generic;
using UnityEngine;

namespace DredPack.SelectableSystem
{
    public class SelectableUiObject : SelectableObject, IUiUpdater
    {
        public List<GameObject> SelectedObjects;
        public List<GameObject> UnSelectedObjects;

        public override void OnSelect(bool state)
        {
            base.OnSelect(state);
            UpdateDynamicUI();
        }


        public virtual void UpdateDynamicUI()
        {
            foreach (var obj in SelectedObjects)
                if(obj)
                    obj.gameObject.SetActive(connectedElement.isSelected);
            
            foreach (var obj in UnSelectedObjects)
                if(obj)
                    obj.gameObject.SetActive(!connectedElement.isSelected);
        }
        public virtual void UpdateStaticUI() { }
        public virtual void UpdateAllUI()
        {
            UpdateDynamicUI();
            UpdateStaticUI();
        }
    }
}