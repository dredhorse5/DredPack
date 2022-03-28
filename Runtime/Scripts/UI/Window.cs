using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public class Window : MonoBehaviour
{
    #region Enums

    public enum SideAppearType
    {
        FromLeft,
        FromRight,
        FromUp,
        FromDown
    }
    
    public enum PanelOpenCloseMethods
    {
        Animator,
        Instantly,
        Slowly,
        SideAppear
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

    #endregion
   

    
    
    
    [SerializeField]
    public WindowStatesAwake stateOnAwake = WindowStatesAwake.Close;
    [ReadOnly]
    [SerializeField]
    public WindowStatesRead CurrentWindowState = WindowStatesRead.Opened;
    [Button("SwitchState")]
    public bool btn;

    [Space] 
    public Button CloseButton;
    public Button OpenButton;
    public Button SwitchButton;
    [Space]
    [SerializeField]
    public PanelOpenCloseMethods Close_OpenMethod = PanelOpenCloseMethods.Slowly;
    
    
    

    #region Slowly Fields
    
    [HideInInspector]
    public float ShowingSpeed = 1f;
    
    [NonSerialized] 
    public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    #endregion
    
    
    

    #region Animator Fields

    [NonSerialized]
    public Animator Animator;
    [NonSerialized]
    public string OpenTriggerAnimatorParameter = "Open";
    [NonSerialized]
    public string CLoseTriggerAnimatorParameter = "Close";
    
    #endregion
    
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
        if(OpenButton)
            OpenButton.onClick.AddListener(Open);
        if(CloseButton)
            CloseButton.onClick.AddListener(Close);
        if (SwitchButton)
            SwitchButton.onClick.AddListener(SwitchState);
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

    public void SwitchState()
    {
        if (CurrentWindowState == WindowStatesRead.Opened || CurrentWindowState == WindowStatesRead.Opening)
        {
            Close_Instantly();
        }
        else
        {
            Open_Instantly();
        }
    }
    public void Switch()
    {
        if(CurrentWindowState == WindowStatesRead.Opened || CurrentWindowState == WindowStatesRead.Opening)
        {
            Close();
        }
        else
        {
            Open();
        }
    }


    #region Open or close visual methods:  Animator
    public virtual void Open_Animator()
    {
        if (Animator == null)
            Animator = GetComponent<Animator>();
        Animator.SetTrigger(OpenTriggerAnimatorParameter);
        CurrentWindowState = WindowStatesRead.Opened;

    }

    public virtual void Close_Animator()
    {
        if (Animator == null)
            Animator = GetComponent<Animator>();
        Animator.SetTrigger(CLoseTriggerAnimatorParameter);
        CurrentWindowState = WindowStatesRead.Closed;
    }
    #endregion



    #region Open or close visual methods:  Instantly
    public virtual void Open_Instantly()
    {
        m_canvasGroup.alpha = 1f;
        m_canvasGroup.blocksRaycasts = true;
        m_canvasGroup.interactable = true;

        CurrentWindowState = WindowStatesRead.Opened;

    }

    public virtual void Close_Instantly()
    {
        m_canvasGroup.alpha = 0f;
        m_canvasGroup.blocksRaycasts = false;
        m_canvasGroup.interactable = false;

        CurrentWindowState = WindowStatesRead.Closed;
    }
    #endregion



    #region Open or close visual methods:  Slowly

    public virtual void OpenSlowlyPanel()
    {
        if (openingCoroutine != null)
            StopCoroutine(openingCoroutine);

        openingCoroutine = StartCoroutine(OpenSlowlyPanelCor());
    }

    public virtual void CloseSlowlyPanel()
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

    #region Open or close visual methods: Appear from side

    

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
            EditorGUI.BeginChangeCheck();
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
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(T);
            }

        }
    }
#endif
    
    #endregion
}
