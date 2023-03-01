using System;
using System.Collections;
using DredPack.Help;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DredPack.UI
{
    [RequireComponent(typeof(Button))]
    public class Switcher : MonoBehaviour
    {
        public bool StateOnStart = true;
        public bool Reverse = false;
        public float SwitchSpeed = 5f;
        public RectTransform Handler;

        [Header("Colors")] 
        public Color BackgroundColorOn = Color.green;
        public Color BackgroundColorOff = Color.white;

        public Color HandlerColorOn = Color.white;
        public Color HandlerColorOff = Color.black;

        [Header("Saveable")] 
        public bool Saveable = false;
        public string ID;
        [Button("SetRandomID")] public bool btntosetid;
        public void SetRandomID() => ID = Guid.NewGuid().ToString();
        [Button(nameof(ResetSave))] public bool deleteKeyInMemory;

        [Header("Events")] public UnityEvent OnEvent;
        public UnityEvent OffEvent;
        public UnityEvent<bool> SwitchEvent;

        [Header("Debug")] [ReadOnly] public bool CurrentState;
        public bool DebugMode;



        public Image Image => _image ??= GetComponent<Image>();
        private Image _image;
        public Image HandlerImage => _circleImage ??= Handler.GetComponent<Image>();
        private Image _circleImage;
        public Button Button => _button ??= GetComponent<Button>();
        private Button _button;

        private Coroutine switchCor;
        private Vector2 handlerPos;



        private void Awake()
        {
            Button.onClick.AddListener(Switch);
            handlerPos = Handler.anchoredPosition;

            if (Saveable)
            {
                if (ID == "")
                {
                    Debug.LogError(" Warning! " + gameObject.name + "'s  ID is null.");
                    Switch(StateOnStart);
                    return;
                }

                if (PlayerPrefs.HasKey(ID))
                    Switch(PlayerPrefs.GetInt(ID) > 0);
                else
                    Switch(StateOnStart);
            }
            else
                Switch(StateOnStart);
        }



        #region Saveable

        public virtual void ResetSave()
        {
            Debug.Log("Key '" + ID + "' on the object '" + gameObject.name + "' was deleted");
            PlayerPrefs.DeleteKey(ID);
        }

        public virtual void SetSave(bool state)
        {
            if (!Saveable || ID == "") return;
            PlayerPrefs.SetInt(ID, state ? 1 : -1);
            PlayerPrefs.Save();
        }

        #endregion


        #region Switching

        public void Switch()
        {
            if (CurrentState)
                SwitchOff();
            else
                SwitchOn();
        }

        public void Switch(bool state)
        {
            if (state)
                SwitchOn();
            else
                SwitchOff();
        }


        public void SwitchWithoutNotification()
        {
            if (CurrentState)
                SwitchOffWithoutNotification();
            else
                SwitchOnWithoutNotification();
        }

        public void SwitchWithoutNotification(bool state)
        {
            if (state)
                SwitchOnWithoutNotification();
            else
                SwitchOffWithoutNotification();
        }



        public void SwitchOn(float speedMult = 1f)
        {
            OnEvent?.Invoke();
            SwitchEvent?.Invoke(true);
            SwitchOnWithoutNotification(speedMult);
        }

        public void SwitchOnWithoutNotification(float speedMult = 1f)
        {
            if (DebugMode)
                Debug.Log("switching to on");

            CurrentState = true;
            if (switchCor != null)
                StopCoroutine(switchCor);
            switchCor = StartCoroutine(SwitchOnIE(speedMult));
            SetSave(true);
        }


        public void SwitchOff(float speedMult = 1f)
        {
            OffEvent?.Invoke();
            SwitchEvent?.Invoke(false);
            SwitchOffWithoutNotification(speedMult);
        }

        public void SwitchOffWithoutNotification(float speedMult = 1f)
        {
            if (DebugMode)
                Debug.Log("switching to off");

            CurrentState = false;
            if (switchCor != null)
                StopCoroutine(switchCor);
            switchCor = StartCoroutine(SwitchOffIE(speedMult));
            SetSave(false);
        }


        private IEnumerator SwitchOnIE(float speedMult = 1f)
        {
            if (speedMult < 0)
            {
                SetVisualValue(1f);
                yield break;
            }

            for (float i = 0f; i < 1; i += Time.deltaTime * speedMult * SwitchSpeed)
            {
                SetVisualValue(i);
                yield return null;
            }

            SetVisualValue(1f);
        }

        private IEnumerator SwitchOffIE(float speedMult = 1f)
        {
            if (speedMult < 0)
            {
                SetVisualValue(0);
                yield break;
            }

            for (float i = 0f; i < 1f; i += Time.deltaTime * speedMult * SwitchSpeed)
            {
                SetVisualValue(1f - i);
                yield return null;
            }

            SetVisualValue(0);
        }

        /// <param name="value">0 - off, 1 - on</param>
        private void SetVisualValue(float value)
        {
            value = Mathf.Clamp01(value);
            value = Reverse ? 1f - value : value;
            Handler.anchoredPosition = Vector2.Lerp(-handlerPos, handlerPos, EasingFunctions.SmoothSquared(value));

            Image.color = Color.Lerp(BackgroundColorOff, BackgroundColorOn, value);
            HandlerImage.color = Color.Lerp(HandlerColorOff, HandlerColorOn, value);
        }

        #endregion
    }
}

