using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("巡邏點A")]
    public Transform pointA;
    [Header("巡邏點B")]
    public Transform pointB;

    [Header("移動速度")]
    public float moveSpeed = 2f;

    private Transform currentTarget;

    private void Start()
    {
        // 起始目標
        currentTarget = pointA;
    }

    private void Update()
    {
        if (currentTarget == null) return;

        // 移動到 currentTarget
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            moveSpeed * Time.deltaTime
        );

        // 判斷是否抵達目標點
        float distance = Vector3.Distance(transform.position, currentTarget.position);
        if (distance < 0.01f)
        {
            // 抵達後切換目標
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerA") || other.CompareTag("PlayerB"))
        {
            Debug.Log("巡邏敵人撞到玩家 -> 玩家死亡");
            GameControllerMoney.Instance.OnPlayerDead(other.gameObject);
        }
    }
}
