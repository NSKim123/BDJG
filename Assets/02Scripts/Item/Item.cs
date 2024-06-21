using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����۵��� �ֻ��� �θ� Ŭ����
public abstract class Item : MonoBehaviour
{
    // �ߺ� ����� ���� ���� ������ �ξ��µ� �׽�Ʈ�غ��� ��� �� �� �����ϴ�. Ȯ���ϰ� �ʿ������ �����ϰڽ��ϴ�.
    protected bool isUsed;

    protected virtual void OnTriggerEnter(Collider other)
    {

        if (other.transform.CompareTag("Player"))
        {
            Use(other);
            Destroy(gameObject);
        }
    }
    // �������� ����ϴ� �޼����Դϴ�.
    protected abstract void Use(Collider collider);
}