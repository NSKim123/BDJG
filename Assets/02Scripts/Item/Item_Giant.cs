using UnityEngine;

// 거대화 아이템에 컴포넌트 붙을 클래스
public class Item_Giant : Item
{
    private const int GiantBuffCode = 100005;

    protected override void Use(Collider collider)
    {
        collider.GetComponent<PlayerCharacter>().buffSystem.AddBuff(GiantBuffCode);
    }





}