using System.Collections.Generic;
using UnityEngine;

public class AudioController_1_2 : MonoBehaviour
{
	// public List<AudioClip> _audioClips = new List<AudioClip>();
	private AudioSource _audioSource;
	private ParagraphController_1_2 _paragraphController;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		_paragraphController = FindFirstObjectByType<ParagraphController_1_2>();
	}

	public void PlayMusic(string audioFileName)
	{
		// 動態載入音檔
		AudioClip clip = Resources.Load<AudioClip>("Audio/Scene1-2/" + audioFileName);
		if (clip != null)
		{
			_audioSource.clip = clip;
			_audioSource.Play();
			Debug.Log($"找到音檔 {audioFileName}");
		}
		else
		{
			Debug.Log($"音檔 {audioFileName} 未找到！請確認檔案是否放置於 Resources 資料夾中。");
		}
	}
}
