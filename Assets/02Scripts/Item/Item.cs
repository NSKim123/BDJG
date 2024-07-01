using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템들의 최상위 부모 클래스
public abstract class Item
{
    // 아이템을 사용하는 메서드입니다.
    public abstract void GetItem(Collider collider);
}
