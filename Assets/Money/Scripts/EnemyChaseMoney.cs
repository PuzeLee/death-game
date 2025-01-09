using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyChaseMoney : MonoBehaviour
{
    [Header("要追蹤的目標(玩家)")]
    public Transform targetPlayer;

    [Header("移動速度(建議慢些)")]
    public float moveSpeed = 1f;

    private void Update()
    {
        if (targetPlayer == null) return;

        // 計算方向
        Vector3 dir = targetPlayer.position - transform.position;
        dir.z = 0f;
        transform.position = Vector3.MoveTowards(
            transform.position,
            transform.position + dir.normalized,
            moveSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerA") || other.CompareTag("PlayerB"))
        {
            Debug.Log("追蹤敵人撞到玩家 -> 玩家死亡");
            GameControllerMoney.Instance.OnPlayerDead(other.gameObject);
        }
    }
}
