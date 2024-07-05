using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : Bullet
{
    [SerializeField] private GameObject _effect;

    protected override void OnTriggerEnter(Collider collider)
    {
        // �÷��̾���� �浹�� �����մϴ�.
        if (collider.gameObject.tag == "Player") return;

        // �������� �浹�� �����մϴ�,
        if (collider.gameObject.layer == LayerMask.GetMask("Ground")) return;

        Collider[] hitResults = Physics.OverlapSphere(transform.position, 2.0f, LayerMask.GetMask("Enemy"));

        foreach(Collider hitResult in hitResults)
        {
            if (hitResult.gameObject.TryGetComponent<IHit>(out IHit hitEnemy))
            {
                Vector3 hitDirection = hitResult.transform.position - transform.position;

                hitDirection.y = 0.0f;
                hitDirection.Normalize();
                hitEnemy.OnDamaged(_AttackPower, hitDirection);

                Instantiate(_effect, transform.position + Vector3.up, Quaternion.identity);
            }
        }

        PlaySound(collider);

        // �� ����ü�� �����մϴ�.
        Destroy(this.gameObject);
    }
}
