using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����۵��� �ֻ��� �θ� Ŭ����
/// </summary>
public abstract class Item
{
    // �������� ����ϴ� �޼����Դϴ�.
    public abstract void GetItem(Collider collider);
}
