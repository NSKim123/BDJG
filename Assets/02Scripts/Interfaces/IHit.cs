using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// player�� enemy�� ���ݹ޾��� ���� ����(�з�����)�� ������ interface
/// </summary>
public interface IHit
{
    // distance : �з��� �Ÿ�, direction : �з��� ����
    public void OnDamaged(float distance, Vector3 direction);
    public void OnDead();
}
