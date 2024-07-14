using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MushroomCloud : MonoBehaviour
{
    [SerializeField] private GameObject screenEffect;
    private float _desiredPos;

    private void Start()
    {
        _desiredPos = transform.position.z + 2f;    
    }

    private void FixedUpdate()
    {
        if (transform.position.z < _desiredPos)
        {
            transform.position += transform.forward * Time.fixedDeltaTime * 0.15f;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            // 화면 가리는 이펙트 생성
            GameObject effect = Instantiate(screenEffect);
            effect.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.0f;
            effect.transform.SetParent(Camera.main.transform);
            EnemyManager.Instance.StartCloud(effect, other);

            Destroy(gameObject);
        }
    }
}
