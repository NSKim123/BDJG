using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 플레이어의 레벨을 관리하는 클래스입니다.
/// </summary>
public class LevelSystem
{
    /// <summary>
    /// 현재 레벨
    /// </summary>
    private int _Level = 1;

    /// <summary>
    /// 처치 수 게이지
    /// </summary>
    private KillCountGauge _KillCountGauge = new(3);

    /// <summary>
    /// 생존 시간 게이지
    /// </summary>
    private SurvivalTimeGauge _SurvivalTimeGauge = new(20.0f);

    /// <summary>
    /// 레벨 업 시 호출되는 대리자입니다.
    /// </summary>
    public event System.Action<int/* 달성한 레벨 */> onLevelUp;

    /// <summary>
    /// 현재 레벨에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public int level => _Level;

    /// <summary>
    /// 레벨 업 조건을 확인하는 메서드입니다.
    /// </summary>
    /// <returns> 레벨 업 조건을 만족했다면 참을 반환합니다.</returns>
    private bool LevelUpCondition()
    {
        // 처치 수 조건과 생존 시간 조건이 모두 만족된다면 참을 반환합니다.
        //return _KillCountGauge.ratio >= 1.0f && _SurvivalTimeGauge.ratio >= 1.0f;
        return _SurvivalTimeGauge.ratio >= 0.5f;
    }

    /// <summary>
    /// 달성 레벨에 따라 목표 처치 수를 재설정하고, 처치 수를 0으로 초기화하는 메서드입니다.
    /// </summary>
    /// <param name="level"> 달성 레벨</param>
    private void RestartKillCount(int level)
    {
        int newTargetKillCount = 0;

        // 레벨에 따른 목표 처치 수를 설정합니다.
        switch(level)
        {
            case 1:
                newTargetKillCount = 3;
                break;
            case 2:
                newTargetKillCount = 5;
                break;
            case 3:
                newTargetKillCount = 10;
                break;
            default:
                newTargetKillCount = 0;
                break;
        }

        _KillCountGauge.max = newTargetKillCount;
        _KillCountGauge.currentValue = 0;
    }

    /// <summary>
    /// 달성 레벨에 따라 목표 생존 시간을 재설정하는 메서드입니다.
    /// </summary>
    /// <param name="level"> 달성 레벨</param>
    private void SetTargetSurvivalTime(int level)
    {
        float newTargetSurvivalTime = 0;

        // 레벨에 따른 목표 생존 시간을 설정합니다.
        switch (level)
        {
            case 1:
                newTargetSurvivalTime = 20.0f;
                break;
            case 2:
                newTargetSurvivalTime = 60.0f;
                break;
            case 3:
                newTargetSurvivalTime = 120.0f;
                break;
            default:
                newTargetSurvivalTime = 0.0f;
                break;
        }

        _SurvivalTimeGauge.max = newTargetSurvivalTime;
    }

    /// <summary>
    /// 레벨 업 시 호출되는 메서드입니다.
    /// </summary>
    /// <param name="newLevel"> 달성 레벨</param>
    private void LevelUp(int newLevel)
    {
        // 레벨을 증가시킵니다.
        _Level = newLevel;

        // 레벨 업 대리자를 호출합니다.
        onLevelUp?.Invoke(_Level);


        // 처치 수를 초기화하고, 목표 처치 수를 재설정합니다.
        RestartKillCount(_Level);

        // 목표 생존 시간을 재설정합니다.
        SetTargetSurvivalTime(_Level);
    }

    /// <summary>
    /// 레벨 시스템을 초기화하는 메서드입니다.
    /// </summary>
    /// <param name="level"> 초기화 레벨</param>
    public void Initailize(int level = 1)
    {
        // 1 레벨로 초기화합니다.
        _Level = 1;

        // 레벨 업 대리자를 호출합니다.
        onLevelUp?.Invoke(_Level);

        // 처치 수를 초기화하고, 목표 처치 수를 재설정합니다.
        RestartKillCount(_Level);

        // 목표 생존 시간을 재설정합니다.
        SetTargetSurvivalTime(_Level);
    }

    /// <summary>
    /// 처치 수를 증가시키는 메서드입니다.
    /// </summary>
    public void IncreaseKillCount()
    {
        ++_KillCountGauge.currentValue;
    }

    /// <summary>
    /// 생존 시간을 설정하는 메서드입니다.
    /// 레벨 업 조건을 확인합니다.
    /// </summary>
    /// <param name="newTime"> 설정 시간</param>
    public void UpdateSurvivalTime(float newTime)
    {
        // 생존 시간을 증가시킵니다.
        _SurvivalTimeGauge.currentValue = newTime;

        // 레벨 업 조건을 확인하고 레벨 업 시킵니다.
        if (LevelUpCondition())
        {
            LevelUp(_Level + 1);
        }
    }    

    public void OnDrawGizmos(Vector3 pos)
    {
        GUIContent gUIContent = new GUIContent();
        gUIContent.text = $"레벨 : {level}\n처치 수 / 레벨업 목표 처치수 : {_KillCountGauge.currentValue} / {_KillCountGauge.max}\n생존 시간 / 레벨업 목표 생존시간 : {_SurvivalTimeGauge.currentValue} / {_SurvivalTimeGauge.max}";
        Handles.Label(pos + Vector3.down, gUIContent);
    }
}

/// <summary>
/// 처치 수 게이지 클래스입니다.
/// </summary>
public class KillCountGauge : IntGauge
{
    /// <summary>
    /// 생성자 입니다.
    /// </summary>
    /// <param name="max"> 최대값</param>
    /// <param name="current"> 현재값</param>
    /// <param name="min"> 최소값</param>
    public KillCountGauge(int max, int current = 0, int min = 0) : base(max, current, min) { }

    /// <summary>
    /// 게이지 비율에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public override float ratio
    {
        get
        {
            // 최고 레벨 달성 시 max를 0으로 만들 것임.
            // 이 경우 비율 프로퍼티로 -1로 반환할 것임.
            // 이 비율 프로퍼티로 레벨 업 조건 달성 여부를 따지는데, 최고 레벨에서 레벨 업하는 상황이 방지하기 위함.
            if (max == 0)
                return -1.0f;

            return (float)currentValue / (float)max;
        }
    }    
}

/// <summary>
/// 생존 시간 게이지 클래스입니다.
/// </summary>
public class SurvivalTimeGauge : FloatGauge
{
    /// <summary>
    /// 생성자 입니다.
    /// </summary>
    /// <param name="max"> 최대값</param>
    /// <param name="current"> 현재값</param>
    /// <param name="min"> 최소값</param>
    public SurvivalTimeGauge(float max, float current = 0.0f, float min = 0.0f) : base(max, current, min) { }

    /// <summary>
    /// 현재 값에 대한 프로퍼티입니다.
    /// </summary>
    public override float currentValue
    {
        get { return _CurrentValue; }
        set
        {
            if (value < min)
                value = min;            

            _CurrentValue = value;
        }
    }

    /// <summary>
    /// 게이지 비율에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public override float ratio
    {
        get
        {
            if (max == 0)
                return -1.0f;

            return (float)currentValue / (float)max;
        }
    }
}

