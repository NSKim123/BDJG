using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 환경 설정 UI 컴포넌트입니다.
/// </summary>
public class ConfigurationUI : MonoBehaviour
{
    [Header("# 메인 화면 버튼")]
    public Button m_Button_MainScene;

    [Header("# 게임 종료 버튼")]
    public Button m_Button_QuitGame;

    [Header("# 취소 버튼")]
    public Button m_Button_Cancel;

    [Header("# 메인 화면 버튼 클릭 시 뜨는 UI")]
    public PanelWithTwoButtonsUI m_PopUpOnClickMainSceneButton;

    [Header("# 게임 종료 버튼 클릭 시 뜨는 UI")]
    public PanelWithTwoButtonsUI m_PopUpOnClickQuitGameButton;

    private void Awake()
    {
        // UI 들의 이벤트를 바인딩합니다.
        BindUIEvents();
    }

    /// <summary>
    /// UI 들의 이벤트를 바인딩하는 메서드입니다.
    /// </summary>
    private void BindUIEvents()
    {
        // 메인 화면 버튼 클릭 시 뜨는 UI의 이벤트를 바인딩합니다.
        BindPopUpOnClickMainSceneEvents();

        //게임 종료 버튼 클릭 시 뜨는 UI의 이벤트를 바인딩합니다.
        BindPopUpOnClickQuitGameButtonEvents();

        // 환경 설정 창의 버튼들의 이벤트를 바인딩합니다.
        BindButtonEvents();
    }

    /// <summary>
    /// 메인 화면 버튼 클릭 시 뜨는 UI의 이벤트를 바인딩하는 메서드입니다.
    /// </summary>
    private void BindPopUpOnClickMainSceneEvents()
    {
        // 메인 화면으로 돌아가는 이벤트 바인딩
        m_PopUpOnClickMainSceneButton.BindButton1Events(() => SceneManager.LoadScene(0));

        // 취소 버튼
        m_PopUpOnClickMainSceneButton.BindButton2Events(() => m_PopUpOnClickMainSceneButton.gameObject.SetActive(false));
    }

    /// <summary>
    /// 게임 종료 버튼 클릭 시 뜨는 UI의 이벤트를 바인딩하는 메서드입니다.
    /// </summary>
    private void BindPopUpOnClickQuitGameButtonEvents()
    {
        // 종료 버튼
        m_PopUpOnClickQuitGameButton.BindButton1Events(Application.Quit);

        // 취소 버튼
        m_PopUpOnClickQuitGameButton.BindButton2Events(() => m_PopUpOnClickQuitGameButton.gameObject.SetActive(false));
    }

    /// <summary>
    /// 환경 설정 창의 버튼들의 이벤트를 바인딩하는 메서드입니다.
    /// </summary>
    private void BindButtonEvents()
    {
        // 메인 화면 버튼 클릭 이벤트 <-- 바인딩 -- 메인 화면 버튼 클릭 시 떠야하는 UI 객체 활성화 함수
        m_Button_MainScene.onClick.AddListener(() => m_PopUpOnClickMainSceneButton.gameObject.SetActive(true));
        m_Button_MainScene.onClick.AddListener(PlayClickSound);

        // 게임 종료 버튼 클릭 이벤트 <-- 바인딩 -- 게임 종료 버튼 클릭 시 떠야하는 UI 객체 활성화 함수
        m_Button_QuitGame.onClick.AddListener(() => m_PopUpOnClickQuitGameButton.gameObject.SetActive(true));
        m_Button_QuitGame.onClick.AddListener(PlayClickSound);

        // 취소 버튼 클릭 이벤트 <-- 바인딩 -- 환경 설정 UI 비활성화 함수
        m_Button_Cancel.onClick.AddListener(() => gameObject.SetActive(false));
        m_Button_Cancel.onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        SoundManager.Instance.PlaySound(Constants.SOUNDNAME_CLICK_ABLEBUTTON, SoundType.Effect);
    }
}
