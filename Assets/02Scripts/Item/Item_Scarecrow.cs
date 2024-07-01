using System;
using UnityEngine;

public class Item_Scarecrow : Item
{
    private const int ScarecrowBuffCode = 100005;

    public override void GetItem(Collider collider)
    {
        collider.GetComponent<PlayerCharacter>().AddItem(ScarecrowBuffCode);
    }
}
