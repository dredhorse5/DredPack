using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DredPack.SelectableSystem
{
    public class Selector : MonoBehaviour
    {
        public int SelectOnStart = -1;
        public UnityEvent<Element> SelectedEvent;
        public Element nowSelectedElement { get; set; }
        protected List<Element> _elements = new List<Element>();

        
        
        
        protected void AddItems<T>(List<T> elements, bool clearLastList = true) where T : Element
        {
            if(clearLastList)
                _elements = new List<Element>();
            foreach (var t in elements)
                AddItem(t);
            if(clearLastList && SelectOnStart >= 0)
                Select(SelectOnStart);
        }

        protected void AddItem<T>(T element) where T : Element
        {
            _elements.Add(element);
            element.Init(this);
        }

        protected void RemoveItem<T>(T element) where T : Element
        {
            if(element == nowSelectedElement)
                UnSelect();
            _elements.Remove(element);
        }
        
        
        

        public void Select(SelectableObject selectableButton)
        {
            if(selectableButton == null || selectableButton.connectedElement == null)
                return;
            Select(selectableButton.connectedElement);
        }
        public void Select(int index)
        {
            if(index >= _elements.Count)
                return;
            var element = _elements[index];
            if(element == null)
                return;
            Select(element);
        }
        public void Select(Element element)
        {
            if(element == null)
                return;
            if(element.selector != this)
                return;
            if(!CanSelect(element))
                return;
            
            if(nowSelectedElement != element)
                nowSelectedElement?.SelectCallback(false);
            nowSelectedElement = element;
            nowSelectedElement.SelectCallback(true);
            
            SelectedEvent?.Invoke(nowSelectedElement);
            OnSelected(nowSelectedElement);
        }

        protected virtual bool CanSelect(Element element) => true; 

        public void UnSelect()
        {
            nowSelectedElement?.SelectCallback(false);
            nowSelectedElement = null;
            OnSelected(nowSelectedElement);
        }

        public virtual void OnSelected(Element element) { }

        
        
        [Serializable]
        public class Element
        {
            public UnityEvent SelectedEvent { get; set; } = new UnityEvent();
            public UnityEvent UnSelectedEvent { get; set; } = new UnityEvent();
            public UnityEvent<bool> SelectEvent { get; set; } = new UnityEvent<bool>();
            
            public bool isSelected { get; private set; }
            public Selector selector { get; private set; }
            
            public HashSet<ISelectableCallback> Callbacks { get; protected set; }

            public virtual void Init(Selector selector)
            {
                this.selector = selector;
                foreach (var obj in Callbacks)
                    obj.SetElement(this);
            }


            public void Select()
            {
                selector.Select(this);
            }

            public virtual void SelectCallback(bool state)
            {
                foreach (var obj in Callbacks)
                    obj.OnSelect(state);
                
                
                isSelected = state;
                
                if (state) SelectedEvent?.Invoke();
                else UnSelectedEvent?.Invoke();
                SelectEvent?.Invoke(state);
            }

            public virtual bool CanSelect() => true;

            public void RegisterCallback(ISelectableCallback callback)
            {
                Callbacks.Add(callback);
                callback.SetElement(this);
            }

            public void UnRegisterCallback(ISelectableCallback callback)
            {
                Callbacks.Remove(callback);
                callback.SetElement(null);
            }
        }
    }
}