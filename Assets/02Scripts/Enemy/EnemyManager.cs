using System;
using Unity.VisualScripting;
using UnityEngine;

public enum Wave
{
    one,
    two,
    three,
    four,
}
public class EnemyManager : SingletonBase<EnemyManager>
{
    public EnemySpawner spawner;
    public ItemSpawner itemSpawner;
    public MapController mapController;

    // 탄창 깨짐 UI
    public GameObject brokenUIEffect;

    private PlayerCharacter player;

    private float holdingtimeGauge = 5f;
    private bool isCountingProhibit = false;

    private float originDuration;
    private float currentDuration;
    private float decreaseOffset = 0.2f;
    private bool isStartCloud;

    private GameObject particleObj;
    private ParticleSystem.MainModule particle;

    private float currTime = 0;
    private float delayTime = 0.3f;

    private GameObject instantiatedBrokenUIEffect;
    [SerializeField] private GameObject thornArea;
    [SerializeField] private GameObject cloud;

    protected override void Awake()
    {
        base.Awake();
    }


    private void Start()
    {
        mapController.OnChangeWave += spawner.ChangeLevelOfSpawn;
    }


    private void Update()
    {
        // 가시 함정 공격
        if (isCountingProhibit)
        {
            holdingtimeGauge -= Time.deltaTime;            

            if (holdingtimeGauge <= 0)
            {
                player.attackComponent.bulletGauge.SwitchProhibitRecover(false);
                holdingtimeGauge = 5f;
                isCountingProhibit = false;

                Destroy(instantiatedBrokenUIEffect);
                instantiatedBrokenUIEffect = null;
            }
        }

        // 포자구름 공격
        if (isStartCloud)
        {
             if (player.movementComponent.normalizedZXSpeed > 0)
             {
                currTime += Time.deltaTime;

                if (currTime > delayTime)
                {
                    DecreaseCloudDuration();
                    currTime = 0;
                }
            }
        }
    }

    /// <summary>
    /// 가시 함정을 생성합니다.
    /// </summary>
    /// <param name="cactus">가시 함정을 생성하는 주체(선인장)</param>
    public void CreateThornArea(SpecialCactus cactus)
    {
        Transform thornTransform = cactus.transform.GetChild(1).transform;
        Instantiate(thornArea, thornTransform.position, Quaternion.identity);

    }

    /// <summary>
    /// 가시 함정에 닿았을 때 데미지를 적용합니다.
    /// </summary>
    /// <param name="other">가시 함정에 충돌한 플레이어</param>
    public void StartThornAttack(Collider other)
    {
        player = other.GetComponent<PlayerCharacter>();
        player.attackComponent.bulletGauge.SwitchProhibitRecover(true);
        isCountingProhibit = true;
        holdingtimeGauge = 5.0f;


        if(!instantiatedBrokenUIEffect)
        {
            instantiatedBrokenUIEffect = Instantiate(brokenUIEffect);
            instantiatedBrokenUIEffect.transform.SetParent(FindAnyObjectByType<Canvas>().transform);
            (instantiatedBrokenUIEffect.transform as RectTransform).anchorMin = Vector2.zero;
            (instantiatedBrokenUIEffect.transform as RectTransform).anchorMax = Vector2.one;
            (instantiatedBrokenUIEffect.transform as RectTransform).localScale = Vector2.one * 1.5f;
            (instantiatedBrokenUIEffect.transform as RectTransform).offsetMin = Vector2.zero;
            (instantiatedBrokenUIEffect.transform as RectTransform).offsetMax = Vector2.zero;
        }
        
    }

    /// <summary>
    /// 포자 구름을 생성합니다.
    /// </summary>
    /// <param name="mushroom">포자 구름을 생성하는 주체(버섯)</param>
    public void CreateCloud(SpecialMushroom mushroom)
    {
        Vector3 pos = mushroom.transform.position + new Vector3(0, 1.0f, 1.5f);
        Instantiate(cloud, pos, Quaternion.identity);
    }

    /// <summary>
    /// 포자구름에 닿았을 때 공격을 시작합니다.(파티클의 지속시간을 줄이기 위한 초기화 작업)
    /// </summary>
    /// <param name="cloud">포자구름</param>
    /// <param name="other">포자구름에 충돌한 플레이어</param>
    public void StartCloud(GameObject cloud, GameObject other)
    {
        isStartCloud = true;
        particleObj = cloud;
        particle = cloud.GetComponent<ParticleSystem>().main;
        player = other.GetComponent<PlayerCharacter>();

        originDuration = particle.duration;
        currentDuration = originDuration;
    }

    /// <summary>
    /// 포자구름 파티클의 지속시간을 줄입니다.
    /// </summary>
    public void DecreaseCloudDuration()
    {
        currentDuration = Mathf.Max(currentDuration - decreaseOffset, 0.1f);

        if (currentDuration <= 0.1f)
        {
            isStartCloud = false;
            return;
        }

        if (particleObj == null)
        {
            return;
        }
        float newSpeedrate = originDuration / currentDuration;
        particle.simulationSpeed = newSpeedrate;
    }

    /// <summary>
    /// 게임 시작 시 특수공격이 남아있다면 파괴합니다.
    /// </summary>
    public void ResetSpecialAttack()
    {
        UtilReset.DestroyActivatedItems("specialAttack");
    }

}
