using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ThornArea : MonoBehaviour
{
    float currentTime = 0f;
    float destroyTime = 5f;

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > destroyTime)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //탄환 회복 막고 함정 없애기
            //other.GetComponent<BulletGauge>().
            //Destroy(gameObject);
        }
    }
}
