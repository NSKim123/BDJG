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
    float desiredPos;

    private void Start()
    {
        desiredPos = transform.position.y + 0.8f;
    }

    private void Update()
    {
        
        if (transform.position.y >= desiredPos)
        {
            currentTime += Time.deltaTime;

        }

        if (currentTime > destroyTime)
        {
            Destroy(gameObject);
        }

    }

    private void FixedUpdate()
    {
        if (transform.position.y < desiredPos)
        {
            transform.position += Vector3.up * Time.fixedDeltaTime * 0.2f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyManager.Instance.StartThornAttack(other);
            Debug.Log("가시 트리거");
            Destroy(gameObject);
        }
    }
}
