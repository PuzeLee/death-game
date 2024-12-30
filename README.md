# death-game
 Window Programming Final Project

## Pull / Branch
* 

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
