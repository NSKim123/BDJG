using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이터를 관리하는 컴포넌트입니다.
/// </summary>
public class AnimController : MonoBehaviour
{
    /// <summary>
    /// 애니메이터 객체
    /// </summary>
    protected Animator _Animator;
    
    /// <summary>
    /// 애니메이터 객체에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public Animator animator => _Animator ?? (_Animator = GetComponent<Animator>());
    
    // 파리미터 설정 함수들
    public void SetFloat(string paramName, float newValue) => animator.SetFloat(paramName, newValue);
    public void SetInt(string paramName, int newValue) => animator.SetInteger(paramName, newValue);
    public void SetBool(string paramName, bool newValue) => animator.SetBool(paramName, newValue);
    public void SetTrigger(string paramName) => animator.SetTrigger(paramName);

    // 파라미터 값을 얻어오는 함수들
    public float GetFloat(string paramName) => animator.GetFloat(paramName);
    public int GetInt(string paramName) => animator.GetInteger(paramName);
    public bool GetBool(string paramName) => animator.GetBool(paramName);

}
