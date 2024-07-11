using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeforeGameStartUI : MonoBehaviour
{
    [Header("# ���� ���� ��ư")]
    public Button m_Button_GameStart;

    [Header("# ���� ���� ��ư")]
    public Button m_Button_QuitGame;

    [Header("# ��� ��ư")]
    public Button m_Button_Cancel;

    private void Awake()
    {
        BindUIEvents();
    }

    private void BindUIEvents()
    {
        m_Button_GameStart.onClick.AddListener(MoveToGameScene);

        m_Button_QuitGame.onClick.AddListener(Application.Quit);

        m_Button_Cancel.onClick.AddListener(() => this.gameObject.SetActive(false));
    }

    private void MoveToGameScene()
    {
        GameData.Instance.m_NextSceneName = Constants.SCENENAME_GAMESCENE;
        SceneManager.LoadScene(Constants.SCENENAME_LOADINGSCENE);
    }
}
