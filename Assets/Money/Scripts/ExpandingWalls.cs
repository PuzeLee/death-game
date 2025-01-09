using UnityEngine;

public class ExpandingWalls : MonoBehaviour
{
    [Header("左側牆壁")]
    public GameObject wallA;

    [Header("右側牆壁")]
    public GameObject wallB;

    [Header("每秒擴張速度(正值)")]
    public float expandSpeed = 0.5f;

    void Update()
    {
        // 讓 wallA, wallB 同步擴張
        // 假設 pivot 在牆中心 => scale X++
        if (wallA != null)
        {
            Vector3 scaleA = wallA.transform.localScale;
            scaleA.x += expandSpeed * Time.deltaTime;
            wallA.transform.localScale = scaleA;
        }
        if (wallB != null)
        {
            Vector3 scaleB = wallB.transform.localScale;
            scaleB.x += expandSpeed * Time.deltaTime;
            wallB.transform.localScale = scaleB;
        }
    }
}
