using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	// public List<AudioClip> _audioClips = new List<AudioClip>();
	private AudioSource _audioSource;
	public ParagraphController _paragraphController;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		// _paragraphController = GetComponent<ParagraphController>();
	}

	public void PlayMusic(string _paragraphName)
	{
		// 動態載入音檔
		AudioClip clip = Resources.Load<AudioClip>("Audio/Scene1-2/" + _paragraphName);
		if (clip != null)
		{
			_audioSource.clip = clip;
			_audioSource.Play();
			Debug.Log($"找到音檔 {_paragraphName}");
		}
		else
		{
			Debug.Log($"音檔 {_paragraphName} 未找到！請確認檔案是否放置於 Resources 資料夾中。");
		}
		// switch (_paragraphName)
		// {
			
		// 	case "4-4_You":
		// 	case "5-7_Celeste":
		// 		break;

		// 	case "OpenTheDoor":
		// 		break;

		// 	default:
		// 		break;
		// }
	}
}
