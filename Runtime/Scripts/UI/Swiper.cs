using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Swiper : MonoBehaviour, IDragHandler, IBeginDragHandler,IEndDragHandler
{
    [Header("DeathZones")]
    public float SwipeDeathZone = 50f;
    public float PullDeathZone = 1f;

    [Header("Sensitivity")] 
    public float MoveDeltaSensitivity = 0.01f;
    public float ZoomSensitivity = 0.01f;

    
    
    
    public UnityEvent<Vector2> SwipeEven = new UnityEvent<Vector2>();
    private bool canSwipe;
    
    
    
    public UnityEvent<Vector2> MoveDeltaEvent= new UnityEvent<Vector2>();
    public Vector2 movedDelta { get; protected set; }
    
    
    
    public UnityEvent<float> ZoomEvent = new UnityEvent<float>();
    private float lastDeltaZoom;
    private bool isZoom;

    public UnityEvent<Vector2> PullEvent = new UnityEvent<Vector2>();
    public Vector2 pullDirection { get; protected set; }



    private Vector2 startTapPoint;

    
    public void OnDrag(PointerEventData eventData)
    {
        MoveDelta(eventData);
        Swipe(eventData);
        Pull(eventData);
        Zoom();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        startTapPoint = eventData.position;
        canSwipe = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canSwipe = true;
    }
    private void Update()
    {
        Zoom();
    }

    
    public void MoveDelta(PointerEventData eventData)
    {
#if !UNITY_EDITOR
        if(Input.touchCount != 1)
            return;
#endif
        movedDelta = eventData.delta * MoveDeltaSensitivity; 
        MoveDeltaEvent?.Invoke(movedDelta);
    }
    public void Swipe(PointerEventData eventData)
    {
        if(!canSwipe)
            return;
        
        Vector2 delta = (eventData.position - startTapPoint);

        if (delta.x > SwipeDeathZone)
        {
            SwipeEven?.Invoke(Vector2.right);
            canSwipe = false;
        }
        else if (delta.x < -SwipeDeathZone)
        {
            SwipeEven?.Invoke(Vector2.left);
            canSwipe = false;
        }
        else if (delta.y > SwipeDeathZone)
        {
            SwipeEven?.Invoke(Vector2.up);
            canSwipe = false;
        }
        else if (delta.y < -SwipeDeathZone)
        {
            SwipeEven?.Invoke(Vector2.down);
            canSwipe = false;
        }
    }
    public void Zoom()
    {
        if (Input.touchCount == 2)
        { 
            if(isZoom == false)
            {
                isZoom = true;
                lastDeltaZoom = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
                return;
            }
            float delta = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
            var zoom = (lastDeltaZoom - delta)*ZoomSensitivity;
            
            if(Mathf.Abs(zoom) > 0.01)
                ZoomEvent.Invoke(-zoom);
            
            lastDeltaZoom = delta;

        }
        else
        {
            isZoom = false;
        }
    }

    public void Pull(PointerEventData eventData)
    {
        var delta = startTapPoint - eventData.position;
        if (delta.magnitude > PullDeathZone)
        {
            pullDirection = delta;
            PullEvent?.Invoke(pullDirection);
        }
    }
    
    
    
}
