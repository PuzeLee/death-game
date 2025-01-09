using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameController_1_3 : MonoBehaviour
{

    [Header("Scene 1-3")]
    [Header("Photo")]
    public GameObject _playDX;
    public GameObject _yes;
    public GameObject _DX;
    [SerializeField] private float shakeRange = 0.1f; // 抖動的範圍
    [SerializeField] private float shakeInterval = 0.2f; // 抖動的間隔時間
    private Vector3 originalPosition;

    [Header("Light")]
    public Light2D _globalLight;
    [SerializeField] private float _globalMinIntensity = 0.05f;
    [SerializeField] private float _globalMaxIntensity = 0.1f;
    public Light2D _spotLight;
    [SerializeField] private float _spotMinIntensity = 2f;
    [SerializeField] private float _spotMaxIntensity = 4f;

    [Header("Role")]
    public GameObject _You;
    public GameObject _Luna;
    public GameObject _Celeste;
    public GameObject _Aria;

    private bool _isGameOver = false;

    private void Start()
    {
        originalPosition = _playDX.transform.position;
    }

    private void Update()
    {
        if (_isGameOver && Input.GetKey(KeyCode.Return))
        {
            SceneController._instance.LoadScene("Scene1-0");
        }
    }

    public void ClickButton(string _paragraphName)
    {
        switch (_paragraphName)
        {
            case "2_Aria":
                _DX.SetActive(true);
                InvokeRepeating("DitheringGlobalLight", 0f, 0.1f);
                InvokeRepeating("DitheringSpotLight", 0f, 0.1f);
                break;

            case "7-1-1_Narration":
                PlayingDX();
                break;

            case "7-1-2_DX":
                InvokeRepeating("ShakeDX", 0f, shakeInterval);
                break;

            case "11_Aria":
                InvokeRepeating("ShakeDX", 0f, shakeInterval);
                break;

            case "12_You":
                YesShowUp();
                break;

            case "13_Luna":
                PlayingDX();
                break;

            case "19_Aria":
                DXShow();
                break;

            case "20_DX":
                DXAsk();
                break;

            case "7-4_Die":
            case "21-2_Die":
            case "22-2_Die":
            case "23-3_Die":
                PlayerDie();
                break;

            case "24_End":
                End();
                break;

            default:
                break;
        }
    }

    private void DitheringGlobalLight()
    {
        _globalLight.intensity = UnityEngine.Random.Range(_globalMinIntensity, _globalMaxIntensity);
    }
    private void DitheringSpotLight()
    {
        _spotLight.intensity = UnityEngine.Random.Range(_spotMinIntensity, _spotMaxIntensity);
    }

    private void ShakeDX()
    {
        float offsetX = UnityEngine.Random.Range(-shakeRange, shakeRange);
        float offsetY = UnityEngine.Random.Range(-shakeRange, shakeRange);
        _DX.transform.position = originalPosition + new Vector3(offsetX, offsetY, 0);
    }
    private void ShakeYes()
    {
        float offsetX = UnityEngine.Random.Range(-shakeRange, shakeRange);
        float offsetY = UnityEngine.Random.Range(-shakeRange, shakeRange);
        _yes.transform.position = originalPosition + new Vector3(offsetX, offsetY, 0);
    }

    private void PlayingDX()
    {
        CancelInvoke("ShakeDX");
        CancelInvoke("ShakeYes");
        _yes.SetActive(false);
        _playDX.SetActive(true);
        _DX.SetActive(true);
        _DX.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
        _DX.transform.position = new Vector3(_DX.transform.position.x, 0.18f, _DX.transform.position.z + 1);
        originalPosition = _DX.transform.position;
    }

    private void YesShowUp()
    {
        CancelInvoke("ShakeDX");
        InvokeRepeating("ShakeYes", 0f, shakeInterval);

        _yes.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
        _yes.transform.position = new Vector3(originalPosition.x, 0.18f, originalPosition.z);

        _DX.SetActive(false);
        _yes.SetActive(true);
    }

    private void DXShow()
    {
        CancelInvoke("ShakeDX");
        CancelInvoke("ShakeYes");
        _yes.SetActive(false);
        _playDX.SetActive(false);
        _DX.SetActive(true);
        _DX.transform.localScale = _playDX.transform.localScale;
        _DX.transform.position = _playDX.transform.position;
    }

    private void DXAsk()
    {
        CancelInvoke("DitheringGlobalLight");
        _spotLight.transform.position = new Vector3(0.27f, 1.92f, 0f);
        _spotLight.pointLightOuterRadius = 2.5f;
        _globalLight.intensity = 0f;

        _Luna.SetActive(false);
        _Celeste.SetActive(false);
        _Aria.SetActive(false);
    }

    private void PlayerDie()
    {
        PlayerController playerController = _You.GetComponent<PlayerController>();
        playerController.Die();
        _isGameOver = true;
    }

    private void End()
    {
        _yes.SetActive(false);
        _playDX.SetActive(false);
        _DX.SetActive(false);
        CancelInvoke("DitheringGlobalLight");
        CancelInvoke("DitheringSpotLight");
        CancelInvoke("ShakeDX");
        CancelInvoke("ShakeYes");
        _spotLight.intensity = 3f;
        _globalLight.intensity = 0.7f;

        _Luna.SetActive(true);
        _Celeste.SetActive(true);
        _Aria.SetActive(true);

        _Luna.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        StartCoroutine(DimLightToZero());
    }
    private IEnumerator DimLightToZero()
    {
        float startIntensity = _globalLight.intensity; // 紀錄初始亮度
        float elapsed = 0f; // 已過時間
        float dimDuration = 3f;

        while (elapsed < dimDuration)
        {
            elapsed += Time.deltaTime; // 累加時間
            // 計算當前的亮度
            _globalLight.intensity = Mathf.Lerp(startIntensity, 0f, elapsed / dimDuration);
            _spotLight.intensity = Mathf.Lerp(startIntensity, 0f, elapsed / dimDuration);
            yield return null; // 等待下一幀
        }

        // 確保最終亮度是 0
        _globalLight.intensity = 0f;
        _spotLight.intensity = 0f;

        yield return new WaitForSeconds(2f);

        SceneController._instance.NextScene();
    }
}
