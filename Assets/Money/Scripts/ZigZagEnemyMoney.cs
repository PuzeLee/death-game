using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ZigZagEnemyMoney : MonoBehaviour
{
    [Header("Z 字型路徑點(依序移動)")]
    public Transform[] waypoints;

    [Header("移動速度")]
    public float moveSpeed = 2f;

    private int currentIndex = 0;
    private bool reverse = false; // 往回走？

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // 抵達目標點
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 0.01f)
        {
            // 決定下一個目標
            if (!reverse)
            {
                currentIndex++;
                if (currentIndex >= waypoints.Length)
                {
                    // 到頂 -> 改為往回
                    currentIndex = waypoints.Length - 2; 
                    reverse = true;
                }
            }
            else
            {
                currentIndex--;
                if (currentIndex < 0)
                {
                    // 回到底 -> 再往正向
                    currentIndex = 1;
                    reverse = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 第四關是雙玩家任一碰到都死
        if (other.CompareTag("PlayerA") || other.CompareTag("PlayerB") || other.CompareTag("Player"))
        {
            Debug.Log("Z 字型敵人撞到玩家 -> 玩家死亡");
            GameControllerMoney.Instance.OnPlayerDead(other.gameObject);
        }
    }
}
