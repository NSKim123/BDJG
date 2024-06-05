using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    // spawn할 적의 종류마다 1개씩 정보 저장해둠, 0번: mushroom
    public EnemySpawnInfoScriptableObject[] spawnScriptableInfo;
    [SerializeField] private GameObject _mushroomPrefab;
    [SerializeField] private GameObject _player;

    public bool isGameOver = false;


    private void Start()
    {
        StartCoroutine(EnemySpawn_Mushroom());
    }

    // 적마다 Spawn함수 각각 작성
    // 게임 오버가 아니라면 spawnTime마다 실행됨
    private IEnumerator EnemySpawn_Mushroom()
    {
        while (!isGameOver)
        {
            // 맵 위 스폰된 몬스터의 최대 개수 등 추가 가능 / 오브젝트 풀로 변경
            // spawnInfo index 접근 말고 다른 방법??

            // 플레이어로부터 distance의 반지름을 가지는 원 내부에 랜덤으로 생성?
            //mushroomObject.transform.position = 

            // 테스트용 임시 위치
            Vector3 tempPosition = new Vector3(Random.Range(-10, 10), 2, Random.Range(-10, 10));

            yield return new WaitForSeconds(spawnScriptableInfo[0].SpawnTime);

            Instantiate(_mushroomPrefab, tempPosition, Quaternion.LookRotation(_player.transform.position - tempPosition));

        }

    }
}
