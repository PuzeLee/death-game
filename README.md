# Death Game
* [GitHub 連結](https://github.com/PuzeLee/death-game)
* [Demo 影片連結](https://www.youtube.com/watch?v=QkwtMe3pX7M)
* [Canva 簡報連結](https://www.canva.com/design/DAGbvyfn5nc/2svSkbdcWSwOuOL67Fej8g/view?utm_content=DAGbvyfn5nc&utm_campaign=designshare&utm_medium=link2&utm_source=uniquelinks&utlId=h9661b54a4a)
* [HackMD 連結](https://hackmd.io/skaxwQOZRS20dLIAM-rJog?view)

## 協作者名單
* [12yuuuu](https://github.com/12yuuuu)
* [matt930415](https://github.com/matt930415)

## 專案使用說明
* `/DeathGame`：此資料夾為 Windows 版本的執行檔，打開可直接遊玩
* 若想以 Unity 開啟遊戲專案：
  * Unity Version: Unity 6 (6000.0.28f1) `LTS`
  * 請先打開 `/Assets/Lee/Scenes/Scene1-0`，此為開頭畫面
  * 將執行畫面設為 Full HD (1920x1080)

## 遊戲說明
### 遊戲靈感 & 發想
* 2D 恐怖風格第三人稱遊戲
* 結合視覺小說、文字冒險遊戲的方式推進劇情
* 玩家與三位 NPC 挑戰三個關卡
* 每關將淘汰一位角色，玩家目標活到最後

### 關卡設計
#### 關卡一：碟仙
* 遊戲設計
  * 透過主角與 NPC 之間的對話，製作像是視覺小說、文字冒險遊戲的遊戲體驗，並以主角選擇的對話方向推進劇情
* 操作說明
  * 左右方向鍵操控主角移動、空白鍵跳躍
  * 使用滑鼠點選對話想回應的文字
  * 觸碰到黑洞（Scene1-1）或是門（Scene1-2）將會切換場景
* 碟仙（Scene）通關的正確對話選擇
  *  `Celeste: Do you want to play DieXian?`：選擇 `Yes`
  *  `DieXian: Who do you hate more?`：選擇 `No, I love them!`
  *  `DieXian: Hate!! Or DIE`：選擇 `Em...Hate Luna...`
* 備註
  * 碟仙過程中若主角死亡，此時按下 `Enter` 可回到開始畫面重新遊戲

#### 關卡二：迷陣
* 操作手冊
  1. 玩家碰到牆壁或敵人即會死亡，恐怖影片播放完畢後點擊畫面任意處即可重新開始。
  2. [快速通關方法影片（測試用）連結](https://youtu.be/3uHxyI81F_s)

#### 關卡三：俄羅斯轉盤
* 遊戲設計
  * 畫面左上角顯示雙方血量，第一回合各2條命，第二回合各4條命。右上方顯示目前子彈數量，實彈和空彈數僅會在填彈時顯示一次，遊戲過程中需要自行計算目前的子彈狀況。道具總共有五種，第二輪開始時會發放給雙方3種道具，每次填彈時會補充至3種，每種各一個不重複，道具說明如下：

| 道具   | 效果                                               |
|:------|:---------------------------------------------------|
| 放大鏡 | 可檢查當前槍膛內是否為實彈或空包彈。               |
| 香菸   | 回復 1 點生命值。                                  |
| 啤酒   | 彈出當前槍膛內的子彈並跳過當前回合。               |
| 手鎗   | 若此發子彈為實彈，則造成 2 點傷害。                |
| 手銬   | 使對手下一回合無法行動。                          |

* 遊戲玩法
  * 開始遊戲 玩家進入第三關後，會有遊戲說明的視窗，點擊”I Agree”按鈕即可開始遊戲。
  * 填彈 公告實彈和空彈的數量。
  * 射擊 玩家先選擇射擊對象。
    * 射擊結果
    * 實彈：被射擊方扣除1滴血。射擊方交換。
    * 空彈：無造成傷害。若射擊自己，射擊方不變；若射擊對方，射擊方交換。
  * 使用道具(第二輪) 根據情況選擇要使用的道具。
* 勝利條件 先讓對方血量歸零者獲勝。
* 遊戲目標
  * 短期目標：在有限次數的射擊中，儘可能讓對手死亡並避免自己死亡。
  * 最終目標：生存到最後並擊敗對手。

## 素材來源
### 說明
* 大部分素材來源自網路，如主角來自 `Unity Asset Store`、背景圖多來自 `itch.io`。
* 一部分素材來源自生成式 AI，如開頭畫面由 `Freepik` 生成畫面、`Hailuo` 生成影片；角色語音由 `TTSMaker` 生成等等。
* **俄羅斯轉盤遊戲畫面源自《惡魔輪盤》的遊戲畫面，此專案僅作為學習用途，若有侵權疑慮請告知刪除**

### 細項
* 主角
  * [Unity Asset Store](https://assetstore.unity.com/)
* 圖片
  * 網路素材
    * [itch.io](https://itch.io/game-assets)
    * [opengameart](https://opengameart.org/)
    * [愛給網](https://www.aigei.com/game2d/lib/rpg_kong_b/?utm_source=chatgpt.com)
  * 生成式 AI 合成
    * [Ideogram](https://ideogram.ai/)
    * [Freepik](https://www.freepik.com/)
* 聲音
  * 網路素材
    * [opengameart](https://opengameart.org/)
  * 生成式 AI 合成
    * 背景音樂：[SUNO](https://suno.com/)
    * 角色語音：[TTSMaker](https://ttsmaker.com/zh-hk)
* 影片
  * 網路素材：惡魔輪盤遊戲畫面
  * 生成式 AI 合成
    * [Hailuo AI](https://hailuoai.video/)