using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����۵��� �ֻ��� �θ� Ŭ����
public abstract class Item : MonoBehaviour
{
    public abstract int BuffCode { get; set; }

    // �ߺ� ����� ���� ���� ������ �ξ��µ� �׽�Ʈ�غ��� ��� �� �� �����ϴ�. Ȯ���ϰ� �ʿ������ �����ϰڽ��ϴ�.
    protected bool isUsed;

    // �����۰� �����ڵ尡 �ִ� ������Ŭ�����Դϴ�.
    public ItemDataRepository ItemDataRepository;
    protected virtual void Start()
    {
        ItemDataRepository = new ItemDataRepository();
    }

    // �������� ����ϴ� �޼����Դϴ�.
    public abstract void Use();
}
