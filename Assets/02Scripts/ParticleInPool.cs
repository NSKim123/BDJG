using System.Collections;
using UnityEngine;

// 파티클을 오브젝트풀로 관리할 때 파티클에 붙는 컴포넌트입니다.
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

    /// <summary>
    /// 파티클의 재생이 끝나면 오브젝트풀로 반환합니다.
    /// </summary>
    /// <returns></returns>
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
