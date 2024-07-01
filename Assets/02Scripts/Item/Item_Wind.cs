using System;
using UnityEngine;

public class Item_Wind : Item
{
    private const int WindBuffCode = 100006;

    public override void GetItem(Collider collider)
    {
        collider.GetComponent<PlayerCharacter>().AddItem(WindBuffCode);
    }
}
