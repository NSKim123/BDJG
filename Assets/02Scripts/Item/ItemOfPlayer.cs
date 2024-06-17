using UnityEngine;

// player가 사용하는 아이템들의 부모클래스
// 소유주에 따라 부모클래스를 만들고 그 아래에 각 아이템 프리팹에 붙을 클래스를 구현했습니다.
// 변수와 트리거에서의 비교 구문을 소유주별로 공통으로 사용할 수 있어서 소유주 클래스를 추가했습니다.
public abstract class ItemOfPlayer : Item
{
    // 플레이어를 Start함수에서 캐싱합니다.
    [SerializeField] private PlayerCharacter _playerCharacter;

    protected override void Start()
    {
        base.Start();
        _playerCharacter = FindObjectOfType<PlayerCharacter>();
    }

    // 트리거에 플레이어가 걸리면 Use호출 후 오브젝트를 파괴합니다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Use();
            Destroy(gameObject);
        }
    }

    // 아이템을 사용하는 함수입니다.
    // Use함수 내에서 주석대로 호출하시면 됩니다.
    // 버프코드는 각각의 아이템 자식클래스에서 설정한 코드로 호출됩니다.
    public override void Use()
    {
        //playerCharacter.buffSystem.AddBuff(BuffCode);
    }

}
