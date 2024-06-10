using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 탄환 게이지를 나타내는 클래스입니다.
/// </summary>
public class BulletGauge : IntGauge
{
    /// <summary>
    /// 1회 게이지 회복량
    /// </summary>
    private int _BulletGaugeRecoverAmount = 2;

    /// <summary>
    /// 회복 주기
    /// </summary>
    private float _BulletGaugeRecoverCycle = 1.0f;

    /// <summary>
    /// 공격 후 게이지 회복을 시작하는 시간
    /// </summary>
    private float _BulletGaugeStartRecoverTime = 1.0f;

    /// <summary>
    /// 마지막 공격 시간
    /// </summary>
    private float _LastAttackTime;

    /// <summary>
    /// 마지막 회복 시간
    /// </summary>
    private float _LastRecoverTime;

    /// <summary>
    /// 과부하 상태인지 나타내는 논리형 변수
    /// </summary>
    private bool _IsOverburden;
    
    /// <summary>
    /// 과부하 상태인지를 나타내는 읽기 전용 프로퍼티입니다.
    /// </summary>
    public bool isOverburden => _IsOverburden;

    /// <summary>
    /// 생성자입니다.
    /// </summary>
    /// <param name="bulletGaugeRecoverAmount"> 1회 게이지 회복량</param>
    /// <param name="bulletGaugeRecoverCycle"> 회복 주기</param>
    /// <param name="bulletGaugeStartRecoverTime"> 공격 후 게이지 회복을 시작하는 시간</param>
    /// <param name="max"></param>
    public BulletGauge(int bulletGaugeRecoverAmount, float bulletGaugeRecoverCycle, float bulletGaugeStartRecoverTime, int max) : base(max)
    {
        _BulletGaugeRecoverAmount = bulletGaugeRecoverAmount;
        _BulletGaugeRecoverCycle = bulletGaugeRecoverCycle;
        _BulletGaugeStartRecoverTime = bulletGaugeStartRecoverTime;

        currentValue = max;
    }

    /// <summary>
    /// 탄환 게이지 회복 조건을 체크하는 메서드입니다.
    /// </summary>
    /// <returns> 회복 가능하다면 참을 반환합니다.</returns>
    private bool CheckRecoverCondition()
    {
        // 공격 버튼 상호 작용 후 1초간 상호작용이 없을 때, 1초당 한 번씩 회복하도록 설정하였습니다.
        return Time.time - _LastAttackTime >= _BulletGaugeStartRecoverTime
                  && Time.time - _LastRecoverTime >= _BulletGaugeRecoverCycle;
    }

    /// <summary>
    /// 탄환 게이지를 회복하는 메서드입니다.
    /// </summary>
    private void Recover()
    {
        // 탄환 게이지를 회복합니다.
        currentValue += _BulletGaugeRecoverAmount;

        // 회복 시간을 저장합니다.
        _LastRecoverTime = Time.time;
    }

    /// <summary>
    /// 과부하 상태를 해제하는 조건을 회복하는 메서드입니다.
    /// </summary>
    /// <returns> 과부하 상태 해제 조건을 만족하면 참을 반환합니다.</returns>
    private bool CheckLiftOverburdenCondition()
    {
        // 탄환 게이지가 모두 차있어야함을 조건으로 설정하였습니다.
        return max == currentValue;
    }

    /// <summary>
    /// 탄환 게이지를 갱신하는 메서드입니다.
    /// </summary>
    public void UpdateBulletGauge()
    {
        // 회복 조건을 체크하고 게이지를 회복합니다.
        if(CheckRecoverCondition())
        {
            Recover();
        }    
        
        // 과부하 상태라면, 과부하 상태 해제 조건을 체크하고 과부하 상태를 해제합니다.
        if(_IsOverburden && CheckLiftOverburdenCondition())
        {
            _IsOverburden = false;
        }
    }

    /// <summary>
    /// 탄환 게이지를 소모하는 메서드입니다.
    /// </summary>
    /// <param name="cost"> 소모량</param>
    public void CostBullet(int cost)
    {
        // 탄환 게이지를 소모합니다.
        currentValue -= cost;

        // 공격 시간을 저장합니다.
        _LastAttackTime = Time.time;

        // 탄환 게이지를 모두 소모했다면 과부하 상태로 돌입합니다.
        if(currentValue <= 0)
        {
            _IsOverburden = true;
        }
    }
}

