using System;
using System.Collections;
using DredPack.UI.Some;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DredPack.UI.Tabs
{

    [Serializable]
    public class GeneralTab : WindowTab, IWindowCallback
    {
        public override int InspectorDrawSort => 0;
        public StatesRead CurrentState = StatesRead.Closed;
        public StatesAwakeMethod StateOnAwakeMethod;
        public string AnimationOnAwake = "Instantly";
        public StatesAwake StateOnAwake;

        //Buttons
        public Button CloseButton;
        public Button OpenButton;
        public Button SwitchButton;

        //Some
        public bool SelectObjectOnOpen = false;
        public bool AutoClose = false;
        public float AutoCloseDelay = 2;
        public bool Disableable = false;
        public bool EnableableCanvas = false;
        public bool EnableableRaycaster = true;
        public bool EnableableCanvasGroupInteractable = true;
        public bool EnableableCanvasGroupRaycasts = true;


        public bool CloseIfAnyWindowOpen;
        public CloseIfAnyWindowOpenTypes CloseIfAnyWindowOpenType;

        public enum CloseIfAnyWindowOpenTypes
        {
            OnStart,
            OnEnd
        }

        public bool CloseOnOutsideClick;

        public bool CanCloseOnOutsideClick => (window.Components.Canvas &&
                                               window.Components.Canvas.renderMode == RenderMode.ScreenSpaceCamera &&
                                               window.Components.Canvas.worldCamera);


        public override void Init(Window owner)
        {
            base.Init(owner);
            if (CloseButton) CloseButton.onClick.AddListener(window.Close);
            if (OpenButton) OpenButton.onClick.AddListener(window.Open);
            if (SwitchButton) SwitchButton.onClick.AddListener(window.Switch);
        }

        #region Callbacks



        public void OnStartOpen()
        {
            if (Disableable && window.Components.DisableableObject)
                window.Components.DisableableObject.gameObject.SetActive(true);
            if (EnableableCanvas && window.Components.Canvas)
                window.Components.Canvas.enabled = true;
            if (SelectObjectOnOpen && window.Components.SelectableOnOpen)
                EventSystem.current.SetSelectedGameObject(window.Components.SelectableOnOpen.gameObject);
        }

        public void OnStartClose()
        {
            if (EnableableRaycaster && window.Components.Raycaster)
                window.Components.Raycaster.enabled = false;

            if (window.Components.CanvasGroup)
            {
                if (EnableableCanvasGroupInteractable)
                    window.Components.CanvasGroup.interactable = false;
                if (EnableableCanvasGroupRaycasts)
                    window.Components.CanvasGroup.blocksRaycasts = false;
            }
        }

        public void OnStartSwitch(bool state)
        {
            StopAutoClose();
        }

        public void OnEndOpen()
        {
            if (EnableableRaycaster && window.Components.Raycaster)
                window.Components.Raycaster.enabled = true;
            if (window.Components.CanvasGroup)
            {
                if (EnableableCanvasGroupInteractable)
                    window.Components.CanvasGroup.interactable = true;
                if (EnableableCanvasGroupRaycasts)
                    window.Components.CanvasGroup.blocksRaycasts = true;
            }

            RunAutoClose();
        }

        public void OnEndClose()
        {
            if (Disableable && window.Components.DisableableObject)
                window.Components.DisableableObject.gameObject.SetActive(false);
            if (EnableableCanvas && window.Components.Canvas)
                window.Components.Canvas.enabled = false;
        }

        public void OnEndSwitch(bool state)
        {
        }

        public void OnStateChanged(StatesRead state)
        {
        }



        #endregion

        public void CheckOutsideClick()
        {
            if (CloseOnOutsideClick && CurrentState == StatesRead.Opened && CanCloseOnOutsideClick)
            {
                // Проверяем, было ли касание на экране
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    CheckWindow(Input.GetTouch(0).position);
                else if (Input.GetMouseButtonUp(0))
                    CheckWindow(Input.mousePosition);

                void CheckWindow(Vector2 touchPosition)
                {
                    // Проверяем, если окно активно и касание было вне его области, закрываем окно
                    if (!RectTransformUtility.RectangleContainsScreenPoint((window.transform as RectTransform),
                            touchPosition, window.Components.CanvasCamera))
                        window.Close();
                }
            }
        }

        private Coroutine autoCloseCor;

        private void StopAutoClose()
        {
            if (AutoClose && autoCloseCor != null)
                window.StopCoroutine(autoCloseCor);
        }

        private void RunAutoClose()
        {
            if (!AutoClose)
                return;
            StopAutoClose();
            autoCloseCor = window.StartCoroutine(IE());

            IEnumerator IE()
            {
                yield return new WaitForSeconds(AutoCloseDelay);
                window.Close();
            }
        }
    }
}