using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private PlayerMovement _PlayerMovement;

    private PlayerAttack _PlayerAttack;

    public PlayerMovement movementComponent => _PlayerMovement ?? (_PlayerMovement = GetComponent<PlayerMovement>());

    public PlayerAttack attackComponent => _PlayerAttack ?? (_PlayerAttack = GetComponent<PlayerAttack>());

    
}
