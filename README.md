# death-game
 Window Programming Final Project

## Pull / Branch
1. Clone 一份新專案
   * 網頁版 GitHub 複製連結
       * ![網頁版 GitHub 複製連結](README_files/螢幕擷取畫面%202024-12-31%20005720.png)
   * GitHub Desktop File Clone
       * ![GitHub Desktop File Clone](README_files/螢幕擷取畫面%202024-12-31%20010001.png)
       * ![GitHub Desktop File Clone](README_files/螢幕擷取畫面%202024-12-31%20010112.png)
2. 新增 Branch
   1. 用 VScode 打開專案資料夾，並開啟 Terminal
   2. 在本地建立一個新分支並切換到該分支 `git checkout -b <branchName>`
		* e.g. `git checkout -b Liu`
   3. 推送本地分支到遠端並建立關聯 `git push -u origin <branchName>`
		* e.g. `git push -u origin Liu`
   4. 確認本地分支是否與遠端分支關聯成功
        * git branch -vv
        * ![git branch -vv](README_files/螢幕擷取畫面%202024-12-31%20010428.png)
        * 看到這個畫面應該就可以了
        * 之後就 commit / push / commit / push...


## 資料夾結構
* Assets
    * **Lee** // 除了 Sprite, Resources 以外的所有檔案我都放在這邊
    * **Liu**
    * **Money**
    * Resources // audio 放在這裡，這邊可能會打架，之後再說
    * Settings // 應該不會動到
    * Sprites
        * **Lee**
        * **Liu**
        * **Money**

* 我的習慣是會用到的素材會複製一份到 Sprites 中

## 使用說明
* Prefabs
    * Lee 的 Prefabs 放在 `Assets/Lee/Prefabs/` 裡面
    * 先放一份到 Scene 中
    * 再把 Scene 中的物件拉到自己的資料夾的 Prefabs 中 (e.g. `Assets/Liu/Prefabs/`)
    * 跳出 `Create Prefab or Variant`，選擇 Original Prefab，取消和原本物件的關聯
    * 如果 Prefabs 有用到 Scripts，怕被我改到的話，建議把對應的 Scripts 複製到自己的資料夾下改檔名
        * （e.g. `CameraController` -> `CameraControllerLiu`）
        * 記得 Scripts 內開頭 public CameraController : MonoBehaviour 也要改成新的檔名
        * 新的 Prefab 改用新的 Scripts
        * （剩下的打架了再說 X）
