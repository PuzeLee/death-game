using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameController : MonoBehaviour
    {
    [Header("Scene 1-2")]
    public GameObject _door;
    public BoxCollider2D _doorBoxCollider;

    [Header("Scene 1-3")]
    public GameObject _dieXian;
    // public SpriteRenderer _dieXianSpriteRenderer;
    // public float _fadeDuration = 1.0f;
    public Light2D _globalLight;

    private ParagraphController _paragraphController;

    private void Awake()
    {
        _paragraphController = GetComponent<ParagraphController>();
    }
    // private void Start()
    // {
    //     // 初始化透明度為 0 並關閉物件
    //     Color color = _dieXianSpriteRenderer.color;
    //     color.a = 0f;
    //     _dieXianSpriteRenderer.color = color;
    //     _dieXianSpriteRenderer.gameObject.SetActive(false);
    // }

    public void ClickButton(string _paragraphName)
    {
        switch (_paragraphName)
        {
            case "4-4_You":
            case "5-7_Celeste":
                _door.SetActive(true);
                // _doorBoxCollider.enabled = false;
                break;

            case "OpenTheDoor":
                _doorBoxCollider.enabled = true;
                break;

            case "2_Aria":
                _dieXian.SetActive(true);
                _globalLight.intensity = 0.1f;
                // ShowDieXian();
                break;

            default:
                break;
        }
        if (_paragraphName == "")
        {
            _door.SetActive(true);
        }
    }

    // private IEnumerator ShowDieXian()
    // {
    //     Debug.Log("Before ShowDieXian");
    //     _dieXian.SetActive(true);
    //     // 啟用物件
    //     _dieXianSpriteRenderer.gameObject.SetActive(true);

    //     float elapsedTime = 0f;
    //     Color color = _dieXianSpriteRenderer.color;


    //     while (elapsedTime < _fadeDuration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         color.a = Mathf.Clamp01(elapsedTime / _fadeDuration);
    //         _dieXianSpriteRenderer.color = color;
    //         yield return null;
    //     }
    //     Debug.Log("After ShowDieXian");

    //     // 確保最後完全可見
    //     color.a = 1f;
    //     _dieXianSpriteRenderer.color = color;
    // }
}
