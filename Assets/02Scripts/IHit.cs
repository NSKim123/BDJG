using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// player�� enemy�� ���ݹ޾��� ���� ����(�з�����)�� ������ interface
public interface IHit
{
    // distance : �з��� �Ÿ�, direction : �з��� ����
    public void OnDamaged(float distance, Vector3 direction);
    public void OnDead();
}
