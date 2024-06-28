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

            switch(choosedItemType)
            {
                case ItemType.Giant:
                    choosedItem = new Item_Giant();
                    break;
                case ItemType.AreaDeployment:
                    choosedItem = new Item_AreaDeployment();
                    break;
            }

            //choosedItem.GetItem(other);
            Destroy(gameObject);
        }
    }
}
