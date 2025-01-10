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
    public Camera mainCamera; // 在 Inspector 拖入Hierarchy中的 Main Camera

    // ★ 這裡定義一個變數來存 CameraController (可以不用 public)
    private CameraControllerMoney cameraControllerMoney;

    [Header("分鏡3:圖片清單")]
    public List<CutsceneImage> cutsceneImages; 
    // 在 Inspector 中可放入圖片，並設定各自顯示與間隔時間

    [Header("分鏡5:工作人員清單")]
    public List<Text> staffList; // 顯示在螢幕上的工作名單 (一條條出現)

    [Header("黑屏 & 遊戲名稱")]
    public GameObject blackScreen;     // 最後黑屏
    public Text gameTitleText;         // 遊戲名稱

    [Header("音效 & BGM")]
    public AudioSource bgm;            // 放煙火時的柔和 BGM
    public AudioSource gunShotSFX;     // 槍聲音效
    public AudioSource reloadSFX;
    public AudioSource frontSFX;

    [Header("參數設定")]
    public float moveSpeed = 2f;       // 角色移動速度
    public float cameraMoveSpeed = 2f; // 鏡頭移動速度
    
    private void Start()
    {
        // 初始化各 UI 狀態
        //if(blackScreen != null) blackScreen.alpha = 0f; // 不顯示黑屏
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

        // ★ 取得 CameraControllerMoney
        if (mainCamera != null)
        {
            cameraControllerMoney = mainCamera.GetComponent<CameraControllerMoney>();
        }

        // 開始進行整個結尾動畫流程
        StartCoroutine(CutsceneFlow());
    }

    /// <summary>
    /// 主要流程：依序執行 7 個分鏡
    /// </summary>
    private IEnumerator CutsceneFlow()
    {
        if(frontSFX != null && !frontSFX.isPlaying)
            frontSFX.Play();
        // == 分鏡 1 ==
        // 假設：希望此時相機跟隨主角
        if(cameraControllerMoney != null)
            cameraControllerMoney.canFollow = true; 

        // AAA 從左側移動到畫面中央
        // (這裡我們呼叫 MoveCharacter 時，也可以選擇 cameraFollow:true or false)
        Coroutine moveA = StartCoroutine(MoveCharacter(
            AAA, 
            new Vector3(4f, AAA.transform.position.y, AAA.transform.position.z), 
            moveSpeed, 
            cameraFollow:true
        ));
        yield return new WaitForSeconds(2f);

        // 同時啟動 B, C, D
        Coroutine moveB = StartCoroutine(MoveCharacter(B, new Vector3(-1.95f, B.transform.position.y, B.transform.position.z), moveSpeed));
        Coroutine moveC = StartCoroutine(MoveCharacter(C, new Vector3(-3f,    C.transform.position.y, C.transform.position.z), moveSpeed));
        Coroutine moveD = StartCoroutine(MoveCharacter(D, new Vector3(-4.07f, D.transform.position.y, D.transform.position.z), moveSpeed));

        // AAA抵達後，讓他面向左
        yield return moveA;
        frontSFX.Stop();
        Vector3 scale = AAA.transform.localScale;
        scale.x = -Mathf.Abs(scale.x); 
        AAA.transform.localScale = scale;

        // 等待 B, C, D 也都抵達
        yield return moveB;
        yield return moveC;
        yield return moveD;
        yield return new WaitForSeconds(0.5f);
        

        // == 分鏡 2 ==
        // C 拿出手槍指向 AAA
        Animator animatorB = B.GetComponent<Animator>();
        animatorB.SetBool("isGun", true);
        if(reloadSFX != null) reloadSFX.Play();
        yield return new WaitForSeconds(2f);
        if(bgm != null && !bgm.isPlaying){
            bgm.Play();
            Debug.Log("bgm played!");
        }else{
            Debug.Log("bgm not played!");
        }
        yield return new WaitForSeconds(2f);

        // == 分鏡 3 ==
        yield return StartCoroutine(ShowCutsceneImages());

        // == 分鏡 4 ==
        // 此時我們要手動移動相機到 x=80 不被干擾，所以關掉相機跟隨
        if(cameraControllerMoney != null)
            cameraControllerMoney.canFollow = false;

        yield return StartCoroutine(MoveCameraRightAndFireworks());
        Debug.Log("4th camera");

        // == 分鏡 5 ==
        //yield return StartCoroutine(ShowStaffList());
        Debug.Log("5th camera");

        // == 分鏡 6 ==
        // 如果此時要再度由手動移動相機或瞬移角色...
        // 也可以再度把canFollow打開 if needed
        // if(cameraController != null) cameraController.canFollow = true;
        //yield return StartCoroutine(RevealCharactersAgain());
        Debug.Log("6th camera");

        // == 分鏡 7 ==
        yield return StartCoroutine(FinalShotAndBlackout());
        Debug.Log("7th camera");
    }

    #region 分鏡輔助方法

    /// <summary>
    /// 角色移動到指定座標 (2D 側視)，可選擇是否要局部 cameraFollow
    /// </summary>
    private IEnumerator MoveCharacter(GameObject character, Vector3 targetPos, float speed, bool cameraFollow = false)
    {
        if(character == null) yield break;

        Animator animator = character.GetComponent<Animator>();
        if(animator != null)
        {
            animator.SetBool("isRun", true);
        }

        while(Vector3.Distance(character.transform.position, targetPos) > 0.1f)
        {
            character.transform.position = Vector3.MoveTowards(
                character.transform.position,
                targetPos,
                speed * Time.deltaTime
            );

            if(cameraFollow && mainCamera != null)
            {
                // 「局部」相機跟隨：直接改變 mainCamera 的 x
                // 如果你只想靠 CameraController.canFollow 就可以把這行拿掉
                Vector3 camPos = mainCamera.transform.position;
                camPos.x = character.transform.position.x;
                mainCamera.transform.position = camPos;
            }

            yield return null;
        }

        if(animator != null)
        {
            animator.SetBool("isRun", false);
        }

        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// 分鏡3：依序顯示多張圖片
    /// </summary>
    private IEnumerator ShowCutsceneImages()
    {
        
        foreach(var ci in cutsceneImages)
        {
            if(ci.image != null)
            {
                ci.image.gameObject.SetActive(true);
                yield return new WaitForSeconds(ci.displayTime);
                ci.image.gameObject.SetActive(false);
                yield return new WaitForSeconds(ci.intervalTime);
            }
        }
    }

    /// <summary>
    /// 分鏡4：往右移到 x=80，放煙火 & BGM
    /// </summary>
    private IEnumerator MoveCameraRightAndFireworks()
    {
            

        float targetX = 48f;
        while(mainCamera.transform.position.x < targetX)
        {
            Vector3 pos = mainCamera.transform.position;
            pos.x += cameraMoveSpeed * Time.deltaTime;
            mainCamera.transform.position = pos;
            yield return null;
        }

        // 假設這裡觸發煙火
        // ...
        yield return new WaitForSeconds(2f);
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
            staffList[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 分鏡6：鏡頭再度向右移動，看到 AAA 被吊著…(示意)
    /// </summary>
    private IEnumerator RevealCharactersAgain()
    {
        //float targetX = mainCamera.transform.position.x + 8f;
        //while(mainCamera.transform.position.x < targetX)
        //{
        //    mainCamera.transform.position += new Vector3(cameraMoveSpeed * Time.deltaTime, 0f, 0f);
        //    yield return null;
        //}

        // AAA 被吊著 / C持槍…(自行設定Animator)
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 分鏡7：C開槍 -> 黑屏 -> 顯示遊戲名稱 -> 結束
    /// </summary>
    private IEnumerator FinalShotAndBlackout()
    {
        bgm.Stop();
        yield return new WaitForSeconds(3f);
        if(gunShotSFX != null) gunShotSFX.Play();
        blackScreen.SetActive(true);
        yield return new WaitForSeconds(3f);
        // Application.Quit();
    }

    #endregion
}
