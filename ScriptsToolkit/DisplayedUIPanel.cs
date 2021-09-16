using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public class DisplayedUIPanel : MonoBehaviour
{
    public enum PanelOpenCloseMethods
    {
        Animator,
        Instantly,
        Slowly
    }
    public enum PanelStates
    {
        Open,
        Opening,
        Close,
        Closing
    }

    [Header("Displayed Panel Settings")]

    [Tooltip("How to be showed open or close visual?")]
    public PanelOpenCloseMethods Close_OpenMethod = PanelOpenCloseMethods.Instantly;
    [Tooltip("Current state of panel. You can also set the initial state, except for the intermediate ones")]
    public PanelStates CurrentPanelState = PanelStates.Close;

    [Space(4f)]
    [Header("Slowly method")]
    [Tooltip("Panel's opening and closing speed")]
    public float ShowingSpeed = 1f;

    [Space(4f)]
    [Header("Animator Method")]
    public Animator Animator;
    public string OpenTriggerAnimatorParameter = "Open";
    public string CLoseTriggerAnimatorParameter = "Close";


    protected Coroutine OpeningCoroutine;
    protected Coroutine ClosingCoroutine;
    protected CanvasGroup _canvasGroup;

    private void Start()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        switch (CurrentPanelState)
        {
            case PanelStates.Close:
            case PanelStates.Closing:
                CloseInstantly();
                break;

            case PanelStates.Open:
            case PanelStates.Opening:
                OpenInstantly();
                break;
        }
    }



    /// <summary>
    /// Call to Open panel
    /// </summary>
    public virtual void OpenPanel()
    {
        switch (Close_OpenMethod)
        {
            case PanelOpenCloseMethods.Animator:
                OpenWithAnimator();
                break;
            case PanelOpenCloseMethods.Instantly:
                OpenInstantly();
                break;
            case PanelOpenCloseMethods.Slowly:
                OpenSlowlyPanel();
                break;
        }
    }

    /// <summary>
    /// call to close panel
    /// </summary>
    public virtual void ClosePanel()
    {
        switch (Close_OpenMethod)
        {
            case PanelOpenCloseMethods.Animator:
                CloseWithAnimator();
                break;
            case PanelOpenCloseMethods.Instantly:
                CloseInstantly();
                break;
            case PanelOpenCloseMethods.Slowly:
                CloseSlowlyPanel();
                break;
        }
    }



    #region Open or close visual methods:  Animator
    protected virtual void OpenWithAnimator()
    {
        Animator.SetTrigger(OpenTriggerAnimatorParameter);
        CurrentPanelState = PanelStates.Open;

    }

    protected virtual void CloseWithAnimator()
    {
        Animator.SetTrigger(CLoseTriggerAnimatorParameter);
        CurrentPanelState = PanelStates.Close;
    }
    #endregion



    #region Open or close visual methods:  Instantly
    protected virtual void OpenInstantly()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;

        CurrentPanelState = PanelStates.Open;

    }

    protected virtual void CloseInstantly()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;

        CurrentPanelState = PanelStates.Close;
    }
    #endregion



    #region Open or close visual methods:  Slowly

    protected virtual void OpenSlowlyPanel()
    {
        if (OpeningCoroutine != null)
            StopCoroutine(OpeningCoroutine);

        OpeningCoroutine = StartCoroutine(OpenSlowlyPanelCor());
    }

    protected virtual void CloseSlowlyPanel()
    {
        if (ClosingCoroutine != null)
            StopCoroutine(ClosingCoroutine);

        ClosingCoroutine = StartCoroutine(CloseSlowlyPanelCor());
    }

    private IEnumerator OpenSlowlyPanelCor()
    {
        CurrentPanelState = PanelStates.Opening;

        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        for (float i = 0f; i < 1; i += Time.deltaTime * ShowingSpeed)
        {
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1f, EasingFunctions.SmoothSquared(i));
            yield return null;
        }

        _canvasGroup.alpha = 1f;

        CurrentPanelState = PanelStates.Open;
        OpeningCoroutine = null;
    }

    private IEnumerator CloseSlowlyPanelCor()
    {
        CurrentPanelState = PanelStates.Closing;

        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        for (float i = 0f; i < 1f; i += Time.deltaTime * ShowingSpeed)
        {
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 0f, EasingFunctions.SmoothSquared(i));
            yield return null;
        }

        _canvasGroup.alpha = 0f;

        CurrentPanelState = PanelStates.Close;
        ClosingCoroutine = null;
    }

    #endregion


}
