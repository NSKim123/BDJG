using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    [SerializeField]
    private ItemProbabilityTableScriptableObject _ItemProbabilityTableScriptableObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            ItemType choosedItemType = _ItemProbabilityTableScriptableObject.Trial();

            Item choosedItem;

            switch (choosedItemType)
            {
                case ItemType.Giant:
                    choosedItem = new Item_Giant();
                    break;
                case ItemType.AreaDeployment:
                    choosedItem = new Item_AreaDeployment();
                    break;
                case ItemType.MachineGun:
                    choosedItem = new Item_MachineGun();
                    break;
                case ItemType.Shell:
                    choosedItem = new Item_Shell();
                    break;
                case ItemType.Scarecrow:
                    choosedItem = new Item_Scarecrow();
                    break;
                case ItemType.Wind:
                    choosedItem = new Item_Wind();
                    break;
                default:
                    choosedItem = null;
                    break;
            }

            choosedItem?.GetItem(other);

            Destroy(gameObject);
        }
    }
}
