using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ȯ�� ���� UI ������Ʈ�Դϴ�.
/// </summary>
public class ConfigurationUI : MonoBehaviour
{
    [Header("# ���� ȭ�� ��ư")]
    public Button m_Button_MainScene;

    [Header("# ���� ���� ��ư")]
    public Button m_Button_QuitGame;

    [Header("# ��� ��ư")]
    public Button m_Button_Cancel;

    [Header("# ���� ȭ�� ��ư Ŭ�� �� �ߴ� UI")]
    public PanelWithTwoButtonsUI m_PopUpOnClickMainSceneButton;

    [Header("# ���� ���� ��ư Ŭ�� �� �ߴ� UI")]
    public PanelWithTwoButtonsUI m_PopUpOnClickQuitGameButton;

    private void Awake()
    {
        // UI ���� �̺�Ʈ�� ���ε��մϴ�.
        BindUIEvents();
    }

    /// <summary>
    /// UI ���� �̺�Ʈ�� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    private void BindUIEvents()
    {
        // ���� ȭ�� ��ư Ŭ�� �� �ߴ� UI�� �̺�Ʈ�� ���ε��մϴ�.
        BindPopUpOnClickMainSceneEvents();

        //���� ���� ��ư Ŭ�� �� �ߴ� UI�� �̺�Ʈ�� ���ε��մϴ�.
        BindPopUpOnClickQuitGameButtonEvents();

        // ȯ�� ���� â�� ��ư���� �̺�Ʈ�� ���ε��մϴ�.
        BindButtonEvents();
    }

    /// <summary>
    /// ���� ȭ�� ��ư Ŭ�� �� �ߴ� UI�� �̺�Ʈ�� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    private void BindPopUpOnClickMainSceneEvents()
    {
        // ���� ȭ������ ���ư��� �̺�Ʈ ���ε�
        m_PopUpOnClickMainSceneButton.BindButton1Events(() => SceneManager.LoadScene(0));

        // ��� ��ư
        m_PopUpOnClickMainSceneButton.BindButton2Events(() => m_PopUpOnClickMainSceneButton.gameObject.SetActive(false));
    }

    /// <summary>
    /// ���� ���� ��ư Ŭ�� �� �ߴ� UI�� �̺�Ʈ�� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    private void BindPopUpOnClickQuitGameButtonEvents()
    {
        // ���� ��ư
        m_PopUpOnClickQuitGameButton.BindButton1Events(Application.Quit);

        // ��� ��ư
        m_PopUpOnClickQuitGameButton.BindButton2Events(() => m_PopUpOnClickQuitGameButton.gameObject.SetActive(false));
    }

    /// <summary>
    /// ȯ�� ���� â�� ��ư���� �̺�Ʈ�� ���ε��ϴ� �޼����Դϴ�.
    /// </summary>
    private void BindButtonEvents()
    {
        // ���� ȭ�� ��ư Ŭ�� �̺�Ʈ <-- ���ε� -- ���� ȭ�� ��ư Ŭ�� �� �����ϴ� UI ��ü Ȱ��ȭ �Լ�
        m_Button_MainScene.onClick.AddListener(() => m_PopUpOnClickMainSceneButton.gameObject.SetActive(true));
        m_Button_MainScene.onClick.AddListener(PlayClickSound);

        // ���� ���� ��ư Ŭ�� �̺�Ʈ <-- ���ε� -- ���� ���� ��ư Ŭ�� �� �����ϴ� UI ��ü Ȱ��ȭ �Լ�
        m_Button_QuitGame.onClick.AddListener(() => m_PopUpOnClickQuitGameButton.gameObject.SetActive(true));
        m_Button_QuitGame.onClick.AddListener(PlayClickSound);

        // ��� ��ư Ŭ�� �̺�Ʈ <-- ���ε� -- ȯ�� ���� UI ��Ȱ��ȭ �Լ�
        m_Button_Cancel.onClick.AddListener(() => gameObject.SetActive(false));
        m_Button_Cancel.onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        SoundManager.Instance.PlaySound(Constants.SOUNDNAME_CLICK_ABLEBUTTON, SoundType.Effect);
    }
}
