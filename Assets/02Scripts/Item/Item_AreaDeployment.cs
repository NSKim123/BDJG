using System.Collections;
using UnityEngine;

// 영역전개 아이템에 컴포넌트로 붙을 클래스
public class Item_AreaDeployment : Item
{
    private const int AreaDeploymentBuffCode = 100002;

    public override void GetItem(Collider collider)
    {
        collider.GetComponent<PlayerCharacter>().AddItem(AreaDeploymentBuffCode);
    }

}
