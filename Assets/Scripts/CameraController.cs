using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

// public class CameraController : MonoBehaviour {

// 	public Transform target;
// 	float trackingSpeed = 2.0f;
// 	void Start () {

// 		if (target == null) {
// 			target = GameObject.Find ("Character").transform;
// 		}
// 	}


// 	void Update () {
// 		Vector3 position = target.position;
// 		position.y = target.position.y;
// 		position.z = -15;
// 		position.x = target.position.x + 1;
// 		position.y = target.position.y;
// 		transform.position = Vector3.Lerp (transform.position, position, trackingSpeed * 3 * Time.deltaTime);
// 	}
// }

public class CameraController : MonoBehaviour
{

    private Transform _target;
    private Vector3 _velocity = Vector3.zero;

    [Range(0, 1)]
    public float _smoothTime;
    // public Vector3 positionOffset;

    [Header("Axis Limitation")]
    public Vector2 _xLimit;
    public Vector2 _yLimit;
    [SerializeField] private float fixedY = -0.5f;
    [SerializeField] private float fixedZ = -10f;

    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // private void LateUpdate()
    // {
    //     Vector3 targetPosition = _target.position + positionOffset;
    //     targetPosition = new Vector3(Mathf.Clamp(targetPosition.x, _xLimit.x, _xLimit.y), Mathf.Clamp(targetPosition.y, _yLimit.x, _yLimit.y), -10);
    //     transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
    // }

    private void LateUpdate()
    {
        Vector3 targetPosition = _target.position;

        // 取得目標位置的 x 座標，加上偏移量，並限制在 x 軸範圍內
        float targetX = Mathf.Clamp(targetPosition.x, _xLimit.x, _xLimit.y);

        // 將攝影機目標位置設為計算後的 (x, y, z)
        targetPosition = new Vector3(targetX, fixedY, fixedZ);

        // 平滑移動到目標位置
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
    }

}
