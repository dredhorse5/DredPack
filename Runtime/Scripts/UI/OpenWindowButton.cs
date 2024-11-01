using System;
using DredPack.UI;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace DredPack.Runtime.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class OpenWindowButton : MonoBehaviour
    {
        public enum FindTypes{General, Local}

        public FindTypes FindType;
        [ShowIf(nameof(FindType), FindTypes.General)]
        public string WindowID;
        [ShowIf(nameof(FindType), FindTypes.Local)]
        public Window Window;

        private void Start()
        {
            if(FindType == FindTypes.General)
            {
                if (string.IsNullOrEmpty(WindowID))
                {
                    Debug.LogError($"WindowID is null! Button name: {name}");
                    return;
                }

                if (!WindowsManager.TryGetWindow(WindowID, out Window))
                    Debug.LogError($"Cant find window with id: {WindowID}");
            }
            GetComponent<Button>().onClick.AddListener(OpenWindow);
        }

        public void OpenWindow()
        {
            if(Window)
                Window.Open();
        }
    }
}