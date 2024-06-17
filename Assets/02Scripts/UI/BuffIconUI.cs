using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 한 버프 아이콘을 나타내는 컴포넌트입니다.
/// </summary>
public class BuffIconUI : MonoBehaviour 
{
    /// <summary>
    /// 버프 아이콘 스크립터블 오브젝트 객체
    /// </summary>
    private static BuffIconScriptableObject _BuffIcons;    

    [Header("# 버프 아이콘")]
    public Image m_BuffIcon;

    /// <summary>
    /// 이 UI가 나타내는 버프의 코드
    /// </summary>
    private int _BuffCode;

    /// <summary>
    /// 아이콘 점멸 조건을 만족하는 지를 나타내는 변수
    /// </summary>
    private bool _Blink = false;

    private void Awake()
    {
        // 모든 버프 아이콘들을 불러옵니다.
        LoadBuffIcons();
    }

    private void Update()
    {
        // 아이콘 점멸 조건을 만족한다면
        if (_Blink)
        {
            // 아이콘을 깜빡거립니다.
            Blink();
        }
    }

    /// <summary>
    /// 모든 버프 아이콘들을 불러오는 메서드입니다.
    /// </summary>
    private void LoadBuffIcons()
    {
        if (_BuffIcons == null)
        {
            _BuffIcons = Resources.Load<BuffIconScriptableObject>("ScriptableObject/BuffIcons/BuffIconScriptableObject");
        }
    }

    /// <summary>
    /// 아이콘을 깜빡거리는 메서드입니다.
    /// </summary>
    private void Blink()
    {
        Color newColor = m_BuffIcon.color;
        newColor.a = (Mathf.Sin(Time.time * 6.0f) + 1.0f) / 2.0f;
        m_BuffIcon.color = newColor;
    }

    /// <summary>
    /// 이 버프 아이콘 객체를 시작하는 메서드입니다.
    /// </summary>
    /// <param name="newBuff"> 설정할 버프</param>
    public void InitIcon(Buff newBuff)
    {
        // 버프 코드 등록
        _BuffCode = newBuff.buffCode;

        // 버프 아이콘 설정
        m_BuffIcon.sprite = _BuffIcons.GetBuffIconByBuffCode(newBuff.buffCode);        
    }

    /// <summary>
    /// 전달된 버프 코드와 이 객체가 나타내고 있는 버프의 코드와 동일한 지를 체크하는 메서드입니다.
    /// </summary>
    /// <param name="buffCode"> 비교할 버프 코드</param>
    /// <returns> 동일하면 true, 아니면 false</returns>
    public bool CheckIconUIByBuffCode(int buffCode)
    {
        return _BuffCode == buffCode;
    }

    /// <summary>
    /// 아이콘 점멸을 시작합니다.
    /// </summary>
    public void StartBlink()
    {
        _Blink = true;
    }

    /// <summary>
    /// 아이콘 점멸을 종료합니다.
    /// </summary>
    public void StopBlink()
    {
        _Blink = false;
        m_BuffIcon.color = Color.white;
    }
}

