using UnityEngine;

// �������� �����ۿ� ������Ʈ�� ���� Ŭ����
public class Item_TimeStop : Item
{
    [SerializeField] private GameObject effect;

    protected override void Use(Collider collider)
    {
        // other(pc)���� ������ ���� ũ��, �ɸ� ���� ��� �״� �����
        Time.timeScale = 0.0f;
        Instantiate(effect);
    }
}
