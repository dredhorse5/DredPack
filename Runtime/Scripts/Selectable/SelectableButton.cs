using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DredPack.SelectableSystem
{
    [RequireComponent(typeof(Button))]
    public class SelectableButton : SelectableUiObject
    {
        public Button Button => _button ??= GetComponent<Button>();
        private Button _button;
        
        protected virtual void Start()
        {
            Button.onClick.AddListener(Select);
        }
    }
}