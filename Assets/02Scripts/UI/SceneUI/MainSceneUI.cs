using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUI : MonoBehaviour
{
    [Header("# ���� ���� UI �� Ȱ��ȭ��Ű�� ��ư")]
    public Button m_Button_OpenGameStartUI;

    [Header("# ���� ���� UI")]
    public BeforeGameStartUI m_BeforeGameStartUI;

    [Header("# ��ġ �ȳ� �ؽ�Ʈ")]
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
