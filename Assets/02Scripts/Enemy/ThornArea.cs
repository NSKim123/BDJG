using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ThornArea : MonoBehaviour
{
    private float _currentTime = 0f;
    [SerializeField] private float _destroyTime = 7f;
    private float _desiredPos;

    private Terrain _terrain;
    private Vector3 _worldPosition;

    [SerializeField] private float _moveOffset = 0.15f;

    private void Awake()
    {
        _terrain = FindAnyObjectByType<Terrain>();
    }


    private void Start()
    {
        _desiredPos = transform.position.y + 0.5f;

        // 터레인의 로컬 좌표계로 변환
        _worldPosition = transform.position;
        Vector3 terrainPosition = _worldPosition - _terrain.transform.position;

        float normalizedX = terrainPosition.x / _terrain.terrainData.size.x;
        float normalizedZ = terrainPosition.z / _terrain.terrainData.size.z;

        // 법선 벡터 계산
        Vector3 normal = _terrain.terrainData.GetInterpolatedNormal(normalizedX, normalizedZ);

        // 법선 벡터와 가시 함정의 위쪽 벡터 사이 회전 계산
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);

        transform.rotation = rotation;
        
    }

    private void Update()
    {
        // 다 올라온 후부터 삭제 타이머 체크
        if (transform.position.y >= _desiredPos)
        {
            _currentTime += Time.deltaTime;
        }
        
        if (_currentTime > _destroyTime)
        {
            Destroy(gameObject);
        }

    }

    private void FixedUpdate()
    {
        // 생성 후 점점 위로 올라오는 효과
        if (transform.position.y < _desiredPos)
        {
            transform.position += Vector3.up * Time.fixedDeltaTime * _moveOffset;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 밟을 시 가시함정 공격 시작
        if (other.CompareTag("Player"))
        {
            EnemyManager.Instance.StartThornAttack(other);
            Destroy(gameObject);
        }
    }
}
