using System.Collections;
using UnityEngine;

// �������� �����ۿ� ������Ʈ�� ���� Ŭ����
public class Item_TimeStop : Item
{
    [SerializeField] private GameObject effect;
    private Collider[] enemies;

    protected override void Use(Collider collider)
    {
        // other(pc)���� ������ ���� ũ��, �ɸ� ���� ��� �״� �����
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
