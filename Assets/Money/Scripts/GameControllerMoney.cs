using UnityEngine;
using UnityEngine.Video;

public class GameControllerMoney : MonoBehaviour
{
    public static GameControllerMoney Instance { get; private set; }

    [Header("關卡 (Game) 物件，順序對應1~5關")]
    public GameObject[] gameLevels; 
    // [0] = Game1, [1] = Game2, [2] = Game3, [3] = Game4, [4] = Game5

    [Header("玩家 (第一~第三關)")]
    public GameObject playerA;          // 先前三關用的單人玩家
    public Transform[] spawnPoints;     // [0]~[2] 給Game1, Game2, Game3 用

    [Header("敵人(第二關)")]
    public GameObject enemyChase; // 敵人物件
    public Transform enemySpawnPoint; // 敵人出生點


    [Header("第四關的雙玩家(新的外觀)")]
    public GameObject playerA4;
    public GameObject playerB4;
    public Transform spawnPointA4;
    public Transform spawnPointB4;
    // 假設這對應第四關要用的出生位置

    [Header("第五關兩面牆(會變寬)")]
    public GameObject wallA; // 左側牆
    public GameObject wallB; // 右側牆

    [Header("每秒擴張速度(正值)")]
    public float expandSpeed = 0.5f;
    // 後面會搭配 ExpandingWalls.cs

    // ---- 第4關雙玩家完成判斷 ----
    private bool playerA4_Finished = false;
    private bool playerB4_Finished = false;

    private int currentLevelIndex = 0;

    // ---- 恐怖影片 & 死亡流程 ----
    [Header("恐怖影片(可選)")]
    public VideoPlayer horrorVideoPlayer;
    public GameObject rawImageObject;

    public VideoClip[] horrorVideoClips; // 新增：多個恐怖影片

    private bool isDead = false;     // 是否正在死亡播放流程
    private bool videoEnded = false; // 影片是否結束

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 初始化影片
        if (horrorVideoPlayer != null)
        {
            horrorVideoPlayer.playOnAwake = false;
            horrorVideoPlayer.Stop();
            horrorVideoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    void Start()
    {
        // 1. 關閉所有關卡
        for (int i = 0; i < gameLevels.Length; i++)
        {
            if (gameLevels[i] != null)
                gameLevels[i].SetActive(false);
        }

        // 2. 顯示第一關 (index=0)
        if (gameLevels.Length > 0 && gameLevels[0] != null)
            gameLevels[0].SetActive(true);

        // 3. 設定第一關玩家位置
        SetPlayerPosition(0);

        // 預設關閉 第四關玩家(4)
        if (playerA4 != null) playerA4.SetActive(false);
        if (playerB4 != null) playerB4.SetActive(false);

        // 若有影片UI，一開始關閉
        if (horrorVideoPlayer != null) horrorVideoPlayer.gameObject.SetActive(false);
        if (rawImageObject != null) rawImageObject.SetActive(false);

        // 第五關牆壁 若只在第五關需要，就先關閉
        if (wallA != null) wallA.SetActive(false);
        if (wallB != null) wallB.SetActive(false);
    }

    //--------------------------------------------------------------------------------
    // (A) 撞到終點 => NotifyPlayerFinished
    //--------------------------------------------------------------------------------
    public void NotifyPlayerFinished(string playerID)
    {
        // 如果不是第四關 => 直接下一關
        if (currentLevelIndex != 3)
        {
            // 若是前三關的玩家
            DisableDrag(playerA);
            GoToNextLevel();
            return;
        }

        // 第四關 => playerA4 / playerB4
        if (playerID == "PlayerA4") playerA4_Finished = true;
        else if (playerID == "PlayerB4") playerB4_Finished = true;

        if (playerA4_Finished && playerB4_Finished)
        {
            // 兩位都到終點
            DisableDrag(playerA4);
            DisableDrag(playerB4);
            GoToNextLevel();
            playerA4_Finished = false;
            playerB4_Finished = false;
        }
    }

    //--------------------------------------------------------------------------------
    // (B) 撞到牆/敵人 => OnPlayerDead
    //--------------------------------------------------------------------------------
    public void OnPlayerDead(GameObject deadPlayer)
{
    Debug.Log("GameController: OnPlayerDead 被呼叫");
    isDead = true;
    videoEnded = false;

    if (deadPlayer != null)
        deadPlayer.SetActive(false);

    // 播放隨機選擇的恐怖影片
    if (horrorVideoPlayer != null && horrorVideoClips.Length > 0)
    {
        // 隨機選擇一個影片
        int randomIndex = Random.Range(0, horrorVideoClips.Length);
        VideoClip selectedClip = horrorVideoClips[randomIndex];
        horrorVideoPlayer.clip = selectedClip;

        horrorVideoPlayer.gameObject.SetActive(true);
        if (rawImageObject != null) rawImageObject.SetActive(true);

        horrorVideoPlayer.Play();
        Debug.Log("播放恐怖影片: " + selectedClip.name);
    }
    else
    {
        Debug.LogWarning("沒有指定任何恐怖影片或 VideoPlayer 未設定。");
        videoEnded = true;
    }
}

    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("影片播放完畢 => 可點擊重新開始");
        videoEnded = true;
    }

    void Update()
    {
        // 若死亡 & 影片完 => 等玩家點擊
        if (isDead && videoEnded && Input.GetMouseButtonDown(0))
        {
            Debug.Log("點擊 => 重新開始關卡");
            if (horrorVideoPlayer != null)
            {
                horrorVideoPlayer.Stop();
                horrorVideoPlayer.gameObject.SetActive(false);
            }
            if (rawImageObject != null)
                rawImageObject.SetActive(false);

            isDead = false;
            RestartCurrentLevel();
        }
        if(currentLevelIndex == 4){
            if (wallA != null)
            {
                Vector3 scaleA = wallA.transform.localScale;
                scaleA.x += expandSpeed * Time.deltaTime;
                wallA.transform.localScale = scaleA;
            }
            if (wallB != null)
            {
                Vector3 scaleB = wallB.transform.localScale;
                scaleB.x += expandSpeed * Time.deltaTime;
                wallB.transform.localScale = scaleB;
            }
        }
        
        
    }

    //--------------------------------------------------------------------------------
    // (C) 切換關卡 / 重新開始
    //--------------------------------------------------------------------------------
    public void GoToNextLevel()
    {
        if (currentLevelIndex < gameLevels.Length)
            gameLevels[currentLevelIndex].SetActive(false);

        currentLevelIndex++;

        if (currentLevelIndex < gameLevels.Length)
        {
            gameLevels[currentLevelIndex].SetActive(true);
            SetPlayerPosition(currentLevelIndex);
            SetEnemiesAndWalls(currentLevelIndex);
        }
        else
        {
            Debug.Log("所有關卡完成！");
			SceneController._instance.NextScene();
		}
    }

    public void RestartCurrentLevel()
    {
        if (currentLevelIndex < gameLevels.Length)
        {
            gameLevels[currentLevelIndex].SetActive(false);
            gameLevels[currentLevelIndex].SetActive(true);

            // 重置玩家
            SetPlayerPosition(currentLevelIndex);
            // 重置敵人或牆
            SetEnemiesAndWalls(currentLevelIndex);
        }

        // 如果是第四關 => 重置雙玩家完成flag
        if (currentLevelIndex == 3)
        {
            playerA4_Finished = false;
            playerB4_Finished = false;
        }
    }

    //--------------------------------------------------------------------------------
    // (D) 設定玩家位置
    //--------------------------------------------------------------------------------
    private void SetPlayerPosition(int levelIndex)
    {
        // 第一~第三關(0,1,2) 用 playerA
        if (levelIndex < 3) 
        {
            // spawnPoints[0] ~ spawnPoints[2] 分別對應
            if (levelIndex < spawnPoints.Length && spawnPoints[levelIndex] != null)
            {
                playerA.transform.position = spawnPoints[levelIndex].position;
            }
            playerA.SetActive(true);
            EnableDrag(playerA);

            // 預防性關掉 第四關玩家
            if (playerA4 != null) playerA4.SetActive(false);
            if (playerB4 != null) playerB4.SetActive(false);
        }
        // 第四關(3) => 用 playerA4, playerB4
        else if (levelIndex == 3)
        {
            // 關掉第一~三關玩家
            if (playerA != null) playerA.SetActive(false);

            // 開啟 A4
            if (playerA4 != null && spawnPointA4 != null)
            {
                playerA4.SetActive(true);
                playerA4.transform.position = spawnPointA4.position;
                EnableDrag(playerA4);
            }
            // 開啟 B4
            if (playerB4 != null && spawnPointB4 != null)
            {
                playerB4.SetActive(true);
                playerB4.transform.position = spawnPointB4.position;
                EnableDrag(playerB4);
            }
        }
        // 第五關(4) => 回到單人playerA
        else if (levelIndex == 4)
        {
            if (playerA != null && spawnPoints.Length > 3 && spawnPoints[3] != null)
            {
                // spawnPoints[3] 是第五關用
                playerA.SetActive(true);
                playerA.transform.position = spawnPoints[3].position;
                EnableDrag(playerA);
            }
            // 關掉A4, B4
            if (playerA4 != null) playerA4.SetActive(false);
            if (playerB4 != null) playerB4.SetActive(false);
        }
    }

    //--------------------------------------------------------------------------------
    // (E) 第五關牆壁/敵人設定 (或其他關的敵人)
    //--------------------------------------------------------------------------------
    private void SetEnemiesAndWalls(int levelIndex)
    {
        // 假設你有第二關敵人 or 第三關巡邏 or 第四關Z字...
        // 這裡只示範第五關牆壁 & 其他關關閉

        // 如果是第五關(index=4) => 開啟 wallA, wallB 讓它們執行 ExpandingWalls
        if (levelIndex == 4)
        {
            if (wallA != null) wallA.SetActive(true);
            if (wallB != null) wallB.SetActive(true);

            Vector3 scaleAA = wallA.transform.localScale;
            scaleAA.x = 3.628763f;
            wallA.transform.localScale = scaleAA;

            Vector3 scaleBB = wallB.transform.localScale;
            scaleBB.x = 3.646907f;
            wallB.transform.localScale = scaleBB;

        }
        else if (levelIndex == 1) // 第二關
        {
            if (enemyChase != null && enemySpawnPoint != null)
            {
                // 重新啟動敵人(若它被 SetActive(false))
                enemyChase.SetActive(true);

                // 將敵人移動到出生點
                enemyChase.transform.position = enemySpawnPoint.position;
            }
        }
        else
        {
            // 其他關關閉
            if (wallA != null) wallA.SetActive(false);
            if (wallB != null) wallB.SetActive(false);
        }
    }

    //--------------------------------------------------------------------------------
    // (F) 控制拖曳
    //--------------------------------------------------------------------------------
    private void DisableDrag(GameObject go)
    {
        if (go == null) return;
        DragObject drag = go.GetComponent<DragObject>();
        if (drag != null)
        {
            drag.ForceStopDrag();
            drag.enabled = false;
            Debug.Log("DisableDrag on " + go.name);
        }
    }
    private void EnableDrag(GameObject go)
    {
        if (go == null) return;
        DragObject drag = go.GetComponent<DragObject>();
        if (drag != null)
        {
            drag.ForceStopDrag();
            drag.enabled = true;
            Debug.Log("EnableDrag on " + go.name);
        }
    }

    public void BackToMainMenu()
    {
        Debug.Log("回到主頁面(示範)");
        // SceneManager.LoadScene("MainMenu");
    }
}
