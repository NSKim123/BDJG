using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Ÿ������ ����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class TargetingSystem
{
    /// <summary>
    /// Ÿ���� ��󿡰� �����ų �ܰ��� ���͸���
    /// </summary>
    private static Material _OutLineMaterial;

    /// <summary>
    /// ���� �ִ� ����
    /// </summary>
    private float _SencerDistance = 20.0f;

    /// <summary>
    /// ���� ��
    /// </summary>
    private float _SencerWidth = 2.0f;

    /// <summary>
    /// ���� ����
    /// </summary>
    private float _SencerHeight = 2.0f;

    /// <summary>
    /// ���� ���̾�
    /// </summary>
    private LayerMask _SenceLayer;

    /// <summary>
    /// ���� Ÿ���� ���� ����� Transform
    /// </summary>
    private Transform _CurrentTargetTransform;

    /// <summary>
    /// ���� Ÿ���� ���� ����� Transform �� ���� �б� ���� ������Ƽ
    /// </summary>
    public Transform currentTargetTransform => _CurrentTargetTransform;

    /// <summary>
    /// ������ �Դϴ�
    /// </summary>
    /// <param name="senceLayer"> ������ ���̾�</param>
    /// <param name="senceDistance"> ���� �Ÿ�</param>
    /// <param name="senceWidth"> ���� ��</param>
    /// <param name="senceHeight"> ���� ����</param>
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
    /// Ÿ���� ��󿡰� �ܰ����� �߰��ϴ� �޼����Դϴ�.
    /// </summary>
    private void AddOuterLine()
    {
        List<Material> materials = _CurrentTargetTransform.GetComponentInChildren<Renderer>().materials.ToList();
        materials.Add(GameObject.Instantiate(_OutLineMaterial));
        _CurrentTargetTransform.GetComponentInChildren<Renderer>().SetMaterials(materials);        
    }

    /// <summary>
    /// Ÿ���ÿ��� ���ܵ� ����� �ܰ����� �����ϴ� �޼����Դϴ�.
    /// </summary>
    private void RemoveOuterLine()
    {
        List<Material> materials = _CurrentTargetTransform.GetComponentInChildren<Renderer>().materials.ToList();
        materials.Remove(materials.Find((mat) => mat.name == _OutLineMaterial.name + "(Clone) (Instance)"));
        _CurrentTargetTransform.GetComponentInChildren<Renderer>().SetMaterials(materials);
    }

    /// <summary>
    /// ���� ����� ���� ��� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    private void NoTarget()
    {
        // �������� Ÿ���� ���� ����� �����ٸ� ȣ���� �����մϴ�.
        if(_CurrentTargetTransform == null) return;
                
        // ������ Ÿ������ ����� �ܰ����� �����մϴ�.
        RemoveOuterLine();

        // Ÿ���� ����� �������� �ʽ��ϴ�.
        _CurrentTargetTransform = null;
    }

    /// <summary>
    /// Ÿ���� ����� ��ü�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="newTarget"> ���ο� Ÿ���� ����� Transform </param>
    private void ChangeTargeting(Transform newTarget)
    {
        // ������ Ÿ������ ����� �ܰ����� �����մϴ�. 
        if (_CurrentTargetTransform != null)
            RemoveOuterLine();

        // ���ο� Ÿ���� ����� �����մϴ�.
        _CurrentTargetTransform = newTarget;

        // ���ο� ��󿡰� �ܰ����� �߰��մϴ�.
        AddOuterLine();
    }

    /// <summary>
    /// ���� ���� ������ �����Ͽ� ������ ��� �浹ü�� ��� ��ȯ�ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="currentTransform"> �� Ÿ���� �ý��۸� ������ ������Ʈ�� Transform</param>
    /// <returns> ������ ��� �浹ü�� ��ȯ�մϴ�.</returns>
    private Collider[] Sencing(Transform currentTransform)
    {
        // ���� ���� ����
        Vector3 center = currentTransform.position + _SencerDistance * 0.5f * currentTransform.forward;
        Vector3 half = new Vector3(_SencerWidth, _SencerHeight, _SencerDistance) * 0.5f;
        Quaternion quaternion = currentTransform.rotation;

        // ���� ����� ��ȯ�մϴ�.
        return Physics.OverlapBox(center, half, quaternion, _SenceLayer);
    }

    /// <summary>
    /// ������ ��� �浹ü �� 
    /// '�������� �������� ��ġ�̰� ���� ���Ͱ� �������� ���� ������ ����'�� 
    /// ���� ����� ���� ã�� �޼����Դϴ�.
    /// </summary>
    /// <param name="senceResults"> ������ ��� �浹ü��.</param>
    /// <param name="ownerTransform"> �� Ÿ���� �ý��۸� ������ ������Ʈ�� Transform</param>
    /// <returns> '�������� �������� ��ġ�̰� ���� ���Ͱ� �������� ���� ������ ����'�� ���� ����� ���� ��ȯ�մϴ�.
    /// ���� senceResults �Ű������� �� �迭�� �������� ��� null �� ��ȯ�մϴ�.</returns>
    private Transform FindClosestTarget(Collider[] senceResults, Transform ownerTransform)
    {
        // ������ ����� ���� ���
        if (senceResults.Length == 0) return null;
                
        Collider closestTarget = senceResults[0];
        float MinDistanceToForwardLine = 100.0f;
        float closestTargetDistanceSqr = (ownerTransform.position - closestTarget.transform.position).sqrMagnitude;

        // '�������� �������� ��ġ�̰� ���� ���Ͱ� �������� ���� ������ ����'�� ���� ����� ���� ã���ϴ�.
        foreach (Collider senceResult in senceResults)
        {
            // ������ ��ġ���� ������ ����� ��ġ�� ���ϴ� ����
            Vector3 ownerPosToSenceResultPos = senceResult.transform.position - ownerTransform.position;

            // ������ ����� ��ġ����
            // '�������� �������� ��ġ�̰� ���� ���Ͱ� �������� ���� ������ ����'������ �Ÿ��� ���մϴ�.
            float distanceToForwardLine =
                Mathf.Sqrt(
                    ownerPosToSenceResultPos.sqrMagnitude -
                        Vector3.Dot(ownerPosToSenceResultPos, ownerTransform.forward) * Vector3.Dot(ownerPosToSenceResultPos, ownerTransform.forward));



            // �ּҰ��� �ɷ����� ����
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

        // Ÿ���� ���ǿ� �����ϴ� ����� Transform�� ��ȯ�մϴ�.
        return closestTarget.transform;
    }

    /// <summary>
    /// Ÿ������ ������ �����ϴ� �޼����Դϴ�.
    /// </summary>
    /// <param name="ownerTransform"> �������� Transform</param>
    public void Targeting(Transform ownerTransform)
    {
        // ���� ���� ������ �����մϴ�.
        Collider[] SenceResults = Sencing(ownerTransform);

        // ������ ���� �� ���ǿ� �����ϴ� ����� ã���ϴ�.
        Transform targetingResult = FindClosestTarget(SenceResults, ownerTransform);

        // �ڽ� ������Ʈ�� CenterTransform ������Ʈ�� �ִٸ� �̸� Ÿ���� ������� �����մϴ�.
        Transform centerTransform = targetingResult?.transform.Find("CenterTransform");
        if (centerTransform != null)
        {
            targetingResult = centerTransform;
        }

        // ������ ��� ���� ���� ���
        if(targetingResult == null)
        {
            NoTarget();
        }
        // ������ Ÿ�����ߴ� ���� ���� Ÿ������ ����� �ٸ� ���
        else if(_CurrentTargetTransform != targetingResult)
        {
            ChangeTargeting(targetingResult);
        }
    }
}
