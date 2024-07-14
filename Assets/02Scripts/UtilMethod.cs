using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// 스폰과 관련하여 공통으로 사용되는 메서드가 있는 정적 클래스입니다.
public static class UtilSpawn
{
    /// <summary>
    /// 중심과 반지름을 받아서 원형 내부에서 랜덤 위치를 계산하는 메서드입니다.
    /// </summary>
    /// <param name="center">중심</param>
    /// <param name="radius">반지름</param>
    /// <returns></returns>
    public static Vector3 GetRandomPositionOnCircleEdge(Vector3 center, float radius)
    {
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
        float x = center.x + radius * Mathf.Cos(angle);
        float z = center.z + radius * Mathf.Sin(angle);
        return new Vector3(x, center.y, z);
    }
}

// 게임 시작 시 초기화와 관련하여 공통으로 사용되는 메서드가 있는 정적 클래스입니다.
public static class UtilReset
{
    /// <summary>
    /// 활성화된 오브젝트들을 파괴합니다.
    /// </summary>
    /// <param name="tag">파괴할 오브젝트의 tag</param>
    public static void DestroyActivatedItems(string tag)
    {
        GameObject[] removeList = IsElementsExistInMap(tag);
        if (removeList != null)
        {
            foreach (var item in removeList)
            {
                GameObject.Destroy(item);
            }
        }
    }

    /// <summary>
    /// 현재 게임 내에 활성화된 요소들을 찾아서 리턴합니다.
    /// </summary>
    /// <param name="tag">파괴할 오브젝트의 tag</param>
    /// <returns></returns>
    public static GameObject[] IsElementsExistInMap(string tag)
    {
        GameObject[] elements = GameObject.FindGameObjectsWithTag(tag);

        if (elements.Length > 0)
        {
            return elements;
        }
        else
        {
            return null;
        }
    }

}
