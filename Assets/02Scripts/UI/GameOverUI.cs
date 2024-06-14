using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : PanelWithTwoButtonsUI
{
    [Header("# 게임 결과 텍스트")]
    [Header("# 생존 시간")]
    public TMP_Text m_Text_SurvivalTimeResult;

    [Header("# 점수")]
    public TMP_Text m_Text_ScoreResult;

    public void OnGameOver(float survivalTimeResult, int scoreResult)
    {   
        int minute = (int)(survivalTimeResult / 60.0f);
        int second = (int)(survivalTimeResult - (int)(survivalTimeResult / 60.0f) * 60.0f);

        m_Text_SurvivalTimeResult.text = $"{minute} : " + (second < 10 ? "0" : "") + second;

        m_Text_ScoreResult.text = scoreResult.ToString();
    }
}
