using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 오버 시 활성화 될 UI 컴포넌트입니다.
/// </summary>
public class GameOverUI : PanelWithTwoButtonsUI
{
    [Header("# 게임 결과 텍스트")]
    [Header("# 생존 시간")]
    public TMP_Text m_Text_SurvivalTimeResult;

    [Header("# 점수")]
    public TMP_Text m_Text_ScoreResult;

    /// <summary>
    /// 버튼들의 클릭 이벤트에 함수를 바인딩을 진행하는 메서드입니다.
    /// 첫 번째 버튼(게임 다시 진행하기)는 GameSceneInstance 에서 바인딩하세요.
    /// </summary>
    protected override void BindButtonsEvents()
    {
        // 메인 화면 버튼 클릭 이벤트 바인딩
        BindButton2Events(() => SceneManager.LoadScene(0));
    }

    /// <summary>
    /// 게임 오버 시 호출될 메서드입니다.
    /// </summary>
    /// <param name="survivalTimeResult"> 생존 시간 결과</param>
    /// <param name="scoreResult"> 점수 결과</param>
    public void OnGameOver(float survivalTimeResult, int scoreResult)
    {   
        // 생존 시간 결과 텍스트 설정
        int minute = (int)(survivalTimeResult / 60.0f);
        int second = (int)(survivalTimeResult - (int)(survivalTimeResult / 60.0f) * 60.0f);
        m_Text_SurvivalTimeResult.text = $"{minute} : " + (second < 10 ? "0" : "") + second;

        // 점수 결과 텍스트 설정
        m_Text_ScoreResult.text = scoreResult.ToString();
    }
}
