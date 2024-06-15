using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconUI : MonoBehaviour 
{
    private static BuffIconScriptableObject _BuffIcons;

    private void Awake()
    {
        if (_BuffIcons == null)
        {
            _BuffIcons = Resources.Load<BuffIconScriptableObject>("ScriptableObject/BuffIcons/BuffIconScriptableObject");
        }
    }

    [Header("# 버프 아이콘")]
    public Image m_BuffIcon;

    private int _BuffCode;

    private bool _Blink = false;

    private void Update()
    {
        if (_Blink)
        {
            Color newColor = m_BuffIcon.color;
            newColor.a = (Mathf.Sin(Time.time * 6.0f) + 1.0f) / 2.0f;
            m_BuffIcon.color = newColor;
        }
    }

    public void InitIcon(Buff newBuff)
    {
        _BuffCode = newBuff.buffCode;
        m_BuffIcon.sprite = _BuffIcons.GetBuffIconByBuffCode(newBuff.buffCode);        
    }

    public bool CheckIconUIByBuffCode(int buffCode)
    {
        return _BuffCode == buffCode;
    }

    public void StartBlink()
    {
        _Blink = true;
    }

    public void StopBlink()
    {
        _Blink = false;
        m_BuffIcon.color = Color.white;
    }
}

