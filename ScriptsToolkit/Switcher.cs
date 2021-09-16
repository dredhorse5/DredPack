using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Switcher : MonoBehaviour
{
    public enum SwitcherStates
    {
        On,
        Off
    }

    [Header("Button State")]
    public SwitcherStates SwitcherState = SwitcherStates.On;

    [Header("Settings")]
    public bool WriteStateToMemory = false;
    [Button("SetRandomID")]
    public bool btntosetid;
    public void SetRandomID()
    {
        ID = System.DateTime.Now.Ticks.ToString();
    }
    public string ID;
    public float AppearingSpeed;
    public RectTransform CircleToSwitch;

    [Header("Colors")]
    public Color BackgroundColorWhenOn = Color.green;
    public Color BackgroundColorWhenOff = Color.white;

    public Color CircleColorWhenOn = Color.white;
    public Color CircleColorWhenOff = Color.black;

    [Header("Events")]
    public UnityEvent OnEvent;
    public UnityEvent OffEvent;

    [Header("Debug")]
    public bool DebugMode;
    [Button("ResetStateInMemory")]
    public bool deleteKeyInMemory;


    protected Button _button;
    protected Vector2 circleToSwitchPos;
    protected Image _image;
    protected Image circleImage;

    protected Coroutine switchToOnCor;
    protected Coroutine switchToOffCor;


    private void Start()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        circleImage = CircleToSwitch.GetComponent<Image>();
        _button.onClick.AddListener(OnSwitch);
        circleToSwitchPos = CircleToSwitch.anchoredPosition;

        //reading state in memory if we use it
        if (WriteStateToMemory)
        {
            if (ID == "")
            {
                Debug.LogError(" Warning! " + gameObject.name + "'s  ID is null.");
                return;
            }
            if (PlayerPrefs.HasKey(ID))
            {
                // if key is exists, read state
                if (PlayerPrefs.GetInt(ID) > 0)
                {
                    SetSwitchToOn();
                }
                else
                {
                    SetSwitchToOff();
                }
            }
            else
            {
                if (DebugMode)
                    Debug.LogError("Key isn't exists");
            }
        }
    }


    #region memoryControl

    public virtual void ResetStateInMemory()
    {
        Debug.LogError("Key '" + ID + "' on the object '" + gameObject.name + "' was deleted");
        PlayerPrefs.DeleteKey(ID);
    }
    public virtual void SetStateInMemoty(bool state)
    {
        if (!WriteStateToMemory || ID == "") return;
        PlayerPrefs.SetInt(ID, state ? 1 : -1);
        PlayerPrefs.Save();
    }

    #endregion


    #region Switching

    public virtual void OnSwitch()
    {
        switch (SwitcherState)
        {
            case SwitcherStates.On:
                SwitchToOff();
                break;

            case SwitcherStates.Off:
                SwitchToOn();
                break;
        }
    }


    public virtual void SetSwitchToOn()
    {
        if (DebugMode)
            Debug.LogError("switching to on");

        SwitcherState = SwitcherStates.On;
        OnEvent?.Invoke();

        _image.color = BackgroundColorWhenOn;
        circleImage.color = CircleColorWhenOn;
        CircleToSwitch.anchoredPosition = circleToSwitchPos;
        SetStateInMemoty(true);
    }
    public virtual void SetSwitchToOff()
    {
        if (DebugMode)
            Debug.LogError("switching to off");

        SwitcherState = SwitcherStates.Off;
        OffEvent?.Invoke();

        _image.color = BackgroundColorWhenOff;
        circleImage.color = CircleColorWhenOff;
        CircleToSwitch.anchoredPosition = -circleToSwitchPos;
        SetStateInMemoty(false);
    }

    public virtual void SwitchToOn()
    {
        if (DebugMode)
            Debug.LogError("switching to on");

        if (switchToOffCor != null)
            StopCoroutine(switchToOffCor);

        SwitcherState = SwitcherStates.On;

        switchToOnCor = StartCoroutine(SwitchToOn_Ienumerator());
        OnEvent?.Invoke();

        SetStateInMemoty(true);
    }
    public virtual void SwitchToOff()
    {
        if (DebugMode)
            Debug.LogError("switching to off");

        if (switchToOnCor != null)
            StopCoroutine(switchToOnCor);

        SwitcherState = SwitcherStates.Off;

        switchToOnCor = StartCoroutine(SwitchToOff_Ienumerator());
        OffEvent?.Invoke();

        SetStateInMemoty(false);
    }

    protected IEnumerator SwitchToOn_Ienumerator()
    {
        for (float i = 0f; i < 1; i += Time.deltaTime * AppearingSpeed)
        {
            CircleToSwitch.anchoredPosition = Vector2.Lerp(-circleToSwitchPos, circleToSwitchPos, EasingFunctions.SmoothSquared(i));

            _image.color = Color.Lerp(BackgroundColorWhenOff, BackgroundColorWhenOn, i);
            circleImage.color = Color.Lerp(CircleColorWhenOff, CircleColorWhenOn, i);

            yield return null;
        }
        CircleToSwitch.anchoredPosition = circleToSwitchPos;
    }
    protected IEnumerator SwitchToOff_Ienumerator()
    {
        for (float i = 0f; i < 1; i += Time.deltaTime * AppearingSpeed)
        {
            CircleToSwitch.anchoredPosition = Vector2.Lerp(circleToSwitchPos, -circleToSwitchPos, EasingFunctions.SmoothSquared(i));

            _image.color = Color.Lerp(BackgroundColorWhenOn, BackgroundColorWhenOff, i);
            circleImage.color = Color.Lerp(CircleColorWhenOn, CircleColorWhenOff, i);

            yield return null;
        }
        CircleToSwitch.anchoredPosition = -circleToSwitchPos;
    }

    #endregion
}
