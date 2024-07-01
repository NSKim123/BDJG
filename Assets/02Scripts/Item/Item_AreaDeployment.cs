using System.Collections;
using UnityEngine;

// �������� �����ۿ� ������Ʈ�� ���� Ŭ����
public class Item_AreaDeployment : Item
{
    private const int AreaDeploymentBuffCode = 100002;

    public override void GetItem(Collider collider)
    {
        collider.GetComponent<PlayerCharacter>().AddItem(AreaDeploymentBuffCode);
    }

}
