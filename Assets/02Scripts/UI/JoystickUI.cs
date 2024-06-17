using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 조이스틱을 UI로 구현한 컴포넌트입니다.
/// 반드시 레버로 지정되어야하는 UI 오브젝트를 레버의 첫 번째 자식 객체로 설정하세요.
/// </summary>
public class JoystickUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    /// <summary>
    /// 레버에 해당하는 UI 오브젝트의 Transform
    /// </summary>
    protected Transform _Lever;

    /// <summary>
    /// 터치를 시작한 위치
    /// </summary>
    protected Vector2 _TouchStartPosition;

    /// <summary>
    /// 레버 객체의 RectTransform
    /// </summary>
    protected RectTransform leverTransform => _Lever as RectTransform;

    /// <summary>
    /// 드래그(레버 조작)시 호출될 이벤트
    /// </summary>
    public event System.Action<Vector2 /* 조이스틱 조작 방향*/> onDrag;

    protected virtual void Awake()
    {
        // 첫 번째 자식 객체를 레버로 지정합니다.
        _Lever = transform.GetChild(0);
    }
        
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _Lever.gameObject.SetActive(true);
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
        _Lever.gameObject.SetActive(false);
        onDrag?.Invoke(Vector2.zero);
    }    
    
    /// <summary>
    /// 드래그 이벤트에 바인딩하는 메서드입니다.
    /// </summary>
    /// <param name="bindFunction"> 이벤트에 바인딩할 함수</param>
    public void BindDragEvent(System.Action<Vector2> bindFunction)
    {
        onDrag += bindFunction;
    }

    /// <summary>
    /// 드래그 이벤트에 언바인딩하는 메서드입니다.
    /// </summary>
    /// <param name="unbindFunction"> 언바인딩할 함수</param>
    public void UnbindDragEvent(System.Action<Vector2> unbindFunction)
    {
        onDrag += unbindFunction;
    }
}
