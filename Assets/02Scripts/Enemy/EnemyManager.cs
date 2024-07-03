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

    // 특수 개체 공격 생성

    public EnemySpawner spawner;
    public ItemSpawner itemSpawner;
    public MapController mapController;

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


    private void Start()
    {
        mapController.OnChangeWave += spawner.SwitchSpawnValue;

    }


    private void Update()
    {
        // 가시 함정
        if (isCountingProhibit)
        {
            holdingtimeGauge -= Time.deltaTime;
            Debug.Log(holdingtimeGauge);

            if (holdingtimeGauge <= 0)
            {
                player.attackComponent.bulletGauge.SwitchProhibitRecover(false);
                holdingtimeGauge = 5f;
                isCountingProhibit = false;
            }
        }

        // 포자구름
        if (isStartCloud)
        {
             if (player.movementComponent.normalizedZXSpeed > 0)
             {
                if (currTime > delayTime)
                {
                    Debug.Log("움직임");
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
        

        Debug.Log(currentDuration + "현재 남은 시간");
    }


}
