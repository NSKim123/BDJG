using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject waterGround;

    private Dictionary<WaveName, float> heightOfWater;
    [SerializeField] private GameObject map;
    [SerializeField] private NavMeshSurface navMeshMap;

    private void Start()
    {
        // ¹° y°ª 1´Ü°è: 0.5, 2´Ü°è: 3.1, 3´Ü°è: 4.7, 4´Ü°è: 6.1 (º¯µ¿°¡´É)
        heightOfWater = new Dictionary<WaveName, float>
        {
            {WaveName.General, 0.5f },
            {WaveName.Trainee, 3.1f },
            {WaveName.Three, 4.7f },
            {WaveName.Four, 6.1f }
        };

        navMeshMap = map.GetComponent<NavMeshSurface>();

    }

    /// <summary>
    /// ï¿½ï¿½ ï¿½ï¿½ï¿½Ì¸ï¿½ ï¿½Ã¸ï¿½ï¿½ï¿½ ï¿½Ú·ï¿½Æ¾ ï¿½Ô¼ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Õ´Ï´ï¿½.
    /// 1ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Ô¼ï¿½ï¿½ï¿½ È£ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ê½ï¿½ï¿½Ï´ï¿½.
    /// </summary>
    /// <param name="level"></param>
    public void SetWaterHeightByLevel(int level)
    {
        if (level == 1)
        {
            return;
        }
        StartCoroutine(C_WaterUP((WaveName)level));
    }

    float maxHeight;
    public IEnumerator C_WaterUP(WaveName wave)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Debug.Log("¹° ¿Ã¶ó¿È");

        while (waterGround.transform.position.y < heightOfWater[wave])
        {

            waterGround.transform.position += new Vector3(0, 0.5f, 0);

            if (waterGround.transform.position.y > heightOfWater[wave])
            {
                waterGround.transform.position =
                    new Vector3(waterGround.transform.position.x, heightOfWater[wave], waterGround.transform.position.z);
            }

            yield return new WaitForSeconds(0.1f);
        }
        navMeshMap.BuildNavMesh();
        Debug.Log("¸Ê ±¸¿ò");
    }

    public void ChangeMap(WaveName wave)
    {
        waterGround.transform.position = new Vector3(waterGround.transform.position.x, heightOfWater[wave], waterGround.transform.position.z);

        // µô·¹ÀÌ È®ÀÎ ÇÊ¿ä
        navMeshMap.BuildNavMesh();
    }

    public void StartWaterCoroutine(WaveName wave)
    {
        StartCoroutine(C_WaterUP(wave));
    }

    

    // Àç½ÃÀÛ ½Ã È£ÃâÇÒ ¸ÊÀ» ÃÊ±âÈ­ÇÏ´Â ¸Þ¼­µåÀÔ´Ï´Ù.
    public void RestartMap(WaveName wave)
    {
        if (wave != WaveName.General)
        {
            waterGround.transform.position = new Vector3(waterGround.transform.position.x, heightOfWater[WaveName.General], waterGround.transform.position.z);
        }
        navMeshMap.BuildNavMesh();

    }

}
