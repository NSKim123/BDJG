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
    public Enemy enemy;
    public MapController mapController;

    private PlayerCharacter player;

    private float holdingtimeGauge = 5f;
    private bool isCountingProhibit;

    private float originDuration;
    private float currentDuration;
    private float decreaseOffset = 0.2f;
    private bool isStartCloud;
    private ParticleSystem.MainModule particle;

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
            StartCloudAttack();

        }

        
    }

    public void StartThornAttack(Collider other)
    {
        player = other.GetComponent<PlayerCharacter>();
        player.attackComponent.bulletGauge.SwitchProhibitRecover(true);
        isCountingProhibit = true;
    }

    public void StartCloud(GameObject cloud)
    {
        isStartCloud = true;
        particle = cloud.GetComponent<ParticleSystem>().main;

    }

    public void StartCloudAttack()
    {
        originDuration = particle.duration;

        currentDuration = Mathf.Max(currentDuration - decreaseOffset, 0.1f);

        if (currentDuration <= 0.1f)
        {
            isStartCloud = false;
            return;
        }

        float newSpeedrate = originDuration / currentDuration;
        particle.simulationSpeed = newSpeedrate;

        Debug.Log(currentDuration + "현재 남은 시간");
    }


}
