using System;
using DredPack.UI.Some;
using UnityEngine.Events;

namespace DredPack.UI.Tabs
{
    
    [Serializable]
    public class EventsTab : WindowTab, IWindowCallback
    {
        public override int InspectorDrawSort => 100;
        public UnityEvent StartOpen;
        public UnityEvent StartClose;
        public UnityEvent<bool> StartSwitch;

        public UnityEvent EndOpen;
        public UnityEvent EndClose;
        public UnityEvent<bool> EndSwitch;

        public UnityEvent<StatesRead> StateChanged;

        public static UnityEvent<Window, bool> StartSwitchStatic = new UnityEvent<Window, bool>();
        public static UnityEvent<Window, bool> EndSwitchStatic = new UnityEvent<Window, bool>();

        public void OnStartOpen() => StartOpen?.Invoke();
        public void OnStartClose() => StartClose?.Invoke();
        public void OnStartSwitch(bool state) => StartSwitch?.Invoke(true);

        public void OnEndOpen() => EndOpen?.Invoke();
        public void OnEndClose() => EndClose?.Invoke();
        public void OnEndSwitch(bool state) => EndSwitch?.Invoke(state);
        public void OnStateChanged(StatesRead state) => StateChanged?.Invoke(state);
    }
}