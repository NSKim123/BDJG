using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveName currentWave;


    public Wave[] waves;

    public delegate void CoroutineStarter(WaveName wave);
    public event CoroutineStarter OnCoroutineStart;
    
    [SerializeField] private Transform mushroomSpawnAxis;
    [SerializeField] private Transform cactusSpawnAxis;

    private Coroutine mushroomCoroutine;
    private Coroutine cactusCoroutine;

    public System.Action<int> onEnemyDead;

    private bool isPaused = false;

    // **** �ڵ� ���� �ʿ�

    private void Start()
    {
        currentWave = WaveName.General;

        //StartMushroomSpawn(currentWave);
        //StartCoroutine(C_Test());

        /*for (int i = 0; i < waves.Length; i++)
        //{
        //    waves[i].spawn.OrderBy(obj => obj.Type).ToList();
        //    waves[i].enemydata.OrderBy(obj => obj.Type).ToList();
        }*/
    }

    private void Update()
    {
        // wave2���� �������� ���� �ڷ�ƾ�� ȣ���մϴ�.
        //if (currentWave == WaveName.Trainee && cactusCoroutine == null)
        //{
        //    cactusCoroutine = StartCoroutine(C_EnemySpawn_Cactus(currentWave));
        //}
    }

    // �׽�Ʈ
    private IEnumerator C_Test()
    {
        yield return new WaitForSeconds(5);

        currentWave = WaveName.Trainee;
        //OnChangeWave?.Invoke(currentWave);
        OnCoroutineStart?.Invoke(currentWave);
    }


    // ������ ���� �ڷ�ƾ�Դϴ�.
    private IEnumerator C_EnemySpawn_Mushroom(WaveName wave)
    {
        while (true)
        {
            // �Ͻ������� �� ���
            while (isPaused)
            {
                yield return null;
            }

            int waveIndex = (int)wave-1;

            EnemySpawnInfoData spawndata;
            EnemyInfoData enemydata;

            spawndata = waves[waveIndex].spawn[(int)EnemyType.Mushroom];
            enemydata = waves[waveIndex].enemydata[(int)EnemyType.Mushroom];

            if (EnemyManager.Instance.MushroomCount < spawndata.MaxEnemyCount)
            {
                //Vector3 randomAxis = GetRandomPositionInCircle(20 - spawndata.SpawnRadius);
                
                Vector3 randomPosition = GetRandomPositionOnCircleEdge(mushroomSpawnAxis.position, spawndata.SpawnRadius);

                yield return new WaitForSeconds(spawndata.SpawnTime);

                GameObject newEnemy = Instantiate(spawndata.EnemyPrefab, randomPosition, Quaternion.identity);

                EnemyInit(newEnemy, enemydata);
                EnemyManager.Instance.MushroomCount++;

            }
            yield return null;


        }
    }

    // �������� ���� �ڷ�ƾ�Դϴ�.
    private IEnumerator C_EnemySpawn_Cactus(WaveName wave)
    {
        while (true)
        {
            while (isPaused)
            {
                yield return null;
            }

            int waveIndex = (int)wave-1;

            EnemySpawnInfoData spawndata;
            EnemyInfoData enemydata;

            spawndata = waves[waveIndex].spawn[(int)EnemyType.Cactus];
            enemydata = waves[waveIndex].enemydata[(int)EnemyType.Cactus];

            if (EnemyManager.Instance.CactusCount < spawndata.MaxEnemyCount)
            {
                //Vector3 randomAxis = GetRandomPositionInCircle(20 - spawndata.SpawnRadius);
                Vector3 randomPosition = GetRandomPositionOnCircleEdge(cactusSpawnAxis.position, spawndata.SpawnRadius);

                yield return new WaitForSeconds(spawndata.SpawnTime);

                GameObject newEnemy = Instantiate(spawndata.EnemyPrefab, randomPosition, Quaternion.identity);

                EnemyInit(newEnemy, enemydata);
                EnemyManager.Instance.CactusCount++;

            }
            yield return null;
        }
    }
    
    // �� ���� �� �����͸� �ʱ�ȭ�ϴ� �޼����Դϴ�.
    private void EnemyInit(GameObject enemy, EnemyInfoData data)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        //e.agent.speed = data.MoveSpeed;
        e.MoveSpeed = data.MoveSpeed;
        e.AttackForce = data.AttackForce;
        e.AttackTime = data.AttackTime;
        e.DetectPlayerDistance = data.AttackRange;
        e.onDead += () => onEnemyDead?.Invoke(1);
    }

    // �Ͻ����� ���� -> ������ �޼��� ȣ��

    // �� ���� �Ͻ����� �����״�
    public void PauseSwitchEnemySpawn()
    {
        isPaused = !isPaused;
    }

    // ������ �� ȣ���� �޼���
    public void ResetForLevelUp(int level)
    {
        // �Ͻ������ϱ�
        PauseSwitchEnemySpawn();
        SpawnByWave((WaveName)level);
    }

    /// <summary>
    /// ������ ���� ���� �޼����Դϴ�.
    /// </summary>
    /// <param name="wave">���� ����, WaveName���� ����ȯ</param>
    private void StartMushroomSpawn(WaveName wave)
    {
        if (mushroomCoroutine == null)
        {
            mushroomCoroutine = StartCoroutine(C_EnemySpawn_Mushroom(wave));
        }
    }

    /// <summary>
    /// �������� ���� ���� �޼����Դϴ�.
    /// </summary>
    /// <param name="wave">���� ����, WaveName���� ����ȯ</param>
    private void StartCactusSpawn(WaveName wave)
    {
        if (cactusCoroutine == null)
        {
            cactusCoroutine = StartCoroutine(C_EnemySpawn_Cactus(wave));
        }
    }

    // ���̺꺰�� �� ����
    private void SpawnByWave(WaveName wave)
    {
        WaveName current = wave;

        // �Ͻ������� �簳�� ����
        PauseSwitchEnemySpawn();

        switch (current)
        {
            case WaveName.General:
                break;
            case WaveName.Trainee:
                {
                    StartCactusSpawn(wave);
                }
                break;
            case WaveName.Three:
                break;
            case WaveName.Four:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ���� �ڷ�ƾ �ߴ� �� �ʿ� �ִ� ���� ��� �ı��ϰ�, ������(1����) ������ �����մϴ�.
    /// </summary>
    public void ResetForRestart()
    {
        // ���� �ڷ�ƾ ���� �ߴ�
        StopAllCoroutines();

        // �� ���� �ִ� ���� ��� �ı�
        GameObject[] removeList = isEnemyExistInMap();
        if (removeList != null)
        {
            foreach (var item in removeList)
            {
                Destroy(item);
            }
        }

        // ������ ���� ����
        currentWave = WaveName.General;  // ������ ����
        StartMushroomSpawn(currentWave);
    }


    // �� ���� ������ �ִٸ� �迭�� �޾ƿ��� �޼����Դϴ�.
    private GameObject[] isEnemyExistInMap()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemys.Length > 0)
        {
            return enemys;
        }
        else
        {
            return null;
        }
    }


    // �������� �޾Ƽ� ���� ���ο��� ���� ��ġ�� ����ϴ� �޼�����Դϴ�.
    private Vector3 GetRandomPositionInCircle(float radius)
    {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad; // ������ �������� ��ȯ
        float r = Mathf.Sqrt(UnityEngine.Random.value) * radius; // �Ÿ��� ����
        float x = r * Mathf.Cos(angle);
        float z = r * Mathf.Sin(angle);
        return new Vector3(x, 1.0f, z); // ��ȯ�� ��ǥ�� ��ȯ
    }

    Vector3 GetRandomPositionOnCircleEdge(Vector3 center, float radius)
    {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad; // ������ �������� ��ȯ
        float x = center.x + radius * Mathf.Cos(angle);
        float z = center.z + radius * Mathf.Sin(angle);
        return new Vector3(x, center.y, z); // �߽��� y���� �״�� ����
    }

    // �� wave���� ���� ������ enemy ������ �����صӴϴ�.
    [Serializable]
    public class Wave
    {
        public List<EnemySpawnInfoData> spawn;
        public List<EnemyInfoData> enemydata;
    }
}
