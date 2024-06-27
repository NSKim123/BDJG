using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����۵��� �ֻ��� �θ� Ŭ����
public abstract class Item : MonoBehaviour
{
    // �ߺ� ����� ���� ���� ������ �ξ��µ� �׽�Ʈ�غ��� ��� �� �� �����ϴ�. Ȯ���ϰ� �ʿ������ �����ϰڽ��ϴ�.
    protected bool isUsed;

    protected virtual void Awake()
    {
        //Destroy(this.gameObject, 7.0f);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GetItem(other);
            Destroy(gameObject);
        }
    }
    // �������� ����ϴ� �޼����Դϴ�.
    protected abstract void GetItem(Collider collider);
}
