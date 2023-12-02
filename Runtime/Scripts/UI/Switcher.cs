using System;
using System.Collections;
using DredPack.Help;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DredPack.UI
{
    [RequireComponent(typeof(Button))]
    public sealed class Switcher : MonoBehaviour
    {
        public bool StateOnStart = true;
        public float SwitchSpeed = 5f;
        
        [Header("Graphic")] 
        public bool ReversedGraphic = false;
        
        [Header("Handler")] 
        public RectTransform Handler;
        public AnimationCurve HandlerMoveCurve = new AnimationCurve(new []{new Keyframe(0f,0f), new Keyframe(1f,1f)});

        [Header("Colors")] 
        public Color BackgroundColorOn = Color.green;
        public Color BackgroundColorOff = Color.white;

        public Color HandlerColorOn = Color.white;
        public Color HandlerColorOff = Color.black;

        [Header("Saveable")] 
        public bool Saveable = false;
        [ShowIf(nameof(Saveable))]
        public string ID;
        [Help.Button(nameof(SetRandomID))][ShowIf(nameof(Saveable))]
        public bool btn1;
        [Help.Button(nameof(ResetSave))][ShowIf(nameof(Saveable))]
        public bool btn2;

        [Header("Events")]
        public UnityEvent OnEvent;
        public UnityEvent OffEvent;
        public UnityEvent<bool> SwitchEvent;

        [Header("Debug")] [ReadOnly] public bool CurrentState;



        public Image Image => _image ??= GetComponent<Image>();
        private Image _image;
        public Image HandlerImage => _circleImage ??= Handler.GetComponent<Image>();
        private Image _circleImage;
        public Button Button => _button ??= GetComponent<Button>();
        private Button _button;

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

#if UNITY_EDITOR
        public void SetRandomID()
        {
            if(Application.isPlaying)
                return;
            ID = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
        }
#endif
        public void ResetSave()
        {
            Debug.Log("Key '" + ID + "' on the object '" + gameObject.name + "' was deleted");
            PlayerPrefs.DeleteKey(ID);
        }

        public void SetSave(bool state)
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
            CurrentState = true;
            SwitchGraphic(speedMult);
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
            CurrentState = false;
            SwitchGraphic(speedMult);
            SetSave(false);
        }

        private Coroutine switchCor;
        private void SwitchGraphic(float speed)
        {
            if (switchCor != null)
                StopCoroutine(switchCor);
            switchCor = StartCoroutine(IE());
            IEnumerator IE()
            {
                bool state = CurrentState;
                if(speed <= 0)
                {
                    SetValue(1f, state);
                    yield break;
                }

                var startVal = Handler.anchoredPosition.InverseLerp(-handlerPos, handlerPos);
                startVal = state ? startVal : 1f - startVal;
                for (float i = startVal; i < 1f; i += Time.deltaTime * speed * SwitchSpeed)
                {
                    SetValue(i, state);
                    yield return null;
                }
                SetValue(1f, state);
            }
            void SetValue(float value, bool state)
            {
                var stateVal = state ? value : 1f - value;
                stateVal = ReversedGraphic ? 1f - stateVal : stateVal;

                var curvedValue = HandlerMoveCurve.Evaluate(value);
                curvedValue = state ? curvedValue : 1f - curvedValue;
                curvedValue = ReversedGraphic ? 1f - curvedValue : curvedValue;
                

                Handler.anchoredPosition = Vector2.LerpUnclamped(-handlerPos, handlerPos, curvedValue);

                Image.color = Color.Lerp(BackgroundColorOff, BackgroundColorOn, stateVal);
                HandlerImage.color = Color.Lerp(HandlerColorOff, HandlerColorOn, stateVal);
            }
        }

        #endregion
    }
}

