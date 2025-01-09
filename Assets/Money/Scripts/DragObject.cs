using UnityEngine;

public class DragObject : MonoBehaviour
{
    private bool isDragging = false;

    /// <summary>
    /// 強制停止拖曳(可在切換關卡或死亡時呼叫)
    /// </summary>
    public void ForceStopDrag()
    {
        isDragging = false;
    }

    void OnMouseDown()
    {
        isDragging = true;
        Debug.Log("MouseDown detected!");
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f; 
            transform.position = mousePos;
        }
    }
}
