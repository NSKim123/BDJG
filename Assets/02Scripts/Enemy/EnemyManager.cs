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

    // źâ ���� UI
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
        // ���� ���� ����
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

        // ���ڱ��� ����
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
    /// ���� ������ �����մϴ�.
    /// </summary>
    /// <param name="cactus">���� ������ �����ϴ� ��ü(������)</param>
    public void CreateThornArea(SpecialCactus cactus)
    {
        Transform thornTransform = cactus.transform.GetChild(1).transform;
        Instantiate(thornArea, thornTransform.position, Quaternion.identity);

    }

    /// <summary>
    /// ���� ������ ����� �� �������� �����մϴ�.
    /// </summary>
    /// <param name="other">���� ������ �浹�� �÷��̾�</param>
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
    /// ���� ������ �����մϴ�.
    /// </summary>
    /// <param name="mushroom">���� ������ �����ϴ� ��ü(����)</param>
    public void CreateCloud(SpecialMushroom mushroom)
    {
        Vector3 pos = mushroom.transform.position + new Vector3(0, 1.0f, 1.5f);
        Instantiate(cloud, pos, Quaternion.identity);
    }

    /// <summary>
    /// ���ڱ����� ����� �� ������ �����մϴ�.(��ƼŬ�� ���ӽð��� ���̱� ���� �ʱ�ȭ �۾�)
    /// </summary>
    /// <param name="cloud">���ڱ���</param>
    /// <param name="other">���ڱ����� �浹�� �÷��̾�</param>
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
    /// ���ڱ��� ��ƼŬ�� ���ӽð��� ���Դϴ�.
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
    /// ���� ���� �� Ư�������� �����ִٸ� �ı��մϴ�.
    /// </summary>
    public void ResetSpecialAttack()
    {
        UtilReset.DestroyActivatedItems("specialAttack");
    }

}
