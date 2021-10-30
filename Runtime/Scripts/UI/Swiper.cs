using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Swiper : MonoBehaviour, IDragHandler, IBeginDragHandler,IEndDragHandler
{
    public float SwipeDeathZone = 50f;

    [Header("Sensitivity")] 
    public float MoveDeltaSensitivity = 0.01f;
    public float ZoomSensitivity = 0.01f;

    public UnityEvent<Vector2> SwipeEven = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> MoveDeltaEvent= new UnityEvent<Vector2>();
    public UnityEvent<float> ZoomEvent = new UnityEvent<float>();

    public Vector2 movedDelta;

    private Vector2 startPoint;
    private bool canSwipe;


    private float lastDeltaZoom;
    private bool isZoom;
    /*private Vector2 startPoint_zoom1;
    private Vector2 startPoint_zoom2;*/
    
    public void OnDrag(PointerEventData eventData)
    {
        MoveDelta(eventData);
        Swipe(eventData);
        Zoom();
    }
    private void Update()
    {
        Zoom();
    }

    
    public void MoveDelta(PointerEventData eventData)
    {
        if(Input.touchCount != 1)
            return;
        movedDelta = eventData.delta * MoveDeltaSensitivity; 
        MoveDeltaEvent?.Invoke(movedDelta);
    }
    public void Swipe(PointerEventData eventData)
    {
        if(!canSwipe)
            return;
        
        Vector2 delta = (eventData.position - startPoint);

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
    
    
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPoint = eventData.position;
        canSwipe = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canSwipe = true;
    }
}
