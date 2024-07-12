using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUI : MonoBehaviour
{
    [Header("# 게임 시작 UI 를 활성화시키는 버튼")]
    public Button m_Button_OpenGameStartUI;

    [Header("# 게임 시작 UI")]
    public BeforeGameStartUI m_BeforeGameStartUI;

    [Header("# 터치 안내 텍스트")]
    public TMP_Text m_TouchHere;

    private void Awake()
    {
        BindUIEvents();
    }

    private void BindUIEvents()
    {
        m_Button_OpenGameStartUI.onClick.AddListener(ActiveBeforeGameStartUI);        
        m_Button_OpenGameStartUI.onClick.AddListener(PlayClickSound);

        m_BeforeGameStartUI.m_Button_Cancel.onClick.AddListener(() => m_TouchHere.gameObject.SetActive(true));

    }

    private void ActiveBeforeGameStartUI()
    {
        m_BeforeGameStartUI.gameObject.SetActive(true);
        m_TouchHere.gameObject.SetActive(false);
    }

    private void PlayClickSound()
    {
        SoundManager.Instance.PlaySound(Constants.SOUNDNAME_CLICK_ABLEBUTTON, SoundType.Effect);
    }
}
