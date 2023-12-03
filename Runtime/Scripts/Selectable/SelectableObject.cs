using DredPack;
using UnityEngine;
using UnityEngine.Events;

namespace DredPack.SelectableSystem
{
    public class SelectableObject : MonoBehaviour, ISelectableCallback
    {
        public UnityEvent SelectedEvent;
        public UnityEvent UnSelectedEvent;
        public UnityEvent<bool> SelectEvent;
        
        public Selector.Element connectedElement { get; protected set; }
        

        public virtual void SetElement(Selector.Element element)
        {
            connectedElement = element;
        }

        public virtual void OnSelect(bool state)
        {
            if (state) SelectedEvent?.Invoke();
            else UnSelectedEvent?.Invoke();
            SelectEvent?.Invoke(state);
        }

        public virtual void Select()
        {
            connectedElement?.Select();
        }
    }
}