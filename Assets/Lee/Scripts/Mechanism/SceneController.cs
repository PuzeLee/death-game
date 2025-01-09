using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
	public static SceneController _instance;
	// public SceneController instance;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

    private void Update()
    {
        // 只有在 Scene1-0 (buildIndex=0) 偵測 Enter 鍵
        if (SceneManager.GetActiveScene().buildIndex == 0
            && Input.GetKeyDown(KeyCode.Return))
        {
            // 動畫載入完畢
            if (FindFirstObjectByType<GameController_1_0>()._isLoaded)
            {
                NextScene();
            }
        }
    }

    public void NextScene()
	{
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1); // use index
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadSceneAsync(sceneName); // use name
	}
}
