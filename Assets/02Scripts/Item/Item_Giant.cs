using UnityEngine;

public class Item_Giant : Item
{
    private const int GiantBuffCode = 100001;

    public override void GetItem(Collider collider)
    {
        collider.GetComponent<PlayerCharacter>().AddItem(GiantBuffCode);
    }
}