using Unity.VisualScripting;
using UnityEngine;

public class WaterGroundControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IHit>(out IHit iHit))
        {
            //Debug.Log("물 감지" + gameObject.name);
            iHit.OnDead();
        }
    }
}
