using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static event onTouchEvent onStartTouch;
    public static event onTouchEvent onFinishTouch;
    public delegate void onTouchEvent();
    public void OnPointerDown(PointerEventData eventData)
    {
        onStartTouch?.Invoke();
    }   

    public void OnPointerUp(PointerEventData eventData)
    {
        onFinishTouch?.Invoke();
    }

}
