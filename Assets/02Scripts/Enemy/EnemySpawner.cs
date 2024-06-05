using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    // spawn�� ���� �������� 1���� ���� �����ص�, 0��: mushroom
    public EnemySpawnInfoScriptableObject[] spawnScriptableInfo;
    [SerializeField] private GameObject _mushroomPrefab;
    [SerializeField] private GameObject _player;

    public bool isGameOver = false;


    private void Start()
    {
        StartCoroutine(EnemySpawn_Mushroom());
    }

    // ������ Spawn�Լ� ���� �ۼ�
    // ���� ������ �ƴ϶�� spawnTime���� �����
    private IEnumerator EnemySpawn_Mushroom()
    {
        while (!isGameOver)
        {
            // �� �� ������ ������ �ִ� ���� �� �߰� ���� / ������Ʈ Ǯ�� ����
            // spawnInfo index ���� ���� �ٸ� ���??

            // �÷��̾�κ��� distance�� �������� ������ �� ���ο� �������� ����?
            //mushroomObject.transform.position = 

            // �׽�Ʈ�� �ӽ� ��ġ
            Vector3 tempPosition = new Vector3(Random.Range(-10, 10), 2, Random.Range(-10, 10));

            yield return new WaitForSeconds(spawnScriptableInfo[0].SpawnTime);

            Instantiate(_mushroomPrefab, tempPosition, Quaternion.LookRotation(_player.transform.position - tempPosition));

        }

    }
}
