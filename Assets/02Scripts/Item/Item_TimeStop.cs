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

        StartCoroutine(C_Wait());

        enemies = Physics.OverlapSphere(collider.transform.position, 40f, 1 << 8);
                
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<IHit>().OnDead();
        }

        Time.timeScale = 1.0f;
    }

    private IEnumerator C_Wait()
    {
        yield return new WaitForSecondsRealtime(2.4f);
    }
}
