using System;
using UnityEngine;

public class Item_Shell : Item
{
    private const int ShellBuffCode = 100004;
    public override void GetItem(Collider collider)
    {
        collider.GetComponent<PlayerCharacter>().AddItem(ShellBuffCode);
    }
}
