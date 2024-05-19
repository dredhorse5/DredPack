using System;
using System.Collections.Generic;
using DredPack.UIWindow;
using UnityEngine;
using UnityEngine.Events;

namespace DredPack.SelectableSystem
{
    public class WindowSelector : Selector
    {
        public List<WindowElement> Windows;
        public float LockTime = 0f;

        private float lockTimer;
        protected virtual void Start()
        {
            AddItems<WindowElement>(Windows);
        }

        private void Update()
        {
            if (lockTimer > 0f)
                lockTimer -= Time.deltaTime;
        }

        protected override bool CanSelect(Element element) => lockTimer <= 0f && element != nowSelectedElement;

        public override void OnSelected(Element element)
        {
            lockTimer = LockTime;
        }

        [Serializable]
        public class WindowElement : Element
        {
            public Window Window;
            public EventsStruct Events;

            public override void SelectCallback(bool state)
            {
                base.SelectCallback(state);
                if(state)
                    Events.SelectedWindowEvent?.Invoke();
                else
                    Events.UnSelectedWindowEvent?.Invoke();
                Events.SelectEvent?.Invoke(state);
                
                if (Window)
                    Window.Switch(state);
            }
            public struct EventsStruct
            {
                public UnityEvent SelectedWindowEvent;
                public UnityEvent UnSelectedWindowEvent;
                public UnityEvent<bool> SelectEvent;
            }
        }
    }
}