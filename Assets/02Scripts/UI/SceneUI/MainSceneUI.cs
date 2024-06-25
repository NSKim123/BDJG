using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUI : MonoBehaviour
{
    [Header("# 게임 시작 UI 를 활성화시키는 버튼")]
    public Button m_Button_OpenGameStartUI;

    [Header("# 게임 시작 UI")]
    public BeforeGameStartUI m_BeforeGameStartUI;

    private void Awake()
    {
        BindUIEvents();
    }

    private void BindUIEvents()
    {
        m_Button_OpenGameStartUI.onClick.AddListener(() => m_BeforeGameStartUI.gameObject.SetActive(true));
        m_Button_OpenGameStartUI.onClick.AddListener(() => m_Button_OpenGameStartUI.gameObject.SetActive(false));

        m_BeforeGameStartUI.m_Button_Cancel.onClick.AddListener(() => m_Button_OpenGameStartUI.gameObject.SetActive(false));
    }
}
