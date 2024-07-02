using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    [SerializeField]
    private ItemProbabilityTableScriptableObject _ItemProbabilityTableScriptableObject;

    private float _InstantiatedTime;

    private void Awake()
    {
        Destroy(this.gameObject, 20.0f);
        _InstantiatedTime = Time.time;
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.up * 0.01f * Mathf.Pow(Time.time - _InstantiatedTime, 2.0f) * Time.fixedDeltaTime;
    }

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
