using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationUI : MonoBehaviour
{
    [Header("# ���� ȭ�� ��ư")]
    public Button m_Button_MainScene;

    [Header("# ���� ���� ��ư")]
    public Button m_Button_QuitGame;

    [Header("# ��� ��ư")]
    public Button m_Button_Cancel;

    [Header("# ���� ȭ�� ��ư Ŭ�� �� �ߴ� UI")]
    public PanelWithTwoButtonsUI m_UIOnClickMainSceneButton;

    [Header("# ���� ���� ��ư Ŭ�� �� �ߴ� UI")]
    public PanelWithTwoButtonsUI m_UIOnClickQuitGameButton;

    private void BindMainSceneButtonOnClickUIEvents()
    {
        // TO DO : ���� ȭ������ ���ư��� �̺�Ʈ ���ε�
        // m_UIOnClickMainSceneButton.BindButton1Events();
        m_UIOnClickMainSceneButton.BindButton2Events(() => m_UIOnClickMainSceneButton.gameObject.SetActive(false));
    }

    private void BindQuitGameButtonOnClickUIEvents()
    {
        m_UIOnClickQuitGameButton.BindButton1Events(Application.Quit);
        m_UIOnClickQuitGameButton.BindButton2Events(() => m_UIOnClickQuitGameButton.gameObject.SetActive(false));
    }

    public void BindUIEvents()
    {
        BindMainSceneButtonOnClickUIEvents();
        BindQuitGameButtonOnClickUIEvents();

        m_Button_MainScene.onClick.AddListener(() => m_UIOnClickMainSceneButton.gameObject.SetActive(true));

        m_Button_QuitGame.onClick.AddListener(() => m_UIOnClickQuitGameButton.gameObject.SetActive(true));

        m_Button_Cancel.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
