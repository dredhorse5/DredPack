using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Swiper : MonoBehaviour, IDragHandler, IBeginDragHandler,IEndDragHandler
{
    public float DeathZone = 50f;

    public UnityEvent<Vector2> SwipeEven = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> MoveDeltaEvent= new UnityEvent<Vector2>();

    private Vector2 pointPressed;
    private bool canEvent;
    
    public void OnDrag(PointerEventData eventData)
    {
        MoveDeltaEvent?.Invoke(eventData.delta);
        
        if(!canEvent)
            return;
        
        Vector2 delta = (eventData.position - pointPressed);

        if (delta.x > DeathZone)
        {
            SwipeEven?.Invoke(Vector2.right);
            canEvent = false;
        }
        else if (delta.x < -DeathZone)
        {
            SwipeEven?.Invoke(Vector2.left);
            canEvent = false;
        }
        else if (delta.y > DeathZone)
        {
            SwipeEven?.Invoke(Vector2.up);
            canEvent = false;
        }
        else if (delta.y < -DeathZone)
        {
            SwipeEven?.Invoke(Vector2.down);
            canEvent = false;
        }
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        pointPressed = eventData.position;
        canEvent = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canEvent = true;
    }
}
