using System.Collections;
using UnityEngine;

// 영역전개 아이템에 컴포넌트로 붙을 클래스
public class Item_TimeStop : Item
{
    [SerializeField] private GameObject effect;
    private Collider[] enemies;

    protected override void Use(Collider collider)
    {
        // other(pc)에서 오버랩 아주 크게, 걸린 적들 모두 죽는 디버프
        Time.timeScale = 0.0f;
        Instantiate(effect);
        enemies = Physics.OverlapSphere(collider.transform.position, 40f, 1 << 8);

        StartCoroutine(C_Wait());

        foreach (var enemy in enemies)
        {
            enemy.GetComponent<IHit>().OnDead();
        }
    }

    private IEnumerator C_Wait()
    {
        yield return new WaitForSecondsRealtime(2);
    }
}
