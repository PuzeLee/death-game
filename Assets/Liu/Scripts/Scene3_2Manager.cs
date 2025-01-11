using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public enum ItemType
{
    Magnifier,
    Cigarette,
    Beer,
    Saw,
    Handcuffs
}

public interface IItemEffect
{
    IEnumerator Execute(Scene3_2Manager manager, bool isPlayer);
}

public class Scene3_2Manager : MonoBehaviour
{
    [Header("UI References")]
    public Text bulletCountText;
    public Text playerHealthText;
    public Text enemyHealthText;
    public Text statusText;
    public Text bulletStatusText;
    public Button shootEnemyButton;
    public Button shootPlayerButton;

    public Button magnifierButton;
    public Button cigaretteButton; 
    public Button beerButton;
    public Button sawButton;
    public Button handcuffsButton;

    public Text magnifierText;
    public Text cigaretteText;
    public Text beerText;
    public Text sawText;
    public Text handcuffsText;
    public bool isMagnifierUsed = false;

    [Header("Dialog UI")]
    public GameObject dialogPanel;
    public Text dialogText;
    public Button dialogButton;

    [Header("Video Clips")]
    public VideoPlayer videoPlayer;
    public VideoClip py0, py1, ps0, ps1, ey0, ey1, es0, es1;

    [Header("Game Settings")]
    [SerializeField] public int firstRoundHealth = 2;
    [SerializeField] public int secondRoundHealth = 4;
    [SerializeField] public float messageDisplayDuration = 3f;
    
    public List<bool> bulletList = new List<bool>();
    public int playerHealth;
    public int enemyHealth;
    public bool isPlayerTurn = true;
    public bool isProcessing = false;
    public int currentRound = 1;
    public Dictionary<ItemType, IItemEffect> itemEffects;
    public List<ItemType> playerItems = new List<ItemType>();
    public List<ItemType> enemyItems = new List<ItemType>();
    public bool isPlayerHandcuffed = false;
    public bool isEnemyHandcuffed = false;
    public bool isSawActive = false;

    public string startGameText = "Game Rules:\n\n" +
        "1. Take turns shooting at each other or yourself\n" +
        "2. Some bullets are real, some are empty\n" +
        "3. In Round 2, items will be introduced\n\n" +
        "Warning: This is a death game...";

    public string round2Text = "Round 2 Items:\n\n" +
        "Magnifier: Check current bullet\n" +
        "Cigarette: Recover 1 HP\n" +
        "Beer: Pop current bullet and skip turn\n" +
        "Saw: Deal 2 damage if real bullet\n" +
        "Handcuffs: Skip opponent's next turn";

    public void Start()
    {
        SetupButtonHoverEffects();
        ShowDialog(startGameText, "I Agree", InitializeGame);
    }

    private void SetupButtonHoverEffects()
    {
        SetupButtonHover(shootEnemyButton);
        SetupButtonHover(shootPlayerButton);
        SetupButtonHover(magnifierButton);
        SetupButtonHover(cigaretteButton);
        SetupButtonHover(beerButton);
        SetupButtonHover(sawButton);
        SetupButtonHover(handcuffsButton);
        SetupButtonHover(dialogButton);
    }

    private void SetupButtonHover(Button button)
    {
        var colors = button.colors;
        var textComponent = button.GetComponentInChildren<Text>();
        
        // 設置按鈕顏色
        colors.normalColor = new Color(1f, 1f, 1f, 1f);
        colors.highlightedColor = new Color(1f, 0.8f, 0.8f, 1f);
        colors.pressedColor = new Color(1f, 0.6f, 0.6f, 1f);
        colors.selectedColor = new Color(1f, 1f, 1f, 1f);
        colors.fadeDuration = 0.1f;
        button.colors = colors;

        if (textComponent != null)
        {
            // 獲取或添加 EventTrigger 組件
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = button.gameObject.AddComponent<EventTrigger>();

            // 清除既有的觸發器
            trigger.triggers.Clear();

            // 添加滑鼠進入事件
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => { textComponent.color = new Color(1f, 0f, 0f, 0.5f); });
            trigger.triggers.Add(enterEntry);

            // 添加滑鼠離開事件
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => { textComponent.color = new Color(1f, 1f, 1f, 1f); });
            trigger.triggers.Add(exitEntry);

            // 添加按下事件
            EventTrigger.Entry downEntry = new EventTrigger.Entry();
            downEntry.eventID = EventTriggerType.PointerDown;
            downEntry.callback.AddListener((data) => { textComponent.color = new Color(1f, 0f, 0f, 1f); });
            trigger.triggers.Add(downEntry);

            // 添加放開事件
            EventTrigger.Entry upEntry = new EventTrigger.Entry();
            upEntry.eventID = EventTriggerType.PointerUp;
            upEntry.callback.AddListener((data) => { textComponent.color = new Color(1f, 1f, 1f, 1f); });
            trigger.triggers.Add(upEntry);
        }
    }

    private void ShowDialog(string message, string buttonText, System.Action onClose)
    {
        // 隐藏所有其他UI元素
        SetAllUIElementsVisible(false);
        SetAllItemUIElements(false);
        
        // 显示对话框
        dialogPanel.SetActive(true);
        dialogText.text = message;
        dialogButton.GetComponentInChildren<Text>().text = buttonText;
        
        dialogButton.onClick.RemoveAllListeners();
        dialogButton.onClick.AddListener(() => {
            dialogPanel.SetActive(false);
            onClose?.Invoke();
            // 如果不在处理中，则显示UI元素
            if (!isProcessing && isPlayerTurn)
            {
                SetAllUIElementsVisible(true);
            }
        });
    }

    private void SetAllUIElementsVisible(bool visible)
    {
        bulletCountText.gameObject.SetActive(visible);
        playerHealthText.gameObject.SetActive(visible);
        enemyHealthText.gameObject.SetActive(visible);
        statusText.gameObject.SetActive(visible);
        bulletStatusText.gameObject.SetActive(visible);
        shootEnemyButton.gameObject.SetActive(visible);
        shootPlayerButton.gameObject.SetActive(visible);
        
        // 如果在第二回合且不是在顯示對話框時，才處理道具的顯示
        if (currentRound == 2 && visible && !dialogPanel.activeSelf)
        {
            UpdateItemUI();
        }
        else
        {
            SetAllItemUIElements(false);
        }
    }

    public void InitializeGame()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.gameObject.SetActive(false);
        InitializeItemEffects();
        currentRound = 1;
        SetupRound(1);
    }

    public void InitializeItemEffects()
    {
        itemEffects = new Dictionary<ItemType, IItemEffect>
        {
            { ItemType.Magnifier, new MagnifierEffect() },
            { ItemType.Cigarette, new CigaretteEffect() },
            { ItemType.Beer, new BeerEffect() },
            { ItemType.Saw, new SawEffect() },
            { ItemType.Handcuffs, new HandcuffsEffect() }
        };
    }

    public void SetupRound(int round)
    {
        currentRound = round;
        playerHealth = round == 1 ? firstRoundHealth : secondRoundHealth;
        enemyHealth = round == 1 ? firstRoundHealth : secondRoundHealth;
        isPlayerTurn = true;
        isProcessing = false;
        isPlayerHandcuffed = false;
        isEnemyHandcuffed = false;
        isSawActive = false;
        
        shootEnemyButton.onClick.RemoveAllListeners();
        shootPlayerButton.onClick.RemoveAllListeners();
        shootEnemyButton.onClick.AddListener(() => OnPlayerShoot(true));
        shootPlayerButton.onClick.AddListener(() => OnPlayerShoot(false));

        SetAllItemUIElements(false);

        if (round == 2)
        {
            playerItems.Clear();
            enemyItems.Clear();
            DistributeItems();
            ShowDialog(round2Text, "I Understand", () => {
                GenerateBullets();
                StartCoroutine(ShowSequentialMessages(round));
            });
        }
        else
        {
            GenerateBullets();
            StartCoroutine(ShowSequentialMessages(round));
        }
    }

    private void SetAllItemUIElements(bool visible)
    {
        // 設置所有道具相關UI為指定狀態
        magnifierButton.gameObject.SetActive(false);
        cigaretteButton.gameObject.SetActive(false);
        beerButton.gameObject.SetActive(false);
        sawButton.gameObject.SetActive(false);
        handcuffsButton.gameObject.SetActive(false);

        magnifierText.gameObject.SetActive(false);
        cigaretteText.gameObject.SetActive(false);
        beerText.gameObject.SetActive(false);
        sawText.gameObject.SetActive(false);
        handcuffsText.gameObject.SetActive(false);
    }

    public IEnumerator ShowSequentialMessages(int round)
    {
        SetUIElementsActive(false);  // 確保所有按鈕隱藏

        UpdateUI();
        
        // 顯示回合開始
        yield return StartCoroutine(ShowGameMessage($"Round {round} Start"));
        
        // 如果是第二回合，顯示道具分配
        if (round == 2)
        {
            yield return StartCoroutine(ShowGameMessage($"Player received {string.Join(", ", playerItems)}, Luna received {string.Join(", ", enemyItems)}"));
        }
        
        bulletStatusText.text = "";
        
        // 最後才顯示按鈕
        if (isPlayerTurn && !isProcessing)
            SetUIElementsActive(true);
        else
            StartCoroutine(ProcessEnemyTurn());
    }

    public void GenerateBullets()
    {
        bulletList.Clear();
        int emptyCount = Random.Range(1, 4);
        int realCount = Random.Range(1, 5 - emptyCount);

        for (int i = 0; i < emptyCount; i++) bulletList.Add(false);
        for (int i = 0; i < realCount; i++) bulletList.Add(true);
        bulletList.Shuffle();

        SetUIElementsActive(false);
        bulletStatusText.text = $"Empty: {emptyCount}, Real: {realCount}";
        bulletCountText.text = $"Bullets: {bulletList.Count}";

        if (currentRound == 2)
        {
            ReplenishItems();
        }
    }

    public void ReplenishItems()
    {
        while (playerItems.Count < 3)
        {
            ItemType newItem = (ItemType)Random.Range(0, System.Enum.GetValues(typeof(ItemType)).Length);
            if (!playerItems.Contains(newItem))
                playerItems.Add(newItem);
        }
        while (enemyItems.Count < 3)
        {
            ItemType newItem = (ItemType)Random.Range(0, System.Enum.GetValues(typeof(ItemType)).Length);
            if (!enemyItems.Contains(newItem))
                enemyItems.Add(newItem);
        }
        StartCoroutine(ShowGameMessage($"Player received {string.Join(", ", playerItems)}, Luna received {string.Join(", ", enemyItems)}"));
        UpdateItemUI();
    }

    public void DistributeItems()
    {
        playerItems.Clear();
        enemyItems.Clear();

        playerItems = GenerateUniqueItems(3);
        enemyItems = GenerateUniqueItems(3);
        UpdateItemUI();
    }

    private List<ItemType> GenerateUniqueItems(int count)
    {
        List<ItemType> allItems = new List<ItemType>((ItemType[])System.Enum.GetValues(typeof(ItemType)));
        List<ItemType> selectedItems = new List<ItemType>();

        for (int i = 0; i < count; i++)
        {
            if (allItems.Count == 0) break;
            int index = Random.Range(0, allItems.Count);
            selectedItems.Add(allItems[index]);
            allItems.RemoveAt(index);
        }

        return selectedItems;
    }

    public void UpdateItemUI()
    {
        // 清除之前的所有監聽器
        magnifierButton.onClick.RemoveAllListeners();
        cigaretteButton.onClick.RemoveAllListeners();
        beerButton.onClick.RemoveAllListeners();
        sawButton.onClick.RemoveAllListeners();
        handcuffsButton.onClick.RemoveAllListeners();

        // 更新按鈕和文字的顯示狀態
        magnifierButton.gameObject.SetActive(playerItems.Contains(ItemType.Magnifier));
        cigaretteButton.gameObject.SetActive(playerItems.Contains(ItemType.Cigarette));
        beerButton.gameObject.SetActive(playerItems.Contains(ItemType.Beer));
        sawButton.gameObject.SetActive(playerItems.Contains(ItemType.Saw));
        handcuffsButton.gameObject.SetActive(playerItems.Contains(ItemType.Handcuffs));

        magnifierText.gameObject.SetActive(enemyItems.Contains(ItemType.Magnifier));
        cigaretteText.gameObject.SetActive(enemyItems.Contains(ItemType.Cigarette));
        beerText.gameObject.SetActive(enemyItems.Contains(ItemType.Beer));
        sawText.gameObject.SetActive(enemyItems.Contains(ItemType.Saw));
        handcuffsText.gameObject.SetActive(enemyItems.Contains(ItemType.Handcuffs));

        // 重新添加按鈕監聽器
        magnifierButton.onClick.AddListener(() => StartCoroutine(UseItem(ItemType.Magnifier, true)));
        cigaretteButton.onClick.AddListener(() => StartCoroutine(UseItem(ItemType.Cigarette, true)));
        beerButton.onClick.AddListener(() => StartCoroutine(UseItem(ItemType.Beer, true)));
        sawButton.onClick.AddListener(() => StartCoroutine(UseItem(ItemType.Saw, true)));
        handcuffsButton.onClick.AddListener(() => StartCoroutine(UseItem(ItemType.Handcuffs, true)));
    }

    public IEnumerator UseItem(ItemType item, bool isPlayer)
    {
        if (isProcessing) yield break;
        
        isProcessing = true;
        string effectDescription = GetItemEffectDescription(item);
        yield return StartCoroutine(ShowItemUsage($"{(isPlayer ? "Player" : "Luna")} used {item}, {effectDescription}"));
        
        if (isPlayer)
            playerItems.Remove(item);
        else
            enemyItems.Remove(item);
            
        UpdateItemUI();
        yield return StartCoroutine(itemEffects[item].Execute(this, isPlayer));
        
        isProcessing = false;
    }

    private string GetItemEffectDescription(ItemType item)
    {
        switch (item)
        {
            case ItemType.Magnifier:
                return "check current bullet";
            case ItemType.Cigarette:
                return "recover 1 HP";
            case ItemType.Beer:
                return "pop up the current bullet and skip current turn";
            case ItemType.Saw:
                return "deal 2 damage if real bullet";
            case ItemType.Handcuffs:
                return "skip opponent's next turn";
            default:
                return "";
        }
    }

    public void UpdateUI()
    {
        string playerHealthStr = "";
        string enemyHealthStr = "";
        
        for(int i = 0; i < playerHealth; i++)
        {
            playerHealthStr += "♥";
        }
        for(int i = 0; i < enemyHealth; i++)
        {
            enemyHealthStr += "♥";
        }
        
        playerHealthText.text = $"You: {playerHealthStr}";
        enemyHealthText.text = $"Luna: {enemyHealthStr}";
    }

    public void OnPlayerShoot(bool targetIsEnemy)
    {
        if (isProcessing || !isPlayerTurn) return;
        ProcessShot(targetIsEnemy);
    }

    public void ProcessShot(bool targetIsEnemy)
    {
        if (bulletList.Count == 0 || CheckGameOver()) return;

        isProcessing = true;
        SetUIElementsActive(false);
        
        // 先播放影片，完成後再處理子彈邏輯
        PlayShootingVideo(bulletList[0], targetIsEnemy);
    }

    public void PlayShootingVideo(bool isRealBullet, bool targetIsEnemy)
    {
        VideoClip clipToPlay;
        System.Action onComplete;

        if (isPlayerTurn)
        {
            clipToPlay = isRealBullet ? (targetIsEnemy ? py1 : ps1) : (targetIsEnemy ? py0 : ps0);
            onComplete = () => FinishShot(isRealBullet, targetIsEnemy, true);
        }
        else
        {
            clipToPlay = isRealBullet ? (targetIsEnemy ? es1 : ey1) : (targetIsEnemy ? es0 : ey0);
            onComplete = () => FinishShot(isRealBullet, targetIsEnemy, false);
        }

        if (clipToPlay == null)
        {
            Debug.LogError("Missing video clip");
            return;
        }

        videoPlayer.clip = clipToPlay;
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
        StartCoroutine(WaitForVideoEnd(onComplete));
    }

    private void FinishShot(bool isRealBullet, bool targetIsEnemy, bool wasPlayerTurn)
    {
        // 在影片播放完成後才移除子彈
        bulletList.RemoveAt(0);
        ProcessShotResult(isRealBullet, targetIsEnemy, wasPlayerTurn);
    }

    public void ProcessShotResult(bool isRealBullet, bool targetIsEnemy, bool wasPlayerTurn)
    {   
        if (isRealBullet)
        {
            int damage = isSawActive ? 2 : 1;
            if (targetIsEnemy) 
                enemyHealth -= damage;
            else 
                playerHealth -= damage;
            
            isSawActive = false;
            
            if (wasPlayerTurn)
            {
                if (isEnemyHandcuffed)
                {
                    Debug.Log("Enemy is handcuffed, staying on player's turn");
                    isEnemyHandcuffed = false;
                    isPlayerTurn = true;
                }
                else
                {
                    isPlayerTurn = false;
                }
            }
            else
            {
                if (isPlayerHandcuffed)
                {
                    isPlayerHandcuffed = false;
                    isPlayerTurn = false;
                }
                else
                {
                    isPlayerTurn = true;
                }
            }
        }
        else
        {
            if (wasPlayerTurn)
            {
                if (targetIsEnemy)
                {
                    if (isEnemyHandcuffed)
                    {
                        isEnemyHandcuffed = false;
                        isPlayerTurn = true;
                    }
                    else
                    {
                        isPlayerTurn = false;
                    }
                }
            }
            else
            {
                if (!targetIsEnemy)
                {
                    if (isPlayerHandcuffed)
                    {
                        isPlayerHandcuffed = false;
                        isPlayerTurn = false;
                    }
                    else
                    {
                        isPlayerTurn = true;
                    }
                }
            }
        }

        UpdateUI();
        bulletCountText.text = $"Bullets: {bulletList.Count}";
        isMagnifierUsed = false;

        if (CheckGameOver())
        {
            if (currentRound == 1)
                StartCoroutine(EndRoundOne());
            else
            {
                // StartCoroutine(ShowGameMessage("Game Over"));
				if (playerHealth == 0)
				{
					SceneController._instance.LoadScene("Scene3-1");
				}
				else if (enemyHealth == 0)
				{
					SceneController._instance.NextScene();
				}
            }
            
            return;
        }

        if (bulletList.Count == 0 && !CheckGameOver())
        {
            GenerateBullets();
            StartCoroutine(ContinueAfterNewBullets(wasPlayerTurn));
        }
        else
        {
            StartCoroutine(EndTurnAndContinue());
        }
    }

    public IEnumerator ContinueAfterNewBullets(bool wasPlayerTurn)
    {
        yield return new WaitForSeconds(messageDisplayDuration);
        bulletStatusText.text = "";
        isProcessing = false;
        
        if (isPlayerTurn && !CheckGameOver())
        {
            SetUIElementsActive(true);
        }
        else if (!CheckGameOver())
        {
            StartCoroutine(ProcessEnemyTurn());
        }
    }

    private IEnumerator EndRoundOne()
    {
        SetUIElementsActive(false);
        yield return StartCoroutine(ShowGameMessage("Round 1 Complete"));
        SetupRound(2);
    }

    public IEnumerator EndTurnAndContinue()
    {
        yield return new WaitForSeconds(0.25f);
        isProcessing = false;
        
        if (isPlayerTurn&& !CheckGameOver())
            SetUIElementsActive(true);
        else if (!CheckGameOver())
            StartCoroutine(ProcessEnemyTurn());
    }

    public IEnumerator ContinueSameTurn(bool wasPlayerTurn)
    {
        yield return new WaitForSeconds(0.25f);
        isProcessing = false;
        
        if (!wasPlayerTurn && !CheckGameOver())
            StartCoroutine(EnemyShoot());
        else
            SetUIElementsActive(true);
    }

    public IEnumerator EnemyShoot()
    {
        if (isProcessing || isPlayerTurn) yield break;
    
        isProcessing = true;
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(ProcessEnemyTurn());
    }

    public IEnumerator ProcessEnemyTurn()
    {
        if (CheckGameOver()) yield break;

        bool sawUsed = false;

        if (currentRound == 2)
        {
            List<ItemType> itemsToCheck = new List<ItemType>(enemyItems);
            foreach (var item in itemsToCheck)
            {
                if (CheckGameOver()) yield break;
                
                if (ShouldEnemyUseItem(item))
                {
                    yield return StartCoroutine(UseItem(item, false));
                    if (item == ItemType.Saw)
                    {
                        sawUsed = true;
                    }
                    if (isPlayerTurn || CheckGameOver()) 
                        yield break;
                }
            }
        }

        // 確保處於敵人回合且遊戲未結束
        if (!isPlayerTurn && !CheckGameOver())
        {
            bool targetIsEnemy = sawUsed ? false : DecideEnemyTarget();
            
            // 確保不會立即處理射擊
            yield return new WaitForSeconds(0.5f);
            ProcessShot(targetIsEnemy);
        }
    }

    public bool ShouldEnemyUseItem(ItemType item)
    {
        switch (item)
        {
            case ItemType.Cigarette:
                return enemyHealth < 4;

            case ItemType.Magnifier:
                isMagnifierUsed = true;
                return true;

            case ItemType.Saw:
                return bulletList.Count > 0 && bulletList[0] && isMagnifierUsed;

            case ItemType.Handcuffs:
                return Random.value < 0.2f;

            case ItemType.Beer:
                return Random.value < 0f;

            default:
                return false;
        }
    }


    public bool DecideEnemyTarget()
    {
        if (isMagnifierUsed && bulletList.Count > 0)
        {
            bool isRealBullet = bulletList[0];
            return !isRealBullet; // 如果是實彈射擊玩家(return false)，虛彈射擊自己(return true)
        }

        int emptyCount = bulletList.FindAll(b => !b).Count;
        int realCount = bulletList.FindAll(b => b).Count;

        if (emptyCount > realCount) return true;
        if (emptyCount < realCount) return false;
        return Random.value > 0.5f;
    }

    public void PlayVideo(VideoClip clip, System.Action onComplete)
    {
        if (clip == null)
        {
            Debug.LogError("Missing video clip");
            return;
        }

        videoPlayer.clip = clip;
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
        StartCoroutine(WaitForVideoEnd(onComplete));
    }

    public IEnumerator WaitForVideoEnd(System.Action onComplete)
    {
        float videoLength = (float)videoPlayer.clip.length;

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(videoLength); // 額外緩衝時間
        videoPlayer.Stop();
        videoPlayer.gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    public void OnVideoEnd(VideoPlayer vp)
    {
        if (!vp.isPlaying)
        {
            vp.gameObject.SetActive(false);
        }
    }

    public bool CheckGameOver()
    {
        return enemyHealth <= 0 || playerHealth <= 0;
    }

    public IEnumerator ShowGameMessage(string message)
    {
        statusText.text = message;
        yield return new WaitForSeconds(messageDisplayDuration);
        statusText.text = "";
    }

    public IEnumerator ShowItemUsage(string message)
    {
        statusText.text = message;
        yield return new WaitForSeconds(messageDisplayDuration);
        statusText.text = "";
    }

    public void SetUIElementsActive(bool active)
    {
        shootEnemyButton.gameObject.SetActive(active);
        shootPlayerButton.gameObject.SetActive(active);
        
        magnifierButton.gameObject.SetActive(active && playerItems.Contains(ItemType.Magnifier));
        cigaretteButton.gameObject.SetActive(active && playerItems.Contains(ItemType.Cigarette));
        beerButton.gameObject.SetActive(active && playerItems.Contains(ItemType.Beer));
        sawButton.gameObject.SetActive(active && playerItems.Contains(ItemType.Saw));
        handcuffsButton.gameObject.SetActive(active && playerItems.Contains(ItemType.Handcuffs));

        magnifierText.gameObject.SetActive(active && enemyItems.Contains(ItemType.Magnifier));
        cigaretteText.gameObject.SetActive(active && enemyItems.Contains(ItemType.Cigarette));
        beerText.gameObject.SetActive(active && enemyItems.Contains(ItemType.Beer));
        sawText.gameObject.SetActive(active && enemyItems.Contains(ItemType.Saw));
        handcuffsText.gameObject.SetActive(active && enemyItems.Contains(ItemType.Handcuffs));
    }
}

public class MagnifierEffect : IItemEffect
{
    public IEnumerator Execute(Scene3_2Manager manager, bool isPlayer)
    {
        if (manager.bulletList.Count > 0)
        {
            string message = manager.bulletList[0] ? "Next bullet is real" : "Next bullet is empty";
            yield return manager.StartCoroutine(manager.ShowItemUsage(message));
        }
    }
}

public class CigaretteEffect : IItemEffect
{
    public IEnumerator Execute(Scene3_2Manager manager, bool isPlayer)
    {
        if (isPlayer)
            manager.playerHealth = Mathf.Min(manager.playerHealth + 1, manager.currentRound == 1 ? manager.firstRoundHealth : manager.secondRoundHealth);
        else
            manager.enemyHealth = Mathf.Min(manager.enemyHealth + 1, manager.currentRound == 1 ? manager.firstRoundHealth : manager.secondRoundHealth);
            
        manager.UpdateUI();
        yield break;
    }
}

public class BeerEffect : IItemEffect
{
    public IEnumerator Execute(Scene3_2Manager manager, bool isPlayer)
    {
        if (manager.bulletList.Count > 0)
        {
            Debug.Log("Beer effect activated");
            string message = manager.bulletList[0] ? "Current bullet is real" : "Current bullet is empty";
            manager.bulletList.RemoveAt(0);
            manager.bulletCountText.text = $"Bullets: {manager.bulletList.Count}";

            if (manager.bulletList.Count == 0)
            {
                manager.GenerateBullets();
            }

            manager.isPlayerTurn = !isPlayer;

            if (isPlayer)
            {
                Debug.Log("Player used beer, triggering enemy turn");
                yield return new WaitForSeconds(manager.messageDisplayDuration);

                if (manager.isPlayerTurn)
                {
                    // 確保玩家按鈕顯示
                    manager.SetUIElementsActive(true);
                }
                else
                {
                    // 觸發敵人回合
                    manager.StartCoroutine(manager.ProcessEnemyTurn());
                }
            }
            else
            {
                // 確保敵人回合邏輯執行
                yield return manager.StartCoroutine(manager.ProcessEnemyTurn());
            }
        }
        yield break;
    }
}

public class SawEffect : IItemEffect
{
    public IEnumerator Execute(Scene3_2Manager manager, bool isPlayer)
    {
        manager.isSawActive = true;
        yield break;
    }
}

public class HandcuffsEffect : IItemEffect
{
    public IEnumerator Execute(Scene3_2Manager manager, bool isPlayer)
    {
        if (isPlayer)
            manager.isEnemyHandcuffed = true;
        else
            manager.isPlayerHandcuffed = true;
        yield break;
    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}