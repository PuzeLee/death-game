using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class GameController_1_0 : MonoBehaviour
{
	[SerializeField] private GameObject _displayText;
	[SerializeField]  private VideoPlayer _videoPlayer; // 拖入 VideoPlayer 組件
	[HideInInspector] public bool _isLoaded = false;

	private void Awake()
	{
		_displayText.SetActive(false);
	}

	private void Start()
	{
		// 開始協程等待並啟用物件
		// StartCoroutine(ActivateObjectAfterDelay());

		// 確保 VideoPlayer 組件已被設置
		if (_videoPlayer != null)
		{
			// 訂閱 loopPointReached 事件，影片播放完畢時會觸發
			_videoPlayer.loopPointReached += OnVideoFinished;
		}
		else
		{
			Debug.LogWarning("VideoPlayer 未設定！");
		}
	}
	
	private void OnVideoFinished(VideoPlayer vp)
	{
		_isLoaded = true;
		_displayText.SetActive(true);
	}

	// private IEnumerator ActivateObjectAfterDelay()
	// {
	// 	// 等待指定秒數
	// 	yield return new WaitForSeconds(_delayTime);

	// 	// 啟用目標物件
	// 	if (_displayText != null)
	// 	{
	// 		_isLoaded = true;
	// 		_displayText.SetActive(true);
	// 	}
	// 	else
	// 	{
	// 		Debug.LogWarning("Target object is not assigned!");
	// 	}
	// }

	// 當影片播放完畢時執行的方法
}
