using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelSystem
{
    private int _Level = 1;

    private KillCountGauge _KillCountGauge = new(3);

    private SurvivalTimeGauge _SurvivalTimeGauge = new(20.0f);

    public System.Action<int/* 달성한 레벨 */> onLevelUp;

    public int level => _Level;

    private bool LevelUpCondition()
    {        
        return _KillCountGauge.ratio >= 1.0f && _SurvivalTimeGauge.ratio >= 1.0f;
    }

    private void InitializeKillCount(int level)
    {
        int newTargetKillCount = 0;

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

    private void SetTargetSurvivalTime(int level)
    {
        float newTargetSurvivalTime = 0;

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

    private void LevelUp()
    {
        ++_Level;

        onLevelUp?.Invoke(_Level);

        InitializeKillCount(_Level);
        SetTargetSurvivalTime(_Level);
    }

    public void Initailize()
    {
        onLevelUp?.Invoke(_Level);

        InitializeKillCount(_Level);
        SetTargetSurvivalTime(_Level);
    }

    public void IncreaseKillCount()
    {
        ++_KillCountGauge.currentValue;
    }

    public void UpdateSurvivalTime()
    {
        _SurvivalTimeGauge.currentValue += Time.deltaTime * Time.timeScale;

        if (LevelUpCondition())
        {
            LevelUp();
        }
    }    

    public void OnDrawGizmos(Vector3 pos)
    {
        GUIContent gUIContent = new GUIContent();
        gUIContent.text = $"레벨 : {level}\n처치 수 / 레벨업 목표 처치수 : {_KillCountGauge.currentValue} / {_KillCountGauge.max}\n생존 시간 / 레벨업 목표 생존시간 : {_SurvivalTimeGauge.currentValue} / {_SurvivalTimeGauge.max}";
        Handles.Label(pos + Vector3.down, gUIContent);
    }
}


public class KillCountGauge : IntGauge
{
    public KillCountGauge(int max, int current = 0, int min = 0) : base(max, current, min) { }

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

public class SurvivalTimeGauge : FloatGauge
{
    public SurvivalTimeGauge(float max, float current = 0.0f, float min = 0.0f) : base(max, current, min) { }

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

