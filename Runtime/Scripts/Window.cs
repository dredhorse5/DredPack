using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public class Window : MonoBehaviour
{
    public enum PanelOpenCloseMethods
    {
        Animator,
        Instantly,
        Slowly
    }
    public enum WindowStatesRead
    {
        Opened,
        Opening,
        Closed,
        Closing
    }
    public enum WindowStatesAwake
    {
        Open,
        Close
    }

    
    
    
    [SerializeField]
    public WindowStatesAwake stateOnAwake = WindowStatesAwake.Close;
    [ReadOnly]
    [SerializeField]
    public WindowStatesRead CurrentWindowState = WindowStatesRead.Opened;
    [Space]
    [SerializeField]
    public PanelOpenCloseMethods Close_OpenMethod = PanelOpenCloseMethods.Slowly;
    
    
    [NonSerialized]
    public float ShowingSpeed = 1f;

    [NonSerialized] public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    
    [NonSerialized]
    public Animator Animator;
    [NonSerialized]
    public string OpenTriggerAnimatorParameter = "Open";
    [NonSerialized]
    public string CLoseTriggerAnimatorParameter = "Close";


    protected Coroutine openingCoroutine;
    protected Coroutine closingCoroutine;
    protected CanvasGroup m_canvasGroup
    {
        get
        {
            if (!_canvasGroup)
                _canvasGroup = GetComponent<CanvasGroup>();
            return _canvasGroup;
        }
    }
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        switch (stateOnAwake)
        {
            case WindowStatesAwake.Close:
                Close_Instantly();
                break;

            case WindowStatesAwake.Open:
                Open_Instantly();
                break;
        }
    }



    /// <summary>
    /// Call to Open this window
    /// </summary>
    public virtual void Open()
    {
        switch (Close_OpenMethod)
        {
            case PanelOpenCloseMethods.Animator:
                Open_Animator();
                break;
            case PanelOpenCloseMethods.Instantly:
                Open_Instantly();
                break;
            case PanelOpenCloseMethods.Slowly:
                OpenSlowlyPanel();
                break;
        }
    }

    /// <summary>
    /// call to close window
    /// </summary>
    public virtual void Close()
    {
        switch (Close_OpenMethod)
        {
            case PanelOpenCloseMethods.Animator:
                Close_Animator();
                break;
            case PanelOpenCloseMethods.Instantly:
                Close_Instantly();
                break;
            case PanelOpenCloseMethods.Slowly:
                CloseSlowlyPanel();
                break;
        }
    }



    #region Open or close visual methods:  Animator
    protected virtual void Open_Animator()
    {
        Animator.SetTrigger(OpenTriggerAnimatorParameter);
        CurrentWindowState = WindowStatesRead.Opened;

    }

    protected virtual void Close_Animator()
    {
        Animator.SetTrigger(CLoseTriggerAnimatorParameter);
        CurrentWindowState = WindowStatesRead.Closed;
    }
    #endregion



    #region Open or close visual methods:  Instantly
    protected virtual void Open_Instantly()
    {
        m_canvasGroup.alpha = 1f;
        m_canvasGroup.blocksRaycasts = true;
        m_canvasGroup.interactable = true;

        CurrentWindowState = WindowStatesRead.Opened;

    }

    protected virtual void Close_Instantly()
    {
        m_canvasGroup.alpha = 0f;
        m_canvasGroup.blocksRaycasts = false;
        m_canvasGroup.interactable = false;

        CurrentWindowState = WindowStatesRead.Closed;
    }
    #endregion



    #region Open or close visual methods:  Slowly

    protected virtual void OpenSlowlyPanel()
    {
        if (openingCoroutine != null)
            StopCoroutine(openingCoroutine);

        openingCoroutine = StartCoroutine(OpenSlowlyPanelCor());
    }

    protected virtual void CloseSlowlyPanel()
    {
        if (closingCoroutine != null)
            StopCoroutine(closingCoroutine);

        closingCoroutine = StartCoroutine(CloseSlowlyPanelCor());
    }

    private IEnumerator OpenSlowlyPanelCor()
    {
        CurrentWindowState = WindowStatesRead.Opening;

        m_canvasGroup.interactable = true;
        m_canvasGroup.blocksRaycasts = true;

        float lastAlpha = m_canvasGroup.alpha;
        
        for (float i = 0f; i < 1; i += Time.deltaTime * ShowingSpeed)
        {
            m_canvasGroup.alpha = Mathf.Lerp(lastAlpha, 1f, Curve.Evaluate(i));
            yield return null;
        }

        m_canvasGroup.alpha = 1f;

        CurrentWindowState = WindowStatesRead.Opened;
        openingCoroutine = null;
    }

    private IEnumerator CloseSlowlyPanelCor()
    {
        CurrentWindowState = WindowStatesRead.Closing;

        m_canvasGroup.interactable = false;
        m_canvasGroup.blocksRaycasts = false;

        float lastAlpha = m_canvasGroup.alpha;

        for (float i = 0f; i < 1f; i += Time.deltaTime * ShowingSpeed)
        {
            m_canvasGroup.alpha = Mathf.Lerp(lastAlpha, 0f, Curve.Evaluate(i));
            yield return null;
        }

        m_canvasGroup.alpha = 0f;

        CurrentWindowState = WindowStatesRead.Closed;
        closingCoroutine = null;
    }

    #endregion


    #region EDITOR
    
#if UNITY_EDITOR
    [CustomEditor(typeof(Window))]
    public class WindowEditor : Editor
    {
        private Window T
        {
            get
            {
                if (_t == null)
                    _t = (Window)target;
                return _t;
            }
        }
        private Window _t;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            switch (T.Close_OpenMethod)
            {
                case PanelOpenCloseMethods.Instantly:
                    break;
                    
                case PanelOpenCloseMethods.Animator:
                    EditorGUI.indentLevel++;
                    T.Animator = (Animator)EditorGUILayout.ObjectField("Animator", T.Animator, typeof(Animator));
                    T.OpenTriggerAnimatorParameter = EditorGUILayout.TextField("Open", T.OpenTriggerAnimatorParameter);
                    T.CLoseTriggerAnimatorParameter = EditorGUILayout.TextField("Close", T.CLoseTriggerAnimatorParameter);
                    EditorGUI.indentLevel--;
                    break;
                
                case PanelOpenCloseMethods.Slowly:
                    EditorGUI.indentLevel++;
                    T.ShowingSpeed = EditorGUILayout.FloatField("Showing Speed", T.ShowingSpeed);
                    T.Curve = EditorGUILayout.CurveField("Curve", T.Curve);
                    EditorGUI.indentLevel--;
                    break;
            }
        }
    }
#endif
    
    #endregion
}
