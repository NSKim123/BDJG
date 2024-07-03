using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MushroomCloudScreen : MonoBehaviour
{
    private PlayerCharacter player;
    private GameObject particle;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerCharacter>();
        //particle = GetComponent<MushroomCloud>();
    }

    private void Update()
    {
        if (player.movementComponent.normalizedZXSpeed > 0)
        {
            
        }
    }

    
}
