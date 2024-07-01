using System;
using UnityEngine;

public class Item_MachineGun : Item
{
    private const int MachineGunBuffCode = 100003;

    public override void GetItem(Collider collider)
    {
        collider.GetComponent<PlayerCharacter>().AddItem(MachineGunBuffCode);
    }
}

