using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// player와 enemy가 공격받았을 때의 상태(밀려나기)를 구현할 interface
/// </summary>
public interface IHit
{
    // distance : 밀려날 거리, direction : 밀려날 방향
    public void OnDamaged(float distance, Vector3 direction);
    public void OnDead();
}
