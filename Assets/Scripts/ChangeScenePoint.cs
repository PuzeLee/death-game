using UnityEngine;

public class ChangeScenePoint : MonoBehaviour
{
	[SerializeField] bool _goNextScene;
	[SerializeField] string _sceneName;
	
	public SceneController instance;

	private void OnTriggerEnter2D(Collider2D collsion)
	{
		if (collsion.CompareTag("Player"))
		{
			if (_goNextScene)
			{
				instance.NextScene();
			}
			else
			{
				instance.LoadScene(_sceneName);
			}
		}
	}
}
