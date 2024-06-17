using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ���̽�ƽ�� UI�� ������ ������Ʈ�Դϴ�.
/// �ݵ�� ������ �����Ǿ���ϴ� UI ������Ʈ�� ������ ù ��° �ڽ� ��ü�� �����ϼ���.
/// </summary>
public class JoystickUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    /// <summary>
    /// ������ �ش��ϴ� UI ������Ʈ�� Transform
    /// </summary>
    protected Transform _Lever;

    /// <summary>
    /// ��ġ�� ������ ��ġ
    /// </summary>
    protected Vector2 _TouchStartPosition;

    /// <summary>
    /// ���� ��ü�� RectTransform
    /// </summary>
    protected RectTransform leverTransform => _Lever as RectTransform;

    /// <summary>
    /// �巡��(���� ����)�� ȣ��� �̺�Ʈ
    /// </summary>
    public event System.Action<Vector2 /* ���̽�ƽ ���� ����*/> onDrag;

    protected virtual void Awake()
    {
        // ù ��° �ڽ� ��ü�� ������ �����մϴ�.
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
    /// �巡�� �̺�Ʈ�� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="bindFunction"> �̺�Ʈ�� ���ε��� �Լ�</param>
    public void BindDragEvent(System.Action<Vector2> bindFunction)
    {
        onDrag += bindFunction;
    }

    /// <summary>
    /// �巡�� �̺�Ʈ�� ����ε��ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="unbindFunction"> ����ε��� �Լ�</param>
    public void UnbindDragEvent(System.Action<Vector2> unbindFunction)
    {
        onDrag += unbindFunction;
    }
}
