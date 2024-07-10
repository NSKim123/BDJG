using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ThornArea : MonoBehaviour
{
    float currentTime = 0f;
    float destroyTime = 7f;
    float desiredPos;

    public Terrain terrain;
    public Vector3 worldPosition; // 터레인 상의 월드 위치


    private void Start()
    {
        desiredPos = transform.position.y + 0.5f;

        // 터레인의 로컬 좌표계로 변환
        worldPosition = transform.position;
        Vector3 terrainPosition = worldPosition - terrain.transform.position;

        // 정규화된 좌표로 변환
        float normalizedX = terrainPosition.x / terrain.terrainData.size.x;
        float normalizedZ = terrainPosition.z / terrain.terrainData.size.z;

        // 특정 위치에서 법선 벡터를 얻음
        Vector3 normal = terrain.terrainData.GetInterpolatedNormal(normalizedX, normalizedZ);

        // 법선 벡터와 세계 좌표의 위쪽 벡터 사이의 회전 계산
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);

        transform.rotation = rotation;
        // 오브젝트 생성 및 회전 적용
        //GameObject instantiatedObject = Instantiate(objectPrefab, worldPosition, rotation);

        // 오브젝트의 위치를 터레인 높이에 맞춤
        float terrainHeight = terrain.SampleHeight(worldPosition) + terrain.transform.position.y;
        transform.position = new Vector3(worldPosition.x, terrainHeight, worldPosition.z);


    }

    private void Update()
    {
        if (transform.position.y >= desiredPos)
        {
            currentTime += Time.deltaTime;

        }


        if (currentTime > destroyTime)
        {
            Destroy(gameObject);
        }

    }

    private void FixedUpdate()
    {
        if (transform.position.y < desiredPos)
        {
            transform.position += Vector3.up * Time.fixedDeltaTime * 0.2f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyManager.Instance.StartThornAttack(other);
            Destroy(gameObject);
        }
    }
}
