using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템들의 최상위 부모 클래스
public abstract class Item : MonoBehaviour
{
    public abstract int BuffCode { get; set; }

    // 중복 사용을 막기 위한 변수로 두었는데 테스트해보니 없어도 될 것 같습니다. 확실하게 필요없으면 삭제하겠습니다.
    protected bool isUsed;

    // 아이템과 버프코드가 있는 데이터클래스입니다.
    public ItemDataRepository ItemDataRepository;
    protected virtual void Start()
    {
        ItemDataRepository = new ItemDataRepository();
    }

    // 아이템을 사용하는 메서드입니다.
    public abstract void Use();
}
