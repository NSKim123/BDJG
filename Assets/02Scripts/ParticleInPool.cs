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
        StartCoroutine(C_WaitForParticleSystem());
    }

    /// <summary>
    /// 파티클의 재생이 끝나면 오브젝트풀로 반환합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_WaitForParticleSystem()
    {
        while (myParticle.isPlaying || myParticle.particleCount > 0)
        {
            yield return null;
        }

        ReturnToPool();
    }

    /// <summary>
    /// 오브젝트풀로 반환합니다.
    /// </summary>
    private void ReturnToPool()
    {
        // 파티클이 플레이어 또는 적의 자식으로 들어가기 때문에 parent를 null로 변경
        transform.SetParent(null);
        ObjectPoolManager.Instance.ReturnToPool(this.gameObject);
    }
}
