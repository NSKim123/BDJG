using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MushroomCloud : MonoBehaviour
{
    [SerializeField] private GameObject screenEffect;
    private float currentTime = 0;
    private float detroyTime = 3;

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > detroyTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("닿음");
            // 화면 가리는 이펙트 생성
            GameObject effect = Instantiate(screenEffect);
            effect.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.0f;


            effect.transform.SetParent(Camera.main.transform);

            Destroy(gameObject);
        }
    }
}
