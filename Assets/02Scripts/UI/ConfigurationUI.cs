using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationUI : MonoBehaviour
{
    [Header("# 메인 화면 버튼")]
    public Button m_Button_MainScene;

    [Header("# 게임 종료 버튼")]
    public Button m_Button_QuitGame;

    [Header("# 취소 버튼")]
    public Button m_Button_Cancel;

    [Header("# 메인 화면 버튼 클릭 시 뜨는 UI")]
    public PanelWithTwoButtonsUI m_UIOnClickMainSceneButton;

    [Header("# 게임 종료 버튼 클릭 시 뜨는 UI")]
    public PanelWithTwoButtonsUI m_UIOnClickQuitGameButton;

    private void BindMainSceneButtonOnClickUIEvents()
    {
        // TO DO : 메인 화면으로 돌아가는 이벤트 바인딩
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
