using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MushroomCloud : MonoBehaviour
{
    [SerializeField] private GameObject _screenEffect;
    [SerializeField] private float _initPositionOffset = 2f;
    [SerializeField] private float _moveOffset = 0.15f;
    private float _desiredPos;

    private void Start()
    {
        _desiredPos = transform.position.z + _initPositionOffset;
    }

    private void FixedUpdate()
    {
        if (transform.position.z < _desiredPos)
        {
            transform.position += transform.forward * Time.fixedDeltaTime * _moveOffset;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            // 화면 가리는 이펙트 생성
            GameObject effect = Instantiate(_screenEffect);
            effect.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
            effect.transform.SetParent(Camera.main.transform);
            EnemyManager.Instance.StartCloud(effect, other);

            Destroy(gameObject);
        }
    }
}
