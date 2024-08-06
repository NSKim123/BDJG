using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ִϸ����͸� �����ϴ� ������Ʈ�Դϴ�.
/// </summary>
public class AnimController : MonoBehaviour
{
    /// <summary>
    /// �ִϸ����� ��ü
    /// </summary>
    protected Animator _Animator;
    
    /// <summary>
    /// �ִϸ����� ��ü�� ���� �б� ���� ������Ƽ�Դϴ�.
    /// </summary>
    public Animator animator => _Animator ?? (_Animator = GetComponent<Animator>());
    
    // �ĸ����� ���� �Լ���
    public void SetFloat(string paramName, float newValue) => animator.SetFloat(paramName, newValue);
    public void SetInt(string paramName, int newValue) => animator.SetInteger(paramName, newValue);
    public void SetBool(string paramName, bool newValue) => animator.SetBool(paramName, newValue);
    public void SetTrigger(string paramName) => animator.SetTrigger(paramName);

    // �Ķ���� ���� ������ �Լ���
    public float GetFloat(string paramName) => animator.GetFloat(paramName);
    public int GetInt(string paramName) => animator.GetInteger(paramName);
    public bool GetBool(string paramName) => animator.GetBool(paramName);

}
