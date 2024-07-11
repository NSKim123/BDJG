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
        // 가시 함정
        if (isCountingProhibit)
        {
            holdingtimeGauge -= Time.deltaTime;            

            if (holdingtimeGauge <= 0)
            {
                player.attackComponent.bulletGauge.SwitchProhibitRecover(false);
                holdingtimeGauge = 5f;
                isCountingProhibit = false;

                Destroy(instantiatedBrokenUIEffect);
            }
        }

        // 포자구름
        if (isStartCloud)
        {
             if (player.movementComponent.normalizedZXSpeed > 0)
             {
                if (currTime > delayTime)
                {
                    StartCloudAttack();
                    currTime = 0;
                }
                currTime += Time.deltaTime;

             }
        }

        
    }

    public void StartThornAttack(Collider other)
    {
        player = other.GetComponent<PlayerCharacter>();
        player.attackComponent.bulletGauge.SwitchProhibitRecover(true);
        isCountingProhibit = true;
        holdingtimeGauge = 5.0f;

        instantiatedBrokenUIEffect = Instantiate(brokenUIEffect);
        instantiatedBrokenUIEffect.transform.SetParent(FindAnyObjectByType<Canvas>().transform);
        (instantiatedBrokenUIEffect.transform as RectTransform).anchorMin = Vector2.zero;
        (instantiatedBrokenUIEffect.transform as RectTransform).anchorMax = Vector2.one;
        (instantiatedBrokenUIEffect.transform as RectTransform).localScale = Vector2.one * 1.5f;
        (instantiatedBrokenUIEffect.transform as RectTransform).offsetMin = Vector2.zero;
        (instantiatedBrokenUIEffect.transform as RectTransform).offsetMax = Vector2.zero;
    }

    public void StartCloud(GameObject cloud, GameObject other)
    {
        isStartCloud = true;
        particleObj = cloud;
        particle = cloud.GetComponent<ParticleSystem>().main;
        player = other.GetComponent<PlayerCharacter>();

        originDuration = particle.duration;
        currentDuration = originDuration;

    }

    public void StartCloudAttack()
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

    public void ResetElements()
    {
        GameObject[] removeList = IsItemExistInMap();
        if (removeList != null)
        {
            foreach (var item in removeList)
            {
                Destroy(item);
            }
        }
    }

    private GameObject[] IsItemExistInMap()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("specialAttack");
        if (items.Length > 0)
        {
            return items;
        }
        else
        {
            return null;
        }
    }


}
