using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���� ���� �� Ȱ��ȭ �� UI ������Ʈ�Դϴ�.
/// </summary>
public class GameOverUI : PanelWithTwoButtonsUI
{
    [Header("# ���� ��� �ؽ�Ʈ")]
    [Header("# ���� �ð�")]
    public TMP_Text m_Text_SurvivalTimeResult;

    [Header("# ����")]
    public TMP_Text m_Text_ScoreResult;

    /// <summary>
    /// ��ư���� Ŭ�� �̺�Ʈ�� �Լ��� ���ε��� �����ϴ� �޼����Դϴ�.
    /// ù ��° ��ư(���� �ٽ� �����ϱ�)�� GameSceneInstance ���� ���ε��ϼ���.
    /// </summary>
    protected override void BindButtonsEvents()
    {
        // ���� ȭ�� ��ư Ŭ�� �̺�Ʈ ���ε�
        BindButton2Events(() => SceneManager.LoadScene(0));
    }

    /// <summary>
    /// ���� ���� �� ȣ��� �޼����Դϴ�.
    /// </summary>
    /// <param name="survivalTimeResult"> ���� �ð� ���</param>
    /// <param name="scoreResult"> ���� ���</param>
    public void OnGameOver(float survivalTimeResult, int scoreResult)
    {   
        // ���� �ð� ��� �ؽ�Ʈ ����
        int minute = (int)(survivalTimeResult / 60.0f);
        int second = (int)(survivalTimeResult - (int)(survivalTimeResult / 60.0f) * 60.0f);
        m_Text_SurvivalTimeResult.text = $"{minute} : " + (second < 10 ? "0" : "") + second;

        // ���� ��� �ؽ�Ʈ ����
        m_Text_ScoreResult.text = scoreResult.ToString();
    }
}
