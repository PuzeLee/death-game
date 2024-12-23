using UnityEngine;

public class ChangeScenePoint : MonoBehaviour
{
	[SerializeField] bool _goNextScene;
	[SerializeField] string _sceneName;
	
    private void OnTriggerEnter2D(Collider2D collsion)
	{
		if (collsion.CompareTag("Player"))
		{
			if (_goNextScene)
			{
				SceneController.instance.NextScene();
			}
			else
			{
				SceneController.instance.LoadScene(_sceneName);
			}
		}
	}
}
