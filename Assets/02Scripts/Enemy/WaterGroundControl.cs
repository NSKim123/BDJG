using Unity.VisualScripting;
using UnityEngine;

public class WaterGroundControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("물 감지");
            other.GetComponent<IHit>().OnDead();
        }
    }
}
