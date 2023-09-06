using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using DredPack.DredpackEditor;
using UnityEditor;
#endif
using DredPack.Help;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace DredPack.UI
{
    
    [RequireComponent(typeof(CanvasGroup))]
    public class Window : MonoBehaviour
    {
        #region Enums
        public enum PanelOpenCloseMethods { Animator, Instantly, Slowly, SideAppearCurve, SideAppearConstant, }
        public enum WindowStatesRead { Opened, Opening, Closed, Closing }
        public enum WindowStatesAwake { Open, Close }
        #endregion

        #region Inspector Fields

        #region General

        
        public WindowStatesAwake stateOnAwake = WindowStatesAwake.Close;
        [ReadOnly] 
        public WindowStatesRead CurrentWindowState = WindowStatesRead.Opened;

        public Button CloseButton;
        public Button OpenButton;
        public Button SwitchButton;

        public bool Disengageable = false;
        public bool CloseOnAnyWindowOpen;
        public bool CloseOnOutsideClick;
        public bool disableCanvasOnClose = false;
        public bool disableRaycastOnClose = true;
        public Canvas _canvas;
        public GraphicRaycaster _graphicRaycaster; 

        #endregion

        #region Events

        
        public UnityEvent OpenEvent;
        public UnityEvent CloseEvent;
        public UnityEvent<bool> SwitchEvent;

        #endregion

        #region Animation

        
        public PanelOpenCloseMethods Close_OpenMethod = PanelOpenCloseMethods.Slowly;


        #region Slowly Fields

        [HideInInspector] public float ShowingSpeed = 8f;

        [HideInInspector] public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        #endregion
        
        #region Animator Fields

        [HideInInspector] public Animator Animator;
        [HideInInspector] public string OpenTriggerAnimatorParameter = "Open";
        [HideInInspector] public string CloseTriggerAnimatorParameter = "Close";
        [HideInInspector] public string SpeedAnimatorParameter = "Speed";

        #endregion

        #region SideAppear Fields

        #region Curve
        public AnimationCurve SideAppear_Curve1 = new AnimationCurve(new[] {new Keyframe(0, 0,4.57445812f,4.57445812f), new Keyframe(.6f, 1.1f,0f,0f), new Keyframe(1, 1,0f,0f)});
        #endregion

        #region Constant

        public float SideAppear_bounceValue = 20f;
        [Range(0,1f)]
        public float SideAppear_bounceTime = .6f;

        #endregion

        public float SideAppear_OpenDelay = 0f;
        public float SideAppear_CloseDelay = 0f;
        public float SideAppear_Speed = 2f;
        public RectTransform SideAppear_Up;
        public RectTransform SideAppear_Right;
        public RectTransform SideAppear_Down;
        public RectTransform SideAppear_Left;
        public Image[] SideAppear_Backgrounds;
        private float[] SideAppear_Backgrounds_alphas;
        public CanvasGroup[] SideAppear_Backgrounds_CanvasRenderers;
        private float[] SideAppear_Backgrounds_CanvasRenderers_alphas;

        #endregion

        #endregion



        
        #endregion
        
        #region innear fields
        

        protected Coroutine openingCoroutine;
        protected Coroutine closingCoroutine;
        
        private static UnityEvent<Window> windowOpenEventStatic = new UnityEvent<Window>();


        private float sideAppear_UpDefY;
        private float sideAppear_RightDefX;
        private float sideAppear_DownDefY;
        private float sideAppear_LeftDefX;
        private float sideAppear_defBackgroundAlpha;

        public static int currentWindowTab;

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
        public UnityEngine.Camera camera;
        
        #endregion

        private void Start()
        {
            Initialization();
        }

        private void Update()
        {
            CheckOutsideClick();
        }

        private void CheckOutsideClick()
        {
            if (CloseOnOutsideClick)
            {
                // Проверяем, было ли касание на экране
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    CheckWindow(Input.GetTouch(0).position);
                else if(Input.GetMouseButtonUp(0))
                    CheckWindow(Input.mousePosition);
                void CheckWindow(Vector2 touchPosition)
                {
                    // Проверяем, если окно активно и касание было вне его области, закрываем окно
                    if (CurrentWindowState == WindowStatesRead.Opened &&
                        !RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), touchPosition,
                            camera))
                        Close();
                }
            }
            
        }
        
        private void SwitchGraphicRaycaster(bool arg0)
        {
            if(_graphicRaycaster && disableRaycastOnClose)
                _graphicRaycaster.enabled = arg0;
        }
        private void OnEnable()
        {
            windowOpenEventStatic.AddListener(OnWindowOpen);
        }

        private void OnDisable()
        {
            windowOpenEventStatic.RemoveListener(OnWindowOpen);
        }

        protected virtual void Initialization()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (OpenButton)
                OpenButton.onClick.AddListener(Open);
            if (CloseButton)
                CloseButton.onClick.AddListener(Close);
            if (SwitchButton)
                SwitchButton.onClick.AddListener(Switch);
            
            if(disableRaycastOnClose)
                SwitchEvent.AddListener(SwitchGraphicRaycaster);
            switch (stateOnAwake)
            {
                case WindowStatesAwake.Close:
                    Close_Instantly();
                    break;

                case WindowStatesAwake.Open:
                    Open_Instantly();
                    break;
            }

            if (SideAppear_Up)
                sideAppear_UpDefY = SideAppear_Up.anchoredPosition.y;
            if (SideAppear_Right)
                sideAppear_RightDefX = SideAppear_Right.anchoredPosition.x;
            if (SideAppear_Down)
                sideAppear_DownDefY = SideAppear_Down.anchoredPosition.y;
            if (SideAppear_Left)
                sideAppear_LeftDefX = SideAppear_Left.anchoredPosition.x;
            
            
            SideAppear_Backgrounds_alphas = new float[SideAppear_Backgrounds.Length];
            for (int i = 0; i < SideAppear_Backgrounds.Length; i++)
                if (SideAppear_Backgrounds[i])
                    SideAppear_Backgrounds_alphas[i] = SideAppear_Backgrounds[i].color.a;
            
            SideAppear_Backgrounds_CanvasRenderers_alphas = new float[SideAppear_Backgrounds_CanvasRenderers.Length];
            for (var i = 0; i < SideAppear_Backgrounds_CanvasRenderers.Length; i++)
                if (SideAppear_Backgrounds_CanvasRenderers[i])
                    SideAppear_Backgrounds_CanvasRenderers_alphas[i] = SideAppear_Backgrounds_CanvasRenderers[i].alpha;

        }



        /// <summary>
        /// Call to Open this window
        /// </summary>
        public virtual void Open()
        {
            if(Disengageable)
                gameObject.SetActive(true);
            if (disableCanvasOnClose && _canvas)
                _canvas.enabled = true;
            windowOpenEventStatic?.Invoke(this);
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
                case PanelOpenCloseMethods.SideAppearCurve:
                    Open_SideAppearCurve();
                    break;
                case PanelOpenCloseMethods.SideAppearConstant:
                    Open_SideAppearConstant();
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
                case PanelOpenCloseMethods.SideAppearCurve:
                    Close_SideAppearCurve();
                    break;
                case PanelOpenCloseMethods.SideAppearConstant:
                    Close_SideAppearConstant();
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
            if (CurrentWindowState == WindowStatesRead.Opened || CurrentWindowState == WindowStatesRead.Opening)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public void Switch(bool state)
        {
            if (state)
                Open();
            else 
                Close();
        }
        
        private void OnWindowOpen(DredPack.UI.Window window)
        {
            if (window != this && CloseOnAnyWindowOpen)
                Close();
        }


        #region Open or close visual methods:  Animator

        public virtual void Open_Animator(bool instantly = false)
        {
            SwitchEvent?.Invoke(true);
            OpenEvent?.Invoke();
            if (Animator == null)
                Animator = GetComponent<Animator>();
            if(instantly || CurrentWindowState != WindowStatesRead.Opened)
            {
                Animator.SetTrigger(OpenTriggerAnimatorParameter);
                Animator.SetFloat(SpeedAnimatorParameter,instantly ? 10000 : 1);
            }
            CurrentWindowState = WindowStatesRead.Opened;

        }

        public virtual void Close_Animator(bool instantly = false)
        {
            SwitchEvent?.Invoke(false);
            CloseEvent?.Invoke();
            if (Animator == null)
                Animator = GetComponent<Animator>();
            if(instantly || CurrentWindowState != WindowStatesRead.Closed)
            {
                Animator.SetTrigger(CloseTriggerAnimatorParameter);
                Animator.SetFloat(SpeedAnimatorParameter, instantly ? 10000 : 1);
            }
            CurrentWindowState = WindowStatesRead.Closed;
        }

        #endregion



        #region Open or close visual methods:  Instantly

        public virtual void Open_Instantly()
        {
            if(Disengageable && Application.isPlaying)
                gameObject.SetActive(true);
            if (disableCanvasOnClose && _canvas && Application.isPlaying)
                _canvas.enabled = true;
            if (Close_OpenMethod == PanelOpenCloseMethods.Animator)
            {
                Open_Animator(true);
            }
            SwitchEvent?.Invoke(true);
            OpenEvent?.Invoke();
            m_canvasGroup.alpha = 1f;
            m_canvasGroup.blocksRaycasts = true;
            m_canvasGroup.interactable = true;

            CurrentWindowState = WindowStatesRead.Opened;
            
        }

        public virtual void Close_Instantly()
        {
            if (Close_OpenMethod == PanelOpenCloseMethods.Animator)
            {
                Close_Animator(true);
            }
            SwitchEvent?.Invoke(false);
            CloseEvent?.Invoke();
            m_canvasGroup.alpha = 0f;
            m_canvasGroup.blocksRaycasts = false;
            m_canvasGroup.interactable = false;

            CurrentWindowState = WindowStatesRead.Closed;
            if(Disengageable && Application.isPlaying)
                gameObject.SetActive(false);
            if (disableCanvasOnClose && _canvas && Application.isPlaying)
                _canvas.enabled = false;
        }

        #endregion



        #region Open or close visual methods:  Slowly

        public virtual void OpenSlowlyPanel()
        {
            SwitchEvent?.Invoke(true);
            OpenEvent?.Invoke();
            if (openingCoroutine != null)
                StopCoroutine(openingCoroutine);

            openingCoroutine = StartCoroutine(OpenSlowlyPanelCor());
        }

        public virtual void CloseSlowlyPanel()
        {
            SwitchEvent?.Invoke(false);
            CloseEvent?.Invoke();
            if (closingCoroutine != null)
                StopCoroutine(closingCoroutine);

            closingCoroutine = StartCoroutine(CloseSlowlyPanelCor());
        }

        public IEnumerator OpenSlowlyPanelCor()
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

        public IEnumerator CloseSlowlyPanelCor()
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
            
            if(Disengageable)
                gameObject.SetActive(false);
            if (disableCanvasOnClose && _canvas)
                _canvas.enabled = false;
        }

        #endregion

        #region Open or close visual methods: SideAppear By Curve


        public virtual void Open_SideAppearCurve()
        {
            if(CurrentWindowState == WindowStatesRead.Opening || CurrentWindowState == WindowStatesRead.Opened)
                return;
            if(Disengageable)
                gameObject.SetActive(true);
            if (disableCanvasOnClose && _canvas)
                _canvas.enabled = true;
            CurrentWindowState = WindowStatesRead.Opening;
            SwitchEvent?.Invoke(true);
            OpenEvent?.Invoke();
            
            StopAllCoroutines();
            openingCoroutine = StartCoroutine(IE());

            IEnumerator IE()
            {
                m_canvasGroup.interactable = false;
                m_canvasGroup.blocksRaycasts = false;
                yield return new WaitForSeconds(SideAppear_OpenDelay);
                m_canvasGroup.alpha = 1f;
                //up
                if (SideAppear_Up) // 4.57445812 0.484901249
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_UpDefY, sideAppear_UpDefY, SideAppear_Speed,
                        SideAppear_Curve1,
                        _ => SideAppear_Up.anchoredPosition = new Vector2(SideAppear_Up.anchoredPosition.x, _)));
                //right
                if (SideAppear_Right)
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_RightDefX, sideAppear_RightDefX, SideAppear_Speed,
                        SideAppear_Curve1,
                        _ => SideAppear_Right.anchoredPosition = new Vector2(_, SideAppear_Right.anchoredPosition.y)));
                //down
                if (SideAppear_Down)
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_DownDefY, sideAppear_DownDefY, SideAppear_Speed,
                        SideAppear_Curve1,
                        _ => SideAppear_Down.anchoredPosition = new Vector2(SideAppear_Down.anchoredPosition.x, _)));
                //left
                if (SideAppear_Left)
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_LeftDefX, sideAppear_LeftDefX, SideAppear_Speed,
                        SideAppear_Curve1,
                        _ => SideAppear_Left.anchoredPosition = new Vector2(_, SideAppear_Left.anchoredPosition.y)));
                
                //backgrounds
                for (var i = 0; i < SideAppear_Backgrounds.Length; i++)
                {
                    var b = SideAppear_Backgrounds[i];
                    if(b)
                        StartCoroutine(Lerper.LerpFloatIE(0f,SideAppear_Backgrounds_alphas[i], SideAppear_Speed,
                            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
                            _ => b.color = new Color(b.color.r, b.color.g, b.color.b, _)));
                }
                

                //canvas renderers
                for (var i = 0; i < SideAppear_Backgrounds_CanvasRenderers.Length; i++)
                {
                    var c = SideAppear_Backgrounds_CanvasRenderers[i];
                    if (c)
                    {
                        StartCoroutine(Lerper.LerpFloatIE(0f,SideAppear_Backgrounds_CanvasRenderers_alphas[i],
                            SideAppear_Speed,
                            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
                            _ => c.alpha = _));
                    }
                }
                yield return new WaitForSeconds(1f / SideAppear_Speed);

                CurrentWindowState = WindowStatesRead.Opened;
                m_canvasGroup.interactable = true;
                m_canvasGroup.blocksRaycasts = true;
                closingCoroutine = null;
                
            }
        }

        public virtual void Close_SideAppearCurve()
        {
            if(CurrentWindowState == WindowStatesRead.Closing || CurrentWindowState == WindowStatesRead.Closed)
                return;
            CurrentWindowState = WindowStatesRead.Closing;
            SwitchEvent?.Invoke(false);
            CloseEvent?.Invoke();
            
            StopAllCoroutines();
            closingCoroutine = StartCoroutine(IE());

            IEnumerator IE()
            {
                m_canvasGroup.interactable = false;
                m_canvasGroup.blocksRaycasts = false;
                yield return new WaitForSeconds(SideAppear_CloseDelay);
                //up
                if (SideAppear_Up)
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_UpDefY, sideAppear_UpDefY, SideAppear_Speed, InverseCurve.Get(SideAppear_Curve1),
                        _ => SideAppear_Up.anchoredPosition = new Vector2(SideAppear_Up.anchoredPosition.x, _)));
                //right
                if (SideAppear_Right)
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_RightDefX, sideAppear_RightDefX, SideAppear_Speed, InverseCurve.Get(SideAppear_Curve1),
                        _ => SideAppear_Right.anchoredPosition = new Vector2(_, SideAppear_Right.anchoredPosition.y)));
                //down
                if (SideAppear_Down)
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_DownDefY, sideAppear_DownDefY, SideAppear_Speed, InverseCurve.Get(SideAppear_Curve1),
                        _ => SideAppear_Down.anchoredPosition = new Vector2(SideAppear_Down.anchoredPosition.x, _)));
                //left
                if (SideAppear_Left)
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_LeftDefX, sideAppear_LeftDefX, SideAppear_Speed, InverseCurve.Get(SideAppear_Curve1),
                        _ => SideAppear_Left.anchoredPosition = new Vector2(_, SideAppear_Left.anchoredPosition.y)));
                
                
                //backgrounds
                for (var i = 0; i < SideAppear_Backgrounds.Length; i++)
                {
                    var b = SideAppear_Backgrounds[i];
                    if(b)
                        StartCoroutine(Lerper.LerpFloatIE(SideAppear_Backgrounds_alphas[i], 0f, SideAppear_Speed,
                            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
                            _ => b.color = new Color(b.color.r, b.color.g, b.color.b, _)));
                }
                

                //canvas renderers
                for (var i = 0; i < SideAppear_Backgrounds_CanvasRenderers.Length; i++)
                {
                    var c = SideAppear_Backgrounds_CanvasRenderers[i];
                    if (c)
                    {
                        StartCoroutine(Lerper.LerpFloatIE(SideAppear_Backgrounds_CanvasRenderers_alphas[i], 0f,
                            SideAppear_Speed,
                            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
                            _ => c.alpha = _));
                    }
                }
                
                yield return new WaitForSeconds(1f / SideAppear_Speed);
                
                

                m_canvasGroup.alpha = 0f;
                CurrentWindowState = WindowStatesRead.Closed;
                closingCoroutine = null;
                
                if(Disengageable)
                    gameObject.SetActive(false);
                if (disableCanvasOnClose && _canvas)
                    _canvas.enabled = false;
            }
        }

        #endregion

        #region Open or close visual methods: SideAppear By Constant Value


        public virtual void Open_SideAppearConstant()
        {
            if(CurrentWindowState == WindowStatesRead.Opening || CurrentWindowState == WindowStatesRead.Opened)
                return;
            if(Disengageable)
                gameObject.SetActive(true);
            if (disableCanvasOnClose && _canvas)
                _canvas.enabled = true;
            CurrentWindowState = WindowStatesRead.Opening;
            SwitchEvent?.Invoke(true);
            OpenEvent?.Invoke();
            
            StopAllCoroutines();
            if (openingCoroutine != null)
                StopCoroutine(openingCoroutine);
            if (closingCoroutine != null)
                StopCoroutine(closingCoroutine);
            openingCoroutine = StartCoroutine(IE());

            IEnumerator IE()
            {
                m_canvasGroup.interactable = false;
                m_canvasGroup.blocksRaycasts = false;
                yield return new WaitForSeconds(SideAppear_OpenDelay);
                m_canvasGroup.alpha = 1f;
                //up
                if (SideAppear_Up)
                {
                    var curve = GetSideAppearBounceCurve(Mathf.Abs(sideAppear_UpDefY));
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_UpDefY, sideAppear_UpDefY, SideAppear_Speed, curve,
                        _ => SideAppear_Up.anchoredPosition = new Vector2(SideAppear_Up.anchoredPosition.x, _)));
                }
                //right
                if (SideAppear_Right)
                {
                    var curve = GetSideAppearBounceCurve(Mathf.Abs(sideAppear_RightDefX)); 
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_RightDefX, sideAppear_RightDefX, SideAppear_Speed, curve,
                        _ => SideAppear_Right.anchoredPosition = new Vector2(_, SideAppear_Right.anchoredPosition.y)));
                }
                //down
                if (SideAppear_Down)
                {
                    var curve = GetSideAppearBounceCurve(Mathf.Abs(sideAppear_DownDefY)); 
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_DownDefY, sideAppear_DownDefY, SideAppear_Speed, curve,
                        _ => SideAppear_Down.anchoredPosition = new Vector2(SideAppear_Down.anchoredPosition.x, _)));
                }
                //left
                if (SideAppear_Left)
                {
                    var curve = GetSideAppearBounceCurve(Mathf.Abs(sideAppear_LeftDefX)); 
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_LeftDefX, sideAppear_LeftDefX, SideAppear_Speed, curve,
                        _ => SideAppear_Left.anchoredPosition = new Vector2(_, SideAppear_Left.anchoredPosition.y)));
                }
                
                
                //backgrounds
                for (var i = 0; i < SideAppear_Backgrounds.Length; i++)
                {
                    var b = SideAppear_Backgrounds[i];
                    if(b)
                        StartCoroutine(Lerper.LerpFloatIE(0f,SideAppear_Backgrounds_alphas[i], SideAppear_Speed,
                            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
                            _ => b.color = new Color(b.color.r, b.color.g, b.color.b, _)));
                }
                

                //canvas renderers
                for (var i = 0; i < SideAppear_Backgrounds_CanvasRenderers.Length; i++)
                {
                    var c = SideAppear_Backgrounds_CanvasRenderers[i];
                    if (c)
                    {
                        StartCoroutine(Lerper.LerpFloatIE(0f,SideAppear_Backgrounds_CanvasRenderers_alphas[i],
                            SideAppear_Speed,
                            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
                            _ => c.alpha = _));
                    }
                }
                yield return new WaitForSeconds(1f / SideAppear_Speed);

                CurrentWindowState = WindowStatesRead.Opened;
                m_canvasGroup.interactable = true;
                m_canvasGroup.blocksRaycasts = true;
                closingCoroutine = null;
                
            }
        }

        public virtual void Close_SideAppearConstant()
        {
            if(CurrentWindowState == WindowStatesRead.Closing || CurrentWindowState == WindowStatesRead.Closed)
                return;
            CurrentWindowState = WindowStatesRead.Closing;
            SwitchEvent?.Invoke(false);
            CloseEvent?.Invoke();
            
            StopAllCoroutines();
            if (closingCoroutine != null)
                StopCoroutine(closingCoroutine);
            if (openingCoroutine != null)
                StopCoroutine(openingCoroutine);
            closingCoroutine = StartCoroutine(IE());

            IEnumerator IE()
            {
                m_canvasGroup.interactable = false;
                m_canvasGroup.blocksRaycasts = false;
                yield return new WaitForSeconds(SideAppear_CloseDelay);
                //up
                if (SideAppear_Up)
                {
                    var curve = GetSideAppearBounceCurve(Mathf.Abs(sideAppear_UpDefY));
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_UpDefY, sideAppear_UpDefY, SideAppear_Speed,
                        InverseCurve.Get(curve),
                        _ => SideAppear_Up.anchoredPosition = new Vector2(SideAppear_Up.anchoredPosition.x, _)));
                }
                //right
                if (SideAppear_Right)
                {
                    var curve = GetSideAppearBounceCurve(Mathf.Abs(sideAppear_RightDefX)); 
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_RightDefX, sideAppear_RightDefX, SideAppear_Speed,
                        InverseCurve.Get(curve),
                        _ => SideAppear_Right.anchoredPosition = new Vector2(_, SideAppear_Right.anchoredPosition.y)));
                }
                //down
                if (SideAppear_Down)
                {
                    var curve = GetSideAppearBounceCurve(Mathf.Abs(sideAppear_DownDefY)); 
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_DownDefY, sideAppear_DownDefY, SideAppear_Speed,
                        InverseCurve.Get(curve),
                        _ => SideAppear_Down.anchoredPosition = new Vector2(SideAppear_Down.anchoredPosition.x, _)));
                }
                //left
                if (SideAppear_Left)
                {
                    
                    var curve = GetSideAppearBounceCurve(Mathf.Abs(sideAppear_LeftDefX)); 
                    StartCoroutine(Lerper.LerpFloatIE(-sideAppear_LeftDefX, sideAppear_LeftDefX, SideAppear_Speed,
                        InverseCurve.Get(curve),
                        _ => SideAppear_Left.anchoredPosition = new Vector2(_, SideAppear_Left.anchoredPosition.y)));
                }
                
                //backgrounds
                for (var i = 0; i < SideAppear_Backgrounds.Length; i++)
                {
                    var b = SideAppear_Backgrounds[i];
                    if(b)
                        StartCoroutine(Lerper.LerpFloatIE(SideAppear_Backgrounds_alphas[i], 0f, SideAppear_Speed,
                            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
                            _ => b.color = new Color(b.color.r, b.color.g, b.color.b, _)));
                }
                

                //canvas renderers
                for (var i = 0; i < SideAppear_Backgrounds_CanvasRenderers.Length; i++)
                {
                    var c = SideAppear_Backgrounds_CanvasRenderers[i];
                    if (c)
                    {
                        StartCoroutine(Lerper.LerpFloatIE(SideAppear_Backgrounds_CanvasRenderers_alphas[i], 0f,
                            SideAppear_Speed,
                            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f),
                            _ => c.alpha = _));
                    }
                }
                yield return new WaitForSeconds(1f / SideAppear_Speed);
                
                

                m_canvasGroup.alpha = 0f;
                CurrentWindowState = WindowStatesRead.Closed;
                closingCoroutine = null;
                
                if(Disengageable)
                    gameObject.SetActive(false);
                if (disableCanvasOnClose && _canvas)
                    _canvas.enabled = false;
            }
        }

        public void FindSidePanels()
        {
            var left = transform.Find("Left");
            var right = transform.Find("Right");
            var down = transform.Find("Down");
            var up = transform.Find("Up");
            SideAppear_Left =   left  ? left.GetComponent<RectTransform>()  : SideAppear_Left;
            SideAppear_Right =  right ? right.GetComponent<RectTransform>() : SideAppear_Right;
            SideAppear_Down =   down  ? down.GetComponent<RectTransform>()  : SideAppear_Down;
            SideAppear_Up =     up    ? up.GetComponent<RectTransform>()    : SideAppear_Up;
            
            /*var background = GetComponent<Image>();
            SideAppear_Background = background ?? SideAppear_Background;*/
        }

        private AnimationCurve GetSideAppearBounceCurve(float maxVal)
        {
            var val = (maxVal + SideAppear_bounceValue) / maxVal;
            var key = new Keyframe(SideAppear_bounceTime,val);
            return new AnimationCurve(new[]
            {
                new Keyframe(0, 0,4.57445812f,4.57445812f), 
                key, 
                new Keyframe(1, 1,0f,0f)
            });

        }
        #endregion



        #region EDITOR

#if UNITY_EDITOR
        [CustomEditor(typeof(Window))]
        public class WindowEditor : DredInspectorEditor<Window>
        {

            public override void OnInspectorGUI()
            {
                serializedObject.Update();
                
                DrawComponentHeader();
                Tabs();
                EditorGUILayout.BeginVertical(GUI.skin.box);
                switch (currentWindowTab)
                {
                    case 0: Tabs_General(); break;
                    case 1: Tabs_Events(); break;
                    case 2: Tabs_Animation(); break;
                }
                EditorGUILayout.EndVertical();
                
                serializedObject.ApplyModifiedProperties();
            }
            private void Reset()
            {
                T._canvas = T.GetComponent<Canvas>();
                if(!T._graphicRaycaster)
                    T._graphicRaycaster = T.GetComponent<GraphicRaycaster>();
                //T.disableCanvasOnClose = T._canvas;
            }

            private void Tabs()
            {
                GUIContent[] toolbarTabs = new GUIContent[3];
                toolbarTabs[0] = new GUIContent("General");
                toolbarTabs[1] = new GUIContent("Events");
                toolbarTabs[2] = new GUIContent("Animation");
                currentWindowTab = GUILayout.Toolbar(currentWindowTab, toolbarTabs);
            }

            private void Tabs_General()
            {
                var labelStyle = LabelStyle();

                GUILayout.Label("States", labelStyle);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.stateOnAwake)), new GUIContent("On Start"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.CurrentWindowState)), new GUIContent("Current"));
                EditorGUI.indentLevel--;
                if (GUILayout.Button("Switch State"))
                    T.SwitchState();
                if (Application.isPlaying)
                {
                    if (GUILayout.Button("Switch with animation"))
                        T.Switch();
                }
                 
                GUILayout.Space(5);
                
                GUILayout.Label("Buttons",labelStyle);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.CloseButton)), new GUIContent("Close"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.OpenButton)), new GUIContent("Open"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SwitchButton)), new GUIContent("Switch"));
                EditorGUI.indentLevel--;
                
                GUILayout.Label("Some",labelStyle);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.Disengageable)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.CloseOnAnyWindowOpen)));
                
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.CloseOnOutsideClick)));
                EditorGUI.indentLevel++;
                if (T.CloseOnOutsideClick)
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.camera)), new GUIContent("Camera"));
                EditorGUI.indentLevel--;
                    
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.disableCanvasOnClose)));
                EditorGUI.indentLevel++;
                if(T.disableCanvasOnClose)
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T._canvas)), new GUIContent("Canvas"));
                EditorGUI.indentLevel--;
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.disableRaycastOnClose)));
                EditorGUI.indentLevel++;
                if(T.disableRaycastOnClose)
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T._graphicRaycaster)), new GUIContent("GraphicRaycaster"));
                EditorGUI.indentLevel--;
                
                
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
                
            }


            private void Tabs_Events()
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.OpenEvent)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.CloseEvent)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SwitchEvent)));
            }

            private void Tabs_Animation()
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.Close_OpenMethod)), new GUIContent("Animation"));
                EditorGUI.indentLevel++;
                switch (T.Close_OpenMethod)
                {
                    case PanelOpenCloseMethods.Instantly:
                        break;

                    case PanelOpenCloseMethods.Animator:
                        
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.Animator)));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.OpenTriggerAnimatorParameter)), new GUIContent("Open Trigger"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.CloseTriggerAnimatorParameter)), new GUIContent("Close Trigger"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SpeedAnimatorParameter)), new GUIContent("Speed Parameter"));
                        EditorGUI.indentLevel--;
                        break;

                    case PanelOpenCloseMethods.Slowly:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.ShowingSpeed)), new GUIContent("Speed"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.Curve)));
                        break;
                    case PanelOpenCloseMethods.SideAppearCurve:
                    case PanelOpenCloseMethods.SideAppearConstant:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SideAppear_Speed)),new GUIContent("Speed"));
                        if(T.Close_OpenMethod == PanelOpenCloseMethods.SideAppearCurve)
                            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SideAppear_Curve1)),new GUIContent("Curve"));
                        else
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SideAppear_bounceTime)),new GUIContent("Bounce Time"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SideAppear_bounceValue)),new GUIContent("Bounce Value"));
                        }
                        GUILayout.Space(8);
                        
                        EditorGUILayout.LabelField("Delays",LabelStyle(12));
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                        GUILayout.Label("Open", GUILayout.Width(33));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SideAppear_OpenDelay)),GUIContent.none);
                        GUILayout.Label("Close", GUILayout.Width(36));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SideAppear_CloseDelay)),GUIContent.none);
                        EditorGUILayout.EndHorizontal();
                        
                        GUILayout.Space(8);
                        if (GUILayout.Button("Find Panels"))
                            T.FindSidePanels();

                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SideAppear_Up)),new GUIContent("Up"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SideAppear_Right)),new GUIContent("Right"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SideAppear_Down)),new GUIContent("Down"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SideAppear_Left)),new GUIContent("Left"));
                        GUILayout.Space(3);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SideAppear_Backgrounds)),new GUIContent("Backgrounds"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(T.SideAppear_Backgrounds_CanvasRenderers)),new GUIContent("CanvasRenderers"));
                        break;
                }
                EditorGUI.indentLevel--;

            }
        }
#endif

        #endregion
    }

}
