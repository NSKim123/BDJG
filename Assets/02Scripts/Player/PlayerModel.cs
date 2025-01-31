using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 슬라임 모델 타입을 나타내는 열겨형식입니다.
/// </summary>
public enum SlimeModelType
{
    Type1 = 1,  // 기본 슬라임
    Type2,      // 추후 추가될 슬라임 타입
}

/// <summary>
/// 달성 레벨에 따른 레벨업 이펙트를 나타내는 클래스입니다.
/// </summary>
[Serializable]
public class LevelUpEffectInfo
{
    public int m_Level;
    public GameObject m_Effect;
}


/// <summary>
/// 캐릭터의 모델 및 스케일을 관리하는 컴포넌트입니다.
/// </summary>
/// 모델 관련 부분
public partial class PlayerModel : MonoBehaviour
{
    [Header("# 탄환 프래팹")]
    public Bullet m_Bullet;

    [Header("# 레벨 업 이펙트")]
    public List<LevelUpEffectInfo> m_LevelUpEffects;

    [Header("# 기관총 효과 시 이펙트")]
    public GameObject m_Effect_MachineGun;

    /// <summary>
    /// 로드할 PlayerModelScriptableObject 객체
    /// </summary>
    private PlayerModelScriptableObject _ModelScriptableObject;

    /// <summary>
    /// 슬라임 모델 타입
    /// </summary>
    private SlimeModelType _ModelType = SlimeModelType.Type1;

    /// <summary>
    /// 현재 모델
    /// </summary>
    private GameObject _CurrentModel;

    /// <summary>
    /// 복사 생성된 기관총 오로라 이펙트
    /// </summary>
    private GameObject _InstantiatedMachineGunEffect;

    /// <summary>
    /// 현재 모델에 대한 읽기 전용 프로퍼티입니다.
    /// </summary>
    public GameObject currentModel => _CurrentModel;

    /// <summary>
    /// 모델이 변경될 때 호출되는 대리자입니다.
    /// </summary>
    public event System.Action onModelChanged;

    private void Awake()
    {
        // 모델 스크립터블 오브젝트를 타입에 맞게 로드합니다.
        _ModelScriptableObject = Resources.Load<PlayerModelScriptableObject>("ScriptableObject/PlayerModelData/PlayerModelData_Type" + ((int)_ModelType).ToString());        
    }

    private void FixedUpdate()
    {
        // 크기를 갱신합니다.
        UpdateScale();
    }

    /// <summary>
    /// 새로운 모델로 교체하는 메서드입니다.
    /// </summary>
    /// <param name="newModel"> 새로운 모델 오브젝트</param>
    private void ChangeModel(GameObject newModel)
    {
        // 현재 모델을 제거합니다.
        if(_CurrentModel != null) 
            Destroy(_CurrentModel);

        // 새로운 모델을 생성하고 transform 을 설정합니다.
        GameObject newCharacter = Instantiate(newModel);
        newCharacter.transform.forward = transform.forward;
        newCharacter.transform.parent = transform;
        newCharacter.transform.localPosition = Vector3.down * 0.415f;
        newCharacter.transform.localScale = Vector3.one;

        // 현재 모델을 저장합니다.
        _CurrentModel = newCharacter;

        // 모델 변경 이벤트를 호출합니다.
        onModelChanged?.Invoke();

        // 탄환의 머터리얼도 바꿉니다.
        ObjectPoolManager.Instance.ChangeBulletMaterial(_CurrentModel.GetComponentInChildren<Renderer>().material);
    }

    /// <summary>
    /// 레벨업 이펙트를 생성하는 메서드입니다.
    /// </summary>
    /// <param name="level"> 달성 레벨</param>
    private void InstantiateLevelUpEffectByLevel(int level)
    {
        // 달성 레벨에 맞는 레벨업 이펙트 정보를 가져옵니다.
        LevelUpEffectInfo effectInfo = m_LevelUpEffects.Find((effectInfo) => effectInfo.m_Level == level);

        // 이펙트 정보가 없었거나, 이펙트가 등록되지 않은 경우 함수 호출 종료
        if (effectInfo == null || effectInfo.m_Effect == null) return;

        // 이펙트 생성
        GameObject effect = Instantiate(effectInfo.m_Effect);
        effect.transform.position = _CurrentModel.transform.position;
        effect.transform.SetParent(transform);
    }

    

    /// <summary>
    /// 레벨 업 시 호출되는 메서드입니다.
    /// </summary>
    /// <param name="level"> 달성 레벨</param>
    public void OnLevelUp(int level)
    {
        // 새로운 모델을 불러옵니다.
        GameObject newModel = _ModelScriptableObject.FindModelByLevel(level);

        // 모델을 교체합니다.
        ChangeModel(newModel);

        // 레벨업 이펙트 생성
        InstantiateLevelUpEffectByLevel(level);
    }
}


/// <summary>
/// 스케일 관련 부분
/// </summary>
public partial class PlayerModel
{
    [Header("# 스케일 관련")]
    [Header("최소 크기")]
    public float m_MinScale = 1.0f;

    [Header("크기 배율(남은 탄환 수에 따른 크기 증가량)")]
    public float m_ScaleCoefficient = 0.1f;

    /// <summary>
    /// 목표 스케일입니다.
    /// </summary>
    private float _TargetScale;

    /// <summary>
    /// 크기를 갱신하는 메서드입니다.
    /// </summary>
    private void UpdateScale()
    {
        // 목표 크기로 부드럽게 변화시킵니다.
        Vector3 newScale = Vector3.MoveTowards(transform.localScale, _TargetScale * Vector3.one, 50.0f * Time.fixedDeltaTime);
        transform.localScale = newScale;
    }

    /// <summary>
    /// 남은 탄환 수에 따른 크기를 계산하는 메서드입니다.
    /// </summary>
    /// <param name="bullets"> 남은 탄환 수</param>
    /// <returns> 크기를 반환합니다.</returns>
    private float CalculateScaleByBulletGauge(int bullets)
    {
        return m_MinScale + 0.1f * bullets;
    }

    /// <summary>
    /// 남은 탄환 수에 따른 목표 크기를 설정하는 메서드입니다.
    /// </summary>
    /// <param name="bullets"> 남은 탄환 수</param>
    public void UpdateTargetScale(int bullets)
    {
        _TargetScale = CalculateScaleByBulletGauge(bullets);
    }
}


/// <summary>
/// 아이템 효과 관련 부분
/// </summary>
public partial class PlayerModel
{
    /// <summary>
    /// 기관총 오로라 이펙트를 생성하는 메서드입니다.
    /// </summary>
    private void InstantiateMachineGunEffect()
    {
        _InstantiatedMachineGunEffect = Instantiate(m_Effect_MachineGun);

        _InstantiatedMachineGunEffect.transform.position = transform.position;
    }

    /// <summary>
    /// 기관총 오로라 이펙트의 loop를 해제함으로써 이펙트를 사라지게 합니다.
    /// </summary>
    private void FinishLoopOfMachineGunEffect()
    {
        ParticleSystem.MainModule newMain;

        // loop 해제
        foreach (ParticleSystem particleSystem in _InstantiatedMachineGunEffect.GetComponentsInChildren<ParticleSystem>())
        {
            newMain = particleSystem.main;
            newMain.loop = false;
        }

        _InstantiatedMachineGunEffect = null;
    }

    /// <summary>
    /// 기관총 아이템 효과 시작 시의 동작을 나타낸 메서드입니다.
    /// </summary>
    public void OnStartMachineGun()
    {
        // 기관총 오로라 이펙트 생성
        InstantiateMachineGunEffect();
    }

    /// <summary>
    /// 기관총 아이템 효과 종료 시의 동작을 나타낸 메서드입니다.
    /// </summary>
    public void OnFinishMachineGun()
    {
        // 기관총 오로라 이펙트 제거
        FinishLoopOfMachineGunEffect();
    }
}



