
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TapCubes
{
    [RequireComponent(typeof(Image))]
    public class Swiper : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("DeathZones")] 
        public float SwipeDeathZone = 50f;
        public float PullDeathZone = 1f;

        [Header("Sensitivity")] 
        public float MoveDeltaSensitivity = 0.01f;
        public float AltMoveDeltaSensitivity = 0.01f;
        public float ZoomSensitivity = 0.01f;
        [Space]
        public float MouseZoomSensitivity = 1f;


        // Swipe
        public UnityEvent<Vector2> SwipeEvent = new UnityEvent<Vector2>();
        private bool canSwipe;


        // Move delta
        public UnityEvent<Vector2> MoveDeltaEvent = new UnityEvent<Vector2>();
        public Vector2 movedDelta { get; protected set; }


        // Zoom
        public UnityEvent<float> ZoomEvent = new UnityEvent<float>();
        private float lastDeltaZoom;
        private bool isZoom;

        // Alternative Move Delta
        // Uses when user moves two fingers on the screen
        public UnityEvent<Vector2> AltMoveDeltaEvent = new UnityEvent<Vector2>();
        public Vector2 altMovedDelta { get; protected set; }


        // Pull
        public UnityEvent<Vector2> PullEvent = new UnityEvent<Vector2>();
        public Vector2 pullDirection { get; protected set; }

        // Some Events
        public UnityEvent<PointerEventData> BeginDragEvent = new UnityEvent<PointerEventData>();
        public UnityEvent<PointerEventData> EndDragEvent = new UnityEvent<PointerEventData>();
        public UnityEvent<PointerEventData> DragEvent = new UnityEvent<PointerEventData>();


        //Some
        private Vector2 startTapPoint;
        private bool mouseInZone = false;


        public void OnDrag(PointerEventData eventData)
        {
            MoveDelta(eventData);
            Swipe(eventData);
            Pull(eventData);
            //Zoom();

            DragEvent?.Invoke(eventData);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            startTapPoint = eventData.position;
            canSwipe = true;

            BeginDragEvent?.Invoke(eventData);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            canSwipe = true;
            pullDirection = Vector2.zero;
            movedDelta = Vector2.zero;

            EndDragEvent?.Invoke(eventData);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseInZone = true;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            mouseInZone = false;
        }


        private void Update()
        {
            Zoom();
            AltMoveDelta();
        }


        private void MoveDelta(PointerEventData eventData)
        {
#if !UNITY_EDITOR
        if(Input.touchCount != 1)
            return;
        movedDelta = eventData.delta * MoveDeltaSensitivity;
        MoveDeltaEvent?.Invoke(movedDelta);
#else
            if (eventData.button == PointerEventData.InputButton.Right) 
            {
                movedDelta = eventData.delta * MoveDeltaSensitivity;
                MoveDeltaEvent?.Invoke(movedDelta);
            }
            else if (eventData.button == PointerEventData.InputButton.Left) 
            {
                if(mouseInZone)
                {
                    altMovedDelta = eventData.delta * AltMoveDeltaSensitivity;
                    AltMoveDeltaEvent?.Invoke(altMovedDelta);
                }
            }
#endif
        }
        private void AltMoveDelta()
        {
            if (Input.touchCount == 2)
            {
                altMovedDelta = ((Input.GetTouch(0).deltaPosition + Input.GetTouch(1).deltaPosition) / 2f) * AltMoveDeltaSensitivity;
                AltMoveDeltaEvent.Invoke(altMovedDelta);
            }
        }
        private void Swipe(PointerEventData eventData)
        {
            if (!canSwipe)
                return;

            Vector2 delta = (eventData.position - startTapPoint);

            if (delta.x > SwipeDeathZone)
            {
                SwipeEvent?.Invoke(Vector2.right);
                canSwipe = false;
            }
            else if (delta.x < -SwipeDeathZone)
            {
                SwipeEvent?.Invoke(Vector2.left);
                canSwipe = false;
            }
            else if (delta.y > SwipeDeathZone)
            {
                SwipeEvent?.Invoke(Vector2.up);
                canSwipe = false;
            }
            else if (delta.y < -SwipeDeathZone)
            {
                SwipeEvent?.Invoke(Vector2.down);
                canSwipe = false;
            }
        }
        private void Zoom()
        {
            if (Input.touchCount == 2)
            {
                if (isZoom == false)
                {
                    isZoom = true;
                    lastDeltaZoom = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
                    return;
                }

                float delta = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
                var zoom = (lastDeltaZoom - delta) * ZoomSensitivity;

                if (Mathf.Abs(zoom) > 0.01)
                    ZoomEvent.Invoke(-zoom);

                lastDeltaZoom = delta;

            }
            else
            {
                isZoom = false;
                if(mouseInZone)
                {
                    ZoomEvent.Invoke(Input.mouseScrollDelta.y * AltMoveDeltaSensitivity * MouseZoomSensitivity);
                }
            }
        }
        private void Pull(PointerEventData eventData)
        {
            var delta = startTapPoint - eventData.position;
            if (delta.magnitude > PullDeathZone)
            {
                pullDirection = delta;
                PullEvent?.Invoke(pullDirection);
            }
        }

    }
}
