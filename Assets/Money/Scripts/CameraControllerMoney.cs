using UnityEngine;
using System.Collections;

public class CameraControllerMoney : MonoBehaviour
{
    private Transform _target;
    private Vector3 _velocity = Vector3.zero;

    [Header("Smooth Time (0~1)")]
    [Range(0, 1)]
    public float _smoothTime = 0.3f;  // 相機平滑移動的時間因子

    [Header("Axis Limitation")]
    // xLimit.x = 最小 X，xLimit.y = 最大 X
    // yLimit.x = 最小 Y，yLimit.y = 最大 Y (如果你打算使用)
    public Vector2 _xLimit;
    public Vector2 _yLimit;

    // 固定攝影機的 y 與 z，視 2D 遊戲需求而定
    [SerializeField] private float fixedY = -0.5f;
    [SerializeField] private float fixedZ = -10f;

    [Header("Toggle Follow")]
    [Tooltip("若為true，則自動跟隨_player；若為false，則不追蹤。")]
    public bool canFollow = true;

    private void Awake()
    {
        // 找到場景中 Tag 為 "Player" 的物件，當作攝影機追蹤目標
        // (若你想手動指定，也可把 _target 改為 public，直接拖曳到 Inspector)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _target = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("CameraController: 找不到Tag=Player的物件，請確認場景中角色是否設定Tag=Player。");
        }
    }

    private void LateUpdate()
    {
        // 如果 canFollow = false，代表過場等情境，暫停自動追蹤
        if (!canFollow || _target == null)
            return;

        // 取得目標位置
        Vector3 targetPosition = _target.position;

        // 限制 X 軸範圍
        float clampedX = Mathf.Clamp(targetPosition.x, _xLimit.x, _xLimit.y);

        // 固定攝影機的 y 與 z
        targetPosition = new Vector3(clampedX, fixedY, fixedZ);

        // 使用 SmoothDamp 平滑移動相機到目標位置
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            targetPosition, 
            ref _velocity, 
            _smoothTime
        );
    }
}
