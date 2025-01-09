using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// 可選：給定一個可序列化的結構，方便自訂每張圖片的顯示與間隔時間
[System.Serializable]
public class CutsceneImage
{
    public Image image;        // Inspector 中拖入該張圖片
    public float displayTime;  // 顯示多久
    public float intervalTime; // 與下一張圖片之間間隔多久
}

public class CutsceneControllerMoney : MonoBehaviour
{
    [Header("角色物件")]
    public GameObject AAA;  // 主角
    public GameObject B;
    public GameObject C;
    public GameObject D;

    [Header("攝影機")]
    public Camera mainCamera;

    [Header("分鏡3:圖片清單")]
    public List<CutsceneImage> cutsceneImages; 
    // 在 Inspector 中可放入 4 張圖片，並設定各自顯示與間隔時間

    [Header("分鏡5:工作人員清單")]
    public List<Text> staffList; // 顯示在螢幕上的工作名單 (一條條出現)

    [Header("黑屏 & 遊戲名稱")]
    public CanvasGroup blackScreen;     // 最後黑屏
    public Text gameTitleText;         // 遊戲名稱

    [Header("音效 & BGM")]
    public AudioSource bgm;            // 放煙火時的柔和 BGM
    public AudioSource gunShotSFX;     // 槍聲音效

    [Header("參數設定")]
    public float moveSpeed = 2f;       // 角色移動速度
    public float cameraMoveSpeed = 2f; // 鏡頭移動速度
    
    private void Start()
    {
        // 初始化各 UI 狀態
        if(blackScreen != null) blackScreen.alpha = 0f; // 不顯示黑屏
        if(gameTitleText != null) gameTitleText.enabled = false;

        // 分鏡 3 的圖片預設先隱藏
        foreach (var ci in cutsceneImages)
        {
            if(ci.image != null)
                ci.image.gameObject.SetActive(false);
        }

        // 分鏡 5 工作人員名單預設隱藏
        foreach (var txt in staffList)
        {
            if(txt != null)
                txt.gameObject.SetActive(false);
        }

        // 開始進行整個結尾動畫流程
        StartCoroutine(CutsceneFlow());
    }

    /// <summary>
    /// 主要流程：依序執行 7 個分鏡
    /// </summary>
    private IEnumerator CutsceneFlow()
    {
        // == 分鏡 1 ==
        // AAA從左側(假設初始位置)移動到畫面中央，鏡頭跟隨；停下後向左看 -> B,C,D 依序出場
        Coroutine moveA = StartCoroutine(MoveCharacter(AAA, new Vector3(4f, AAA.transform.position.y, AAA.transform.position.z), 
                                                  moveSpeed, cameraFollow:true));
        yield return new WaitForSeconds(2f);

        // 同時啟動三個角色移動
        Coroutine moveB = StartCoroutine(MoveCharacter(B, new Vector3(-1.95f, B.transform.position.y, B.transform.position.z), moveSpeed));
        Coroutine moveC = StartCoroutine(MoveCharacter(C, new Vector3(-3f, C.transform.position.y, C.transform.position.z), moveSpeed));
        Coroutine moveD = StartCoroutine(MoveCharacter(D, new Vector3(-4.07f, D.transform.position.y, D.transform.position.z), moveSpeed));

       // AAA到達目標點，稍微等待後再讓他面向左側
        yield return moveA;
        Debug.Log("AAA arrived");
        yield return new WaitForSeconds(0.5f);
        Vector3 scale = AAA.transform.localScale;
        scale.x = -Mathf.Abs(scale.x); 
        AAA.transform.localScale = scale;
        Debug.Log("Flip");

        // 等待全部 Coroutine 結束
        yield return moveB;
        yield return moveC;
        yield return moveD;
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Three arrived");

        // 到此時，三位配角都已經到達指定位置



        // == 分鏡 2 ==
        // C 拿出手槍指向 AAA (此處用 WaitForSeconds 代表給點時間給掏槍動畫)
        Animator animatorB = B.GetComponent<Animator>();
        animatorB.SetBool("isGun", true);
        yield return new WaitForSeconds(4f);

        Debug.Log("2nd camera");

        // == 分鏡 3 ==
        // 依序顯示多張圖片(每張圖片都已包含藍底白字)
        yield return StartCoroutine(ShowCutsceneImages());
        Debug.Log("3rd camera");

        // == 分鏡 4 ==
        // 場景回復，四人不動，鏡頭向右移動直到看不到四人；播放煙火 & 柔和 BGM
        yield return StartCoroutine(MoveCameraRightAndFireworks());
        Debug.Log("4th camera");

        // == 分鏡 5 ==
        // 一條一條顯示工作人員清單
        yield return StartCoroutine(ShowStaffList());
        Debug.Log("5th camera");

        // == 分鏡 6 ==
        // 鏡頭繼續向右移，一段後再看到四人，此時AAA被吊起來、C持槍(自行切換動畫)
        yield return StartCoroutine(RevealCharactersAgain());
        Debug.Log("6th camera");

        // == 分鏡 7 ==
        // C 開槍，播放槍聲 -> 畫面黑屏 -> 顯示遊戲名稱 -> 結束
        yield return StartCoroutine(FinalShotAndBlackout());
        Debug.Log("7th camera");
    }

    #region 分鏡輔助方法

    /// <summary>
    /// 角色移動到指定座標 (2D 側視)，可選擇是否攝影機跟隨。
    /// </summary>
    /// <summary>
/// 角色移動到指定座標 (2D 側視)，可選擇是否攝影機跟隨。
/// 在移動時啟動跑步動畫(isRun = true)，到達後關閉(isRun = false)。
/// </summary>
private IEnumerator MoveCharacter(GameObject character, Vector3 targetPos, float speed, bool cameraFollow = false)
{
    if(character == null) yield break;

    // 1. 取得角色上的 Animator
    Animator animator = character.GetComponent<Animator>();
    if(animator != null)
    {
        // 進入「跑步」狀態
        animator.SetBool("isRun", true);
    }

    // 2. 持續移動角色，直到抵達目標點
    while(Vector3.Distance(character.transform.position, targetPos) > 0.1f)
    {
        character.transform.position = Vector3.MoveTowards(
            character.transform.position,
            targetPos,
            speed * Time.deltaTime
        );

        // 如果需要攝影機跟隨，就更新攝影機 x 座標到角色所在
        if(cameraFollow && mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(
                character.transform.position.x,
                mainCamera.transform.position.y,
                mainCamera.transform.position.z
            );
        }

        yield return null; 
    }

    // 3. 到達後，切回「待機」狀態
    if(animator != null)
    {
        animator.SetBool("isRun", false);
    }

    // 移動結束後，稍微暫停片刻 (看需求是否保留)
    yield return new WaitForSeconds(0.5f);
}


    /// <summary>
    /// 讓角色面向左(簡易 2D做法，可用 flipX 或動畫參數)
    /// </summary>
    private void FlipCharacterLeft(GameObject character)
    {
        // 以下只是簡單的 flipX 範例
        SpriteRenderer sr = character.GetComponent<SpriteRenderer>();
        if(sr != null) sr.flipX = true;
    }

    /// <summary>
    /// 分鏡3：依序顯示圖片
    /// </summary>
    private IEnumerator ShowCutsceneImages()
    {
        foreach(var ci in cutsceneImages)
        {
            // 顯示圖片
            if(ci.image != null)
            {
                ci.image.gameObject.SetActive(true);
                // 顯示時間
                yield return new WaitForSeconds(ci.displayTime);
                // 隱藏
                ci.image.gameObject.SetActive(false);
                // 與下一張圖片的間隔
                yield return new WaitForSeconds(ci.intervalTime);
            }
        }
    }

    /// <summary>
    /// 分鏡4：鏡頭往右移，直到看不到四人；此時釋放煙火、播放BGM
    /// </summary>
    private IEnumerator MoveCameraRightAndFireworks()
    {
        // 播放 BGM (如果尚未播放)
        if(bgm != null && !bgm.isPlaying)
            bgm.Play();

        // 假設往右移 10 (或自行調整)
        float targetX = mainCamera.transform.position.x + 10f;
        while(mainCamera.transform.position.x < 60f)
        {
            mainCamera.transform.position += new Vector3(cameraMoveSpeed * Time.deltaTime, 0f, 0f);
            yield return null;
        }

        // 這裡可以觸發煙火特效(若有粒子特效可 Play)
        // ...
        yield return new WaitForSeconds(2f); // 留點時間讓煙火看起來更持續
    }

    /// <summary>
    /// 分鏡5：一條一條顯示工作人員名單
    /// </summary>
    private IEnumerator ShowStaffList()
    {
        for(int i = 0; i < staffList.Count; i++)
        {
            staffList[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);

            // 顯示完後隱藏，或可選擇保持顯示
            staffList[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 分鏡6：鏡頭再度向右移動，直到出現AAA被吊著 & C持槍
    /// </summary>
    private IEnumerator RevealCharactersAgain()
    {
        float targetX = mainCamera.transform.position.x + 8f;
        while(mainCamera.transform.position.x < targetX)
        {
            mainCamera.transform.position += new Vector3(cameraMoveSpeed * Time.deltaTime, 0f, 0f);
            yield return null;
        }

        // 此時切換 AAA 狀態 -> 被吊著；C 持槍(可以在角色Animator觸發對應動作)
        // ...

        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 分鏡7：C開槍，黑屏淡入，顯示遊戲名稱，結束
    /// </summary>
    private IEnumerator FinalShotAndBlackout()
    {
        // 播槍聲
        if(gunShotSFX != null) gunShotSFX.Play();

        // 稍微延遲，再開始黑屏
        yield return new WaitForSeconds(0.5f);

        // 黑屏淡入
        float fadeDuration = 2f;
        float timeElapsed = 0f;
        while(timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            if(blackScreen != null)
                blackScreen.alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);

            yield return null;
        }

        // 顯示遊戲名稱
        if(gameTitleText != null)
            gameTitleText.enabled = true;

        // 再等個幾秒結束過場
        yield return new WaitForSeconds(3f);

        // 可在此結束遊戲或回主選單
        // Application.Quit(); 
        // 或 UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    #endregion
}
