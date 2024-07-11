using System.Collections;
using UnityEngine;

public class ParticleInPool : MonoBehaviour
{
    private ParticleSystem myParticle;

    private void Awake()
    {
        myParticle = GetComponent<ParticleSystem>();
    }
   
   
    void OnEnable()
    {
        StartCoroutine(WaitForParticleSystem());
    }

    private IEnumerator WaitForParticleSystem()
    {
        while (myParticle.isPlaying || myParticle.particleCount > 0)
        {
            yield return null;
        }

        ReturnToPool();
    }


    private void ReturnToPool()
    {
        transform.SetParent(null);
        ObjectPoolManager.Instance.ReturnToPool(this.gameObject);
    }
}
