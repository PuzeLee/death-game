using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("此玩家 ID (ex: PlayerA, PlayerB, PlayerA4, PlayerB4)")]
    public string playerID = "PlayerA";

    [Header("對應的終點 Tag (ex: Finish, FinishA4, FinishB4)")]
    public string finishTag = "Finish"; // 或 "FinishA4" / "FinishB4"

    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollisionTag(collision.gameObject.tag);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollisionTag(other.gameObject.tag);
    }

    private void CheckCollisionTag(string tagName)
    {
        if (tagName == "Walls")
        {
            Debug.Log($"{playerID} 撞到牆 => 玩家死亡");
            GameControllerMoney.Instance.OnPlayerDead(this.gameObject);
        }
        else if (tagName == "Enemy" || tagName == "EnemyChase")
        {
            // 例如某敵人Tag
            Debug.Log($"{playerID} 被敵人撞到 => 玩家死亡");
            GameControllerMoney.Instance.OnPlayerDead(this.gameObject);
        }
        else if (tagName == finishTag)
        {
            Debug.Log($"{playerID} 抵達終點 => 通知GameController");
            GameControllerMoney.Instance.NotifyPlayerFinished(playerID);
        }
    }
}
