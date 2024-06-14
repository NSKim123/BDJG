using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    protected GameObject _Lever;

    protected Vector2 _TouchStartPosition;

    protected RectTransform leverTransform => _Lever.transform as RectTransform;

    public event System.Action<Vector2> onDrag;

    protected virtual void Awake()
    {
        _Lever = transform.GetChild(0).gameObject;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _Lever.SetActive(true);
        leverTransform.anchoredPosition = Vector2.zero;
        _TouchStartPosition = eventData.pressPosition;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = (eventData.position - _TouchStartPosition).normalized;
        leverTransform.anchoredPosition = direction * 90.0f;
        onDrag?.Invoke(direction);
    }    

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        _Lever.SetActive(false);
        onDrag?.Invoke(Vector2.zero);
    }    
    
    public void BindDragEvent(System.Action<Vector2> bindFunction)
    {
        onDrag += bindFunction;
    }

    public void UnbindDragEvent(System.Action<Vector2> unbindFunction)
    {
        onDrag += unbindFunction;
    }
}
