using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 타겟팅을 담당하는 클래스입니다.
/// </summary>
public class TargetingSystem
{
    /// <summary>
    /// 타겟팅 대상에게 적용시킬 외곽선 머터리얼
    /// </summary>
    private static Material _OutLineMaterial;

    /// <summary>
    /// 감지 최대 길이
    /// </summary>
    private float _SencerDistance = 20.0f;

    /// <summary>
    /// 감지 폭
    /// </summary>
    private float _SencerWidth = 2.0f;

    /// <summary>
    /// 감지 높이
    /// </summary>
    private float _SencerHeight = 2.0f;

    /// <summary>
    /// 감지 레이어
    /// </summary>
    private LayerMask _SenceLayer;

    /// <summary>
    /// 현재 타겟팅 중인 대상의 Transform
    /// </summary>
    private Transform _CurrentTargetTransform;

    /// <summary>
    /// 현재 타겟팅 중인 대상의 Transform 에 대한 읽기 전용 프로퍼티
    /// </summary>
    public Transform currentTargetTransform => _CurrentTargetTransform;

    /// <summary>
    /// 생성자 입니다
    /// </summary>
    /// <param name="senceLayer"> 감지할 레이어</param>
    /// <param name="senceDistance"> 감지 거리</param>
    /// <param name="senceWidth"> 감지 폭</param>
    /// <param name="senceHeight"> 감지 높이</param>
    public TargetingSystem(LayerMask senceLayer, float senceDistance, float senceWidth, float senceHeight)
    {
        if (_OutLineMaterial == null)
            _OutLineMaterial = Resources.Load<Material>("Materials/M_TargetingOuterLine");

        _SencerDistance = senceDistance;
        _SencerWidth = senceWidth;
        _SencerHeight = senceHeight;
        _SenceLayer = senceLayer;
    }

    /// <summary>
    /// 타겟팅 대상에게 외곽선을 추가하는 메서드입니다.
    /// </summary>
    private void AddOuterLine()
    {
        List<Material> materials = _CurrentTargetTransform.GetComponentInChildren<Renderer>().materials.ToList();
        materials.Add(GameObject.Instantiate(_OutLineMaterial));
        _CurrentTargetTransform.GetComponentInChildren<Renderer>().SetMaterials(materials);        
    }

    /// <summary>
    /// 타겟팅에서 제외된 대상의 외곽선을 제거하는 메서드입니다.
    /// </summary>
    private void RemoveOuterLine()
    {
        List<Material> materials = _CurrentTargetTransform.GetComponentInChildren<Renderer>().materials.ToList();
        materials.Remove(materials.Find((mat) => mat.name == _OutLineMaterial.name + "(Clone) (Instance)"));
        _CurrentTargetTransform.GetComponentInChildren<Renderer>().SetMaterials(materials);
    }

    /// <summary>
    /// 감지 대상이 없을 경우 호출되는 메서드입니다.
    /// </summary>
    private void NoTarget()
    {
        // 이전에도 타겟팅 중인 대상이 없었다면 호출을 종료합니다.
        if(_CurrentTargetTransform == null) return;
                
        // 이전에 타겟팅한 대상의 외곽선을 제거합니다.
        RemoveOuterLine();

        // 타겟팅 대상을 저장하지 않습니다.
        _CurrentTargetTransform = null;
    }

    /// <summary>
    /// 타겟팅 대상을 교체하는 메서드입니다.
    /// </summary>
    /// <param name="newTarget"> 새로운 타겟팅 대상의 Transform </param>
    private void ChangeTargeting(Transform newTarget)
    {
        // 이전에 타겟팅한 대상의 외곽선을 제거합니다. 
        if (_CurrentTargetTransform != null)
            RemoveOuterLine();

        // 새로운 타겟팅 대상을 저장합니다.
        _CurrentTargetTransform = newTarget;

        // 새로운 대상에게 외곽선을 추가합니다.
        AddOuterLine();
    }

    /// <summary>
    /// 전방 일정 범위를 감지하여 감지한 모든 충돌체를 모두 반환하는 메서드입니다.
    /// </summary>
    /// <param name="currentTransform"> 이 타겟팅 시스템를 소유한 오브젝트의 Transform</param>
    /// <returns> 감지한 모든 충돌체를 반환합니다.</returns>
    private Collider[] Sencing(Transform currentTransform)
    {
        // 감지 범위 설정
        Vector3 center = currentTransform.position + _SencerDistance * 0.5f * currentTransform.forward;
        Vector3 half = new Vector3(_SencerWidth, _SencerHeight, _SencerDistance) * 0.5f;
        Quaternion quaternion = currentTransform.rotation;

        // 감지 결과를 반환합니다.
        return Physics.OverlapBox(center, half, quaternion, _SenceLayer);
    }

    /// <summary>
    /// 감지된 모든 충돌체 중 
    /// '시작점이 소유주의 위치이고 방향 벡터가 소유주의 전방 벡터인 직선'에 
    /// 가장 가까운 적을 찾는 메서드입니다.
    /// </summary>
    /// <param name="senceResults"> 감지한 모든 충돌체들.</param>
    /// <param name="ownerTransform"> 이 타겟팅 시스템를 소유한 오브젝트의 Transform</param>
    /// <returns> '시작점이 소유주의 위치이고 방향 벡터가 소유주의 전방 벡터인 직선'에 가장 가까운 적을 반환합니다.
    /// 만약 senceResults 매개변수에 빈 배열을 전달했을 경우 null 을 반환합니다.</returns>
    private Transform FindClosestTarget(Collider[] senceResults, Transform ownerTransform)
    {
        // 감지된 대상이 없는 경우
        if (senceResults.Length == 0) return null;
                
        Collider closestTarget = senceResults[0];
        float MinDistanceToForwardLine = 100.0f;
        float closestTargetDistanceSqr = (ownerTransform.position - closestTarget.transform.position).sqrMagnitude;

        // '시작점이 소유주의 위치이고 방향 벡터가 소유주의 전방 벡터인 직선'에 가장 가까운 적을 찾습니다.
        foreach (Collider senceResult in senceResults)
        {
            // 소유주 위치에서 감지된 대상의 위치를 향하는 벡터
            Vector3 ownerPosToSenceResultPos = senceResult.transform.position - ownerTransform.position;

            // 감지된 대상의 위치에서
            // '시작점이 소유주의 위치이고 방향 벡터가 소유주의 전방 벡터인 직선'까지의 거리를 구합니다.
            float distanceToForwardLine =
                Mathf.Sqrt(
                    ownerPosToSenceResultPos.sqrMagnitude -
                        Vector3.Dot(ownerPosToSenceResultPos, ownerTransform.forward) * Vector3.Dot(ownerPosToSenceResultPos, ownerTransform.forward));



            // 최소값을 걸러내는 과정
            bool isMinimum = false;

            if (distanceToForwardLine < MinDistanceToForwardLine)
            {
                isMinimum = true;
            }
            else if(distanceToForwardLine == MinDistanceToForwardLine)
            {
                if (ownerPosToSenceResultPos.sqrMagnitude < closestTargetDistanceSqr)
                {
                    isMinimum = true;
                }
                else if(ownerPosToSenceResultPos.sqrMagnitude == closestTargetDistanceSqr)
                {
                    int random = UnityEngine.Random.Range(0, 2);

                    if(random == 0)
                    {
                        isMinimum = true;
                    }
                }
            }

            if(isMinimum)
            {
                closestTarget = senceResult;
                MinDistanceToForwardLine = distanceToForwardLine;
                closestTargetDistanceSqr = ownerPosToSenceResultPos.sqrMagnitude;
            }
        }

        // 타겟팅 조건에 부합하는 대상의 Transform을 반환합니다.
        return closestTarget.transform;
    }

    /// <summary>
    /// 타겟팅의 과정을 수행하는 메서드입니다.
    /// </summary>
    /// <param name="ownerTransform"> 소유주의 Transform</param>
    public void Targeting(Transform ownerTransform)
    {
        // 전방 일정 범위를 감지합니다.
        Collider[] SenceResults = Sencing(ownerTransform);

        // 감지된 대상들 중 조건에 부합하는 대상을 찾습니다.
        Transform targetingResult = FindClosestTarget(SenceResults, ownerTransform);

        // 자식 오브젝트에 CenterTransform 오브젝트가 있다면 이를 타겟팅 대상으로 설정합니다.
        Transform centerTransform = targetingResult?.transform.Find("CenterTransform");
        if (centerTransform != null)
        {
            targetingResult = centerTransform;
        }

        // 감지된 대상 조차 없는 경우
        if(targetingResult == null)
        {
            NoTarget();
        }
        // 이전에 타겟팅했던 대상과 지금 타겟팅할 대상이 다른 경우
        else if(_CurrentTargetTransform != targetingResult)
        {
            ChangeTargeting(targetingResult);
        }
    }
}
